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


        private void OnEnable()
        {
            NetworkEvents.current.OnPlayerSpawned += this.OnStart;
            PhotonNetwork.NetworkingClient.EventReceived += NetworkingEvent;
        }

        private void OnDisable()
        {
            NetworkEvents.current.OnPlayerSpawned -= this.OnStart;
            PhotonNetwork.NetworkingClient.EventReceived -= NetworkingEvent;
        }

        private void OnStart(PhotonView view)
        {
            if (!NetworkUtil.IsMaster()) return;
            InvokeRepeating("Updating", 0f, this.updateTime);
        }

        private void SetIsActive()
        {
            bool result = ((this.spawnType == SpawnType.none)
                           || (this.spawnType == SpawnType.day && !time.night)
                           || (this.spawnType == SpawnType.night && time.night)
                           || (this.spawnType == SpawnType.time && this.from >= time.GetHour() && this.to <= time.GetHour()));

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
                    ShowGameObjectEvent(this.respawnObjects[i].gameObject);
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
                    HideGameObjectEvent(this.transform.GetChild(i).gameObject);
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


        public void HideGameObjectEvent(GameObject gameObject)
        {
            NetworkUtil.NetworkEvent(gameObject, NetworkUtil.HIDE_CHARACTER, NetworkUtil.TargetAll());
        }

        public void ShowGameObjectEvent(GameObject gameObject)
        {
            NetworkUtil.NetworkEvent(gameObject, NetworkUtil.SHOW_CHARACTER, NetworkUtil.TargetAll());
        }

        private void NetworkingEvent(EventData obj)
        {
            if (obj.Code == NetworkUtil.HIDE_CHARACTER)
            {
                object[] datas = (object[])obj.CustomData;
                int ID = (int)datas[0];
                string objectName = (string)datas[1];
                Vector2 position = (Vector2)datas[2];

                GameObject gameObject = GetGameObject(ID, objectName, position);
                if (gameObject != null) HideGameObject(gameObject);
            }
            else if (obj.Code == NetworkUtil.SHOW_CHARACTER)
            {
                object[] datas = (object[])obj.CustomData;
                int ID = (int)datas[0];
                string objectName = (string)datas[1];
                Vector2 position = (Vector2)datas[2];

                GameObject gameObject = GetGameObject(ID, objectName, position);
                if (gameObject != null) ShowGameObject(gameObject);
            }
        }

        private void HideGameObject(GameObject gameObject)
        {
            Character character = gameObject.GetComponent<Character>();

            if (character != null)
            {
                if (character.respawnAnimation != null)
                {
                    RespawnAnimation respawnObject = Instantiate(character.respawnAnimation, character.GetSpawnPosition(), Quaternion.identity);
                    respawnObject.Reverse(character);
                    character.SetCharacterSprites(true);
                }
                else
                {
                    if (GameManager.current.loadingCompleted) character.PlayDespawnAnimation();
                    character.SpawnOut();
                }

                character.values.currentState = CharacterState.respawning;
            }
            else
            {
                gameObject.SetActive(false);
            }
        }

        private void ShowGameObject(GameObject gameObject)
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
                    if(GameManager.current.loadingCompleted) character.PlayRespawnAnimation();
                    character.SpawnIn();
                }
            }
            else
            {
                gameObject.SetActive(true);
            }
        }

        private GameObject GetGameObject(int ID, string objectName, Vector2 position)
        {
            GameObject networkGameObject = NetworkUtil.GetGameObject(ID);
            if (networkGameObject != null) return networkGameObject;

            for (int i = 0; i < this.transform.childCount; i++)
            {
                if (this.transform.GetChild(i).gameObject.name == objectName 
                    && (Vector2)this.transform.GetChild(i).gameObject.transform.position == position) 
                    return this.transform.GetChild(i).gameObject;
            }

            return null;
        }
    }
}

