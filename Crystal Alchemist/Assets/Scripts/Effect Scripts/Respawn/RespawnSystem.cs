using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Photon.Pun;

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

    private void Start()
    {
        GameEvents.current.OnPlayerSpawned += this.OnStart;
    }

    private void OnDestroy()
    {
        GameEvents.current.OnPlayerSpawned -= this.OnStart;
    }

    private void OnStart(GameObject gameObject)
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
                NetworkEvents.current.ShowGameObject(this.respawnObjects[i].gameObject, this.isInit);
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
                NetworkEvents.current.HideGameObject(this.transform.GetChild(i).gameObject, this.isInit);
        }
    }

    private bool Contains(GameObject gameObject)
    {
        for(int i = 0; i < this.respawnObjects.Count; i++)
        {
            if(this.respawnObjects[i].gameObject == gameObject) return true;            
        }
        return false;
    }
}

