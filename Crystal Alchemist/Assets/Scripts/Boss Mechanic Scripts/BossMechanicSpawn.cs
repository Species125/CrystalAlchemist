﻿using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

public class BossMechanicSpawn : BossMechanicProperty
{
    [System.Serializable]
    public class ChildSequenceProperty : SequenceProperty
    {
        public Object spawnObject;

        [ShowIf("spawnObject")]
        [HideIf("spawnPositonType", SpawnPositionType.spawnPoints)]
        [MinValue(1)]
        public int amount = 1;

        [Tooltip("Set to true, if skill needs to know the target")]
        public bool AddTarget = false;

        [ShowIf("spawnObject")]
        [Tooltip("Time between Spawns")]
        public float spawnDelay = 0f;

        [ShowIf("spawnObject")]
        [MinValue(0)]
        public int repeat = 0;

        [ShowIf("spawnObject")]
        [HideIf("repeat", 0)]
        [Tooltip("Time between Spawns")]
        [MinValue(0.1)]
        public float repeatDelay = 1f;

        public override int GetMax()
        {
            if (this.spawnPositonType == SpawnPositionType.spawnPoints) return this.spawnPoints.Count;
            else return this.amount;
        }
    }

    [System.Serializable]
    public class SequenceObject
    {
        private int amount = 1;
        private float delay = 0;
        public bool isRunning = true;
        public bool spawnIt = false;
        public GameObject spawnPoint;
        private float elapsed;
        private bool delete;

        public SequenceObject(GameObject spawnPoint, int amount, float delay, bool delete)
        {
            this.spawnPoint = spawnPoint;
            this.amount = amount;
            this.delay = delay;
            this.delete = delete;
        }

        public void Updating(float time)
        {
            if (this.elapsed <= 0) this.spawnIt = true;
            else this.elapsed -= time;
        }

        public void SetNext()
        {
            this.spawnIt = false;
            this.elapsed = this.delay;
            this.amount--;
            if (this.amount <= 0)
            {
                this.isRunning = false;
                if(this.delete) Destroy(this.spawnPoint, 0.3f);
            }
        }
    }

    [SerializeField]
    [BoxGroup("Children")]
    private float startDelay = 0f;

    [SerializeField]
    [HideLabel]
    [BoxGroup("Children")]
    private ChildSequenceProperty childProperty;

    private float timeLeftToSpawnNext;

    private bool isRunning = true;

    private List<SequenceObject> sequences = new List<SequenceObject>();
    

    private void Start()
    {
        this.childProperty.AddSpawnPoints(this.transform);
        this.timeLeftToSpawnNext = this.startDelay;
    }
    
    private void Update()
    {
        if(this.isRunning) AddSequences(); //Add new sequence 

        UpdatingSequences(); //update existing sequences

        if (!this.isRunning && this.sequences.Count == 0) this.enabled = false; //deactivate when all sequences are done
    }

    private void UpdatingSequences()
    {
        foreach (SequenceObject sequence in this.sequences)
        {
            if (sequence.isRunning)
            {
                sequence.Updating(Time.deltaTime);

                if (sequence.spawnIt)
                {
                    Quaternion rotation = GetRotation(this.childProperty.rotationType, this.childProperty.rotationFactor, sequence.spawnPoint, this.childProperty.GetOffset());
                    Instantiate(sequence.spawnPoint.transform.position, rotation);
                    sequence.SetNext();
                }
            }        
        }

        this.sequences.RemoveAll(x => x.isRunning == false);
    }

    private void AddSequences()
    {
        this.timeLeftToSpawnNext -= Time.deltaTime;
        if (this.timeLeftToSpawnNext <= 0 && this.childProperty.spawnObject != null) AddSequence();
    }

    private void AddSequence()
    {
        GameObject spawnPoint = GetSpawnPosition(this.childProperty);

        this.sequences.Add(new SequenceObject(spawnPoint, this.childProperty.repeat, this.childProperty.repeatDelay, this.childProperty.GetDelete()));
        this.timeLeftToSpawnNext = this.childProperty.spawnDelay;
        this.counter++;
        if (this.counter >= this.childProperty.GetMax()) this.isRunning = false;
    }

    private void Instantiate(Vector2 position, Quaternion rotation)
    {
        Character target = null;

        if (this.childProperty.AddTarget) target = this.target;

        if (this.childProperty.spawnObject.GetType() == typeof(GameObject))
        {
            GameObject spawnedObject = Instantiate(this.childProperty.spawnObject, position, Quaternion.identity, this.transform) as GameObject;

            SetSkill(spawnedObject.GetComponent<Skill>(), target, rotation);
            SetAdd(spawnedObject.GetComponent<AI>(), target);
        }
        else if (this.childProperty.spawnObject.GetType() == typeof(Ability))
        {
            Ability ability = Instantiate(this.childProperty.spawnObject) as Ability;
            Skill skill = ability.InstantiateSkill(target, position, this.sender, rotation);
            skill.transform.SetParent(this.transform);
            Destroy(ability);
        }
    }

    private void SetAdd(AI character, Character target)
    {
        character.IsSummoned = true;
        character.stats = Instantiate(character.stats);
        character.stats.hasRespawn = false;
        character.target = target;
        character.transform.SetParent(null);
    }

    private void SetSkill(Skill skill, Character target, Quaternion rotation)
    {
        if (skill != null)
        {
            skill.transform.rotation = rotation;
            skill.sender = this.sender;
            skill.target = target;
        }
    }
}
