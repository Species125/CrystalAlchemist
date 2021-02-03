using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace CrystalAlchemist
{
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

            public bool overrideDuration = false;

            [ShowIf("overrideDuration")]
            public float maxDuration = 0f;

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
                    if (this.delete) Destroy(this.spawnPoint, 0.3f);
                }
            }
        }

        [InfoBox("Photon View required on this!", InfoMessageType.Warning)]
        [SerializeField]
        [BoxGroup("Children")]
        private float startDelay = 0f;

        [SerializeField]
        [HideLabel]
        [BoxGroup("Children")]
        private ChildSequenceProperty childProperty;

        private float timeLeftToSpawnNext;
        private bool isCanceld = false;
        private bool isRunning = true;

        private List<SequenceObject> sequences = new List<SequenceObject>();

        private void Awake()
        {
            GameEvents.current.OnInterrupt += InterruptSequence;
            if (PhotonView.Get(this) == null) Debug.LogError("PhotonView is missing on " + this.gameObject.name);
        }

        private void Start()
        {
            this.childProperty.AddSpawnPoints(this.transform);
            this.timeLeftToSpawnNext = this.startDelay;
        }

        private void OnDestroy()
        {
            GameEvents.current.OnInterrupt -= InterruptSequence;
        }

        private void InterruptSequence()
        {
            this.isCanceld = true;
            this.isRunning = false;
            this.sequences.Clear();
            this.enabled = false;
            this.gameObject.SetActive(false);
        }

        private void Update()
        {
            if (this.isCanceld) return;
            if (this.isRunning) AddSequences(); //Add new sequence 

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
            foreach (GameObject spawnpoint in GetSpawnPosition(this.childProperty))
            {
                this.sequences.Add(new SequenceObject(spawnpoint, this.childProperty.repeat, this.childProperty.repeatDelay, this.childProperty.GetDelete()));    
            }

            this.timeLeftToSpawnNext = this.childProperty.spawnDelay;
            this.counter++;
            if (this.counter >= this.childProperty.GetMax()) this.isRunning = false;
        }

         private void Instantiate(Vector2 position, Quaternion rotation)
         {
            if (!NetworkUtil.IsMaster()) return;

            if (this.childProperty.spawnObject.GetType() == typeof(GameObject))
            {
                GameObject prefab = childProperty.spawnObject as GameObject;

                if (prefab.GetComponent<Character>()) 
                    SpawnCharacter(prefab.GetComponent<Character>(), position, rotation);
                else if (prefab.GetComponent<AI>()) 
                    SpawnAI(prefab.GetComponent<AI>(), position, rotation);
                else if (prefab.GetComponent<AddSpawn>()) 
                    SpawnAdd(prefab.GetComponent<AddSpawn>(), position, rotation);
                else if (prefab.GetComponent<Skill>()) 
                    SpawnSkill(prefab.GetComponent<Skill>(), position, rotation);
                else if (prefab.GetComponent<NetworkBehaviour>()) 
                    SpawnNetworkObject(prefab.GetComponent<NetworkBehaviour>(), position, rotation);
            }
            else if (this.childProperty.spawnObject.GetType() == typeof(Ability))
            {
                Ability ability = this.childProperty.spawnObject as Ability;
                SpawnAbility(ability, position, rotation);
            }
        }

        private void SpawnNetworkObject(NetworkBehaviour obj, Vector2 position, Quaternion rotation)
        {
            NetworkEvents.current.RaiseBossObjectSpawnEvent(obj, this.gameObject, position, rotation,
                                                            this.childProperty.overrideDuration, this.childProperty.maxDuration);
        }

        private void SpawnCharacter(Character character, Vector2 position, Quaternion rotation)
        {
            NetworkEvents.current.RaiseBossCharacterSpawnEvent(character, this.gameObject, position, rotation,
                                                               this.childProperty.overrideDuration, this.childProperty.maxDuration);
        }

        private void SpawnAI(AI character, Vector2 position, Quaternion rotation)
        {
            NetworkEvents.current.RaiseBossAISpawnEvent(character, this.target, this.gameObject, position, rotation,
                                                        this.childProperty.overrideDuration, this.childProperty.maxDuration);
        }

        private void SpawnAdd(AddSpawn addSpawn, Vector2 position, Quaternion rotation)
        {
            NetworkEvents.current.RaiseBossAddSpawnEvent(addSpawn, this.target, this.gameObject, position, rotation,
                                                        this.childProperty.overrideDuration, this.childProperty.maxDuration);
        }

        private void SpawnSkill(Skill skill, Vector2 position, Quaternion rotation)
        {
            NetworkEvents.current.RaiseBossSkillSpawnEvent(skill, this.sender, this.target, this.gameObject, 
                                                           position, rotation, this.childProperty.overrideDuration,
                                                           this.childProperty.maxDuration, this.childProperty.AddTarget);
        }

        private void SpawnAbility(Ability ability, Vector2 position, Quaternion rotation)
        {
            NetworkEvents.current.RaiseBossAbilitySpawnEvent(ability, this.sender, this.target, this.gameObject,
                                                             position, rotation, this.childProperty.overrideDuration,
                                                             this.childProperty.maxDuration, this.childProperty.AddTarget);
        }
    }
}
