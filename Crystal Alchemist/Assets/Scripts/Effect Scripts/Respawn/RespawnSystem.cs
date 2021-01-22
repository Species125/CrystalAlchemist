using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using ExitGames.Client.Photon;
using Photon.Pun;
using System.Collections;
using Photon.Realtime;

namespace CrystalAlchemist
{
    public class RespawnSystem : MonoBehaviour
    {
        private enum SpawnType
        {
            none,
            day,
            night,
            time
        }

        [System.Serializable]
        private class RespawnTimer
        {
            public float timeElapsed;
            public GameObject gameObject;
            public bool spawnIt = false;

            public RespawnTimer(GameObject gameObject)
            {
                this.gameObject = gameObject;

                Character character = this.gameObject.GetComponent<Character>();

                if (character != null)
                {
                    if (character.values == null) character.values = new CharacterValues();
                    character.values.currentState = CharacterState.respawning;
                    this.timeElapsed = character.stats.respawnTime;
                }
            }

            public void Updating(float time)
            {
                if (!this.spawnIt)
                {
                    if (this.timeElapsed > 0) this.timeElapsed -= time;
                    else
                    {
                        Character character = this.gameObject.GetComponent<Character>();

                        if (character != null)
                        {
                            if (Random.Range(1, 100) <= character.stats.respawnChance) this.spawnIt = true;
                            else this.timeElapsed = character.stats.respawnTime;
                        }
                        else this.spawnIt = true;
                    }
                }
            }

            public void SetSpawnImmediately()
            {
                Character character = this.gameObject.GetComponent<Character>();
                this.timeElapsed = 0;

                if (character != null)
                {
                    if (Random.Range(1, 100) <= character.stats.respawnChance) this.spawnIt = true;
                    else this.timeElapsed = character.stats.respawnTime;
                }
                else this.spawnIt = true;
            }
        }

        [SerializeField]
        private TimeValue time;

        [SerializeField]
        private float updateTime = 1f;

        [SerializeField]
        private SpawnType spawnType = SpawnType.none;

        [SerializeField]
        [ShowIf("spawnType", SpawnType.time)]
        private int from;

        [SerializeField]
        [ShowIf("spawnType", SpawnType.time)]
        private int to;

        [BoxGroup("Debug")]
        [SerializeField]
        private List<RespawnTimer> respawnObjects = new List<RespawnTimer>();

        private bool isActive = false;
        private bool isInit = true;

        private void OnEnable()
        {
            PhotonNetwork.NetworkingClient.EventReceived += NetworkingEvent;
        }

        private void Start()
        {
            GameEvents.current.OnPlayerSpawned += this.OnStart;
        }

        private void OnDisable()
        {
            PhotonNetwork.NetworkingClient.EventReceived -= NetworkingEvent;
        }

        private void OnDestroy()
        {
            GameEvents.current.OnPlayerSpawned -= this.OnStart;
        }

        private void OnStart(int ID)
        {
            if (!NetworkUtil.IsMaster()) return;
            InvokeRepeating("Updating", 0f, this.updateTime);
        }

        private void SetIsActive()
        {
            bool result = ((this.spawnType == SpawnType.none)
                           || (this.spawnType == SpawnType.day && !time.night)
                           || (this.spawnType == SpawnType.night && time.night)
                           || (this.spawnType == SpawnType.time && this.from >= time.getHour() && this.to <= time.getHour()));

            if (result != this.isActive)
            {
                this.isActive = result;
                RespawnAll();
            }
        }

        private bool MustDespawn(GameObject child)
        {
            return (child != null && child.gameObject.activeInHierarchy);
        }

        private void Updating()
        {
            if (this.gameObject.activeInHierarchy) //stops system when not active
            {
                SetIsActive();
                if (!this.isActive) DisableGameObjects(); //set characters inactive
                SetRespawnObjects(); //Add inactive characters to list
                UpdateRespawnObjects(); //update timer of listed characters
                if (this.isActive) SpawnObjects(); //spawn characters    

                if (this.isInit) this.isInit = false;
            }
        }

        private void SetRespawnObjects()
        {
            for (int i = 0; i < this.transform.childCount; i++)
            {
                GameObject child = this.transform.GetChild(i).gameObject;

                if (!child.activeInHierarchy
                    && !this.Contains(child)
                    && ((child.GetComponent<Character>() != null && child.GetComponent<Character>().stats.hasRespawn)
                        || child.GetComponent<Character>() == null)) this.respawnObjects.Add(new RespawnTimer(child));
            }
        }

        private void UpdateRespawnObjects()
        {
            for (int i = 0; i < this.respawnObjects.Count; i++)
            {
                if (!this.respawnObjects[i].spawnIt) respawnObjects[i].Updating(this.updateTime);
            }
        }

        private void RespawnAll()
        {
            for (int i = 0; i < this.respawnObjects.Count; i++)
            {
                if (!this.respawnObjects[i].spawnIt) respawnObjects[i].SetSpawnImmediately();
            }
        }


