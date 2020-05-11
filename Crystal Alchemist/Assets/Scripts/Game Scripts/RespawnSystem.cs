﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class RespawnSystem : MonoBehaviour
{
    private enum SpawnType
    {
        none,
        day,
        night
    }

    [System.Serializable]
    private class RespawnTimer
    {
        public float timeElapsed;
        public Character character;
        public bool spawnIt;

        public RespawnTimer(Character character)
        {
            this.character = character;
            if (this.character.values == null) this.character.values = new CharacterValues();
            this.character.values.currentState = CharacterState.respawning;

            this.timeElapsed = character.stats.respawnTime;
        }

        public void Updating(float time)
        {
            if (this.timeElapsed > 0) this.timeElapsed -= time;
            else this.spawnIt = true;
        }
    }

    [SerializeField]
    private TimeValue time;

    [SerializeField]
    private float updateTime = 1f;

    [SerializeField]
    private SpawnType spawnType = SpawnType.none;

    [BoxGroup("Debug")]
    [SerializeField]
    private List<RespawnTimer> respawnObjects = new List<RespawnTimer>();

    private void Start() => InvokeRepeating("Updating", 0f, this.updateTime);    

    private bool NotActive()
    {
        return (this.spawnType == SpawnType.day && time.night)
         || (this.spawnType == SpawnType.night && !time.night);
    }

    private void DisableGameObjects()
    {
        for (int i = 0; i < this.transform.childCount; i++)
        {
            GameObject child = this.transform.GetChild(i).gameObject;
            Character character = child.gameObject.GetComponent<Character>();
            if (character != null) child.SetActive(false);            
        }          
    }

    private void Updating()
    {
        if (NotActive()) DisableGameObjects(); //set characters inactive
        SetRespawnObjects(); //Add inactive characters to list
        UpdateRespawnObjects(); //update timer of listed characters
        if (!NotActive()) SpawnObjects(); //spawn characters      
    }

    private void SetRespawnObjects()
    {
        for (int i = 0; i < this.transform.childCount; i++)
        {
            GameObject child = this.transform.GetChild(i).gameObject;
            Character character = child.gameObject.GetComponent<Character>();

            if (character != null 
                && !character.gameObject.activeInHierarchy
                && !this.Contains(character)
                && character.stats.hasRespawn) this.respawnObjects.Add(new RespawnTimer(character));
        }
    }

    private void UpdateRespawnObjects()
    {
        for (int i = 0; i < this.respawnObjects.Count; i++)
        {
            if (!this.respawnObjects[i].spawnIt) respawnObjects[i].Updating(this.updateTime);
        }
    }

    private void SpawnObjects()
    {
        for (int i = 0; i < this.respawnObjects.Count; i++)
        {
            if (this.respawnObjects[i].spawnIt)
            {
                respawnCharacter(this.respawnObjects[i].character);
                this.respawnObjects[i] = null;
            }
        }
        this.respawnObjects.RemoveAll(x => x == null);
    }

    private bool Contains(Character character)
    {
        for(int i = 0; i < this.respawnObjects.Count; i++)
        {
            if(this.respawnObjects[i].character == character) return true;            
        }
        return false;
    }

    private void respawnCharacter(Character character)
    {
        character.gameObject.SetActive(true);
        character.values.currentState = CharacterState.respawning;

        if (character.stats.respawnAnimation != null)
        {
            //spawn character after animation
            RespawnAnimation respawnObject = Instantiate(character.stats.respawnAnimation, character.GetSpawnPosition(), Quaternion.identity);
            respawnObject.Initialize(character);
            character.SetCharacterSprites(false);
        }
        else
        {
            //spawn character immediately
            character.SetCharacterSprites(true);
            character.PlayRespawnAnimation();
            character.SpawnIn();
        }
    }
}