        private void SpawnObjects()
        {
            for (int i = 0; i < this.respawnObjects.Count; i++)
            {
                if (this.respawnObjects[i].spawnIt)
                {
                    ShowGameObjectEvent(this.respawnObjects[i].gameObject, this.isInit);
                    this.respawnObjects[i] = null;
                }
            }
            this.respawnObjects.RemoveAll(x => x == null);
        }

        private void DisableGameObjects()
        {
            for (int i = 0; i < this.transform.childCount; i++)
            {
                if (MustDespawn(this.transform.GetChild(i).gameObject))
                    HideGameObjectEvent(this.transform.GetChild(i).gameObject, this.isInit);
            }
        }

        private bool Contains(GameObject gameObject)
        {
            for (int i = 0; i < this.respawnObjects.Count; i++)
            {
                if (this.respawnObjects[i].gameObject == gameObject) return true;
            }
            return false;
        }


        public void HideGameObjectEvent(GameObject gameObject, bool isInit)
        {
            if (!NetworkUtil.IsMaster() || gameObject == null) return;

            int ID = gameObject.GetPhotonView().ViewID;

            object[] datas = new object[] { ID, isInit };

            RaiseEventOptions options = new RaiseEventOptions()
            {
                Receivers = ReceiverGroup.All
            };

            PhotonNetwork.RaiseEvent(NetworkUtil.HIDE_CHARACTER, datas, options, SendOptions.SendUnreliable);
        }

        public void ShowGameObjectEvent(GameObject gameObject, bool isInit)
        {
            if (!NetworkUtil.IsMaster() || gameObject == null) return;

            int ID = gameObject.GetPhotonView().ViewID;

            object[] datas = new object[] { ID, isInit };

            RaiseEventOptions options = new RaiseEventOptions()
            {
                Receivers = ReceiverGroup.All
            };

            PhotonNetwork.RaiseEvent(NetworkUtil.SHOW_CHARACTER, datas, options, SendOptions.SendUnreliable);
        }

        private void NetworkingEvent(EventData obj)
        {
            if (obj.Code == NetworkUtil.HIDE_CHARACTER)
            {
                object[] datas = (object[])obj.CustomData;
                int ID = (int)datas[0];
                bool isInit = (bool)datas[1];

                GameObject gameObject = NetworkUtil.GetGameObject(ID);
                if (gameObject != null) HideGameObject(gameObject, isInit);
            }
            else if (obj.Code == NetworkUtil.SHOW_CHARACTER)
            {
                object[] datas = (object[])obj.CustomData;
                int ID = (int)datas[0];
                bool isInit = (bool)datas[1];

                GameObject gameObject = NetworkUtil.GetGameObject(ID);
                if (gameObject != null) ShowGameObject(gameObject, isInit);
            }
        }

        private void HideGameObject(GameObject gameObject, bool isInit)
        {
            Character character = gameObject.GetComponent<Character>();

            if (character != null)
            {
                if (character.respawnAnimation != null)
                {
                    RespawnAnimation respawnObject = Instantiate(character.respawnAnimation, character.GetSpawnPosition(), Quaternion.identity);
                    respawnObject.Reverse(character);
                    character.SetCharacterSprites(true);

                    StartCoroutine(InactiveCo(respawnObject.getAnimationLength(), character.gameObject));
                    StartCoroutine(InactiveCo(respawnObject.getAnimationLength(), respawnObject.gameObject));
                }
                else
                {
                    if (!isInit) character.PlayDespawnAnimation();
                    character.SpawnOut();
                    StartCoroutine(InactiveCo(character.GetDespawnLength(), character.gameObject));
                }

                character.values.currentState = CharacterState.respawning;
            }
            else
            {
                SetGameObjectActive(gameObject, false, isInit);
            }
        }

        private IEnumerator InactiveCo(float seconds, GameObject gameObject)
        {
            yield return new WaitForSeconds(seconds);
            gameObject.SetActive(false);
        }

        private void ShowGameObject(GameObject gameObject, bool isInit)
        {
            Character character = gameObject.GetComponent<Character>();

            if (character != null)
            {
                character.gameObject.SetActive(true);
                character.values.currentState = CharacterState.respawning;

                if (character.respawnAnimation != null)
                {
                    //spawn character after animation
                    RespawnAnimation respawnObject = Instantiate(character.respawnAnimation, character.GetSpawnPosition(), Quaternion.identity);
                    respawnObject.Initialize(character);
                    character.SetCharacterSprites(false);
                }
                else
                {
                    //spawn character immediately
                    character.SetCharacterSprites(true);
                    if (!isInit) character.PlayRespawnAnimation();
                    character.SpawnIn();
                }
            }
            else
            {
                SetGameObjectActive(gameObject, true, isInit);
            }
        }

        private void SetGameObjectActive(GameObject gameObject, bool value, bool isInit)
        {
            //Prevent Effect on Initialize
            if (gameObject.GetComponent<Collectable>() != null)
            {
                gameObject.GetComponent<Collectable>().SetBounce(!isInit, Vector2.zero);
                gameObject.GetComponent<Collectable>().SetSmoke(!isInit);
            }
            else if (gameObject.GetComponent<Interactable>() != null)
                gameObject.GetComponent<Interactable>().SetSmoke(!isInit);

            gameObject.SetActive(value);
        }
    }
}

