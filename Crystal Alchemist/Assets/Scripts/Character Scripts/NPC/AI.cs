using DG.Tweening;
using Photon.Pun;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace CrystalAlchemist
{
    public class AI : NonPlayer
    {
        [BoxGroup("Pflichtfelder")]
        [Required]
        public AggroStats aggroStats;

        [BoxGroup("Pflichtfelder")]
        [ReadOnly]
        [SerializeField]
        private int mainTargetID;

        [BoxGroup("Pflichtfelder")]
        [ReadOnly]
        [SerializeField]
        private List<string> aggro = new List<string>();
        

        [BoxGroup("Events")]
        [SerializeField]
        public UnityEvent onTarget;

        [BoxGroup("Events")]
        [SerializeField]
        private List<AI> alertPartners = new List<AI>();

        [BoxGroup("Debug")]
        [ReadOnly]
        public Character partner;

        [BoxGroup("Debug")]
        [ReadOnly]
        public bool rangeTriggered;

        private float needed = 1;
        private bool isSleeping = true;
        private AggroClue activeClue;
        private Dictionary<int, float[]> aggroList = new Dictionary<int, float[]>();

        public override void Awake()
        {
            base.Awake();
            ClearMainTarget();
        }

        public override void OnEnable()
        {
            base.OnEnable();
            this.aggroList.Clear();
        }

        public override void OnDisable()
        {
            base.OnDisable();
            this.ClearMainTarget();
            this.HideClue();
            this.aggroList.Clear();            
        }

        #region Animation, StateMachine

        public int GetMainTargetID()
        {
            return this.mainTargetID;
        }

        public void SetMainTargetID(int ID)
        {
            if (this.mainTargetID != ID)
            {
                this.mainTargetID = ID;

                if (!this.aggroList.ContainsKey(ID)) this.AddToAggroList(ID, 100, 100);
                else this.UpdateAggro(ID, 1);

                this.onTarget?.Invoke();
                foreach (AI npc in this.alertPartners) npc.SetMainTargetID(ID);
            }
        }

        public void ClearMainTarget()
        {
            if (this.mainTargetID != 0) this.mainTargetID = 0;
        }
        
        public bool HasMainTarget()
        {
            return this.mainTargetID > 0;
        }       
        
        public void InitializeAddSpawn(int target, bool hasMaxDuration, float maxDuration)
        {
            this.InitializeAddSpawn(hasMaxDuration, maxDuration);
            this.mainTargetID = target;
        }

        public Character GetTarget()
        {
            return NetworkUtil.GetCharacter(this.mainTargetID);
        }

        public Character GetTarget(int index)
        {
            return NetworkUtil.GetCharacter(this.aggroList.ElementAt(index).Key);
        }

        public List<Character> GetTargets()
        {
            List<Character> targets = new List<Character>();

            foreach(KeyValuePair<int,float[]> elem in this.aggroList)
            {
                Character character = NetworkUtil.GetCharacter(elem.Key);
                if (character != null) targets.Add(character);
            }

            return targets;
        }

        public override void Start()
        {
            base.Start();

            //if (!NetworkUtil.IsMaster()) return;

            GameEvents.current.OnRangeTriggered += SetRangeTriggered;

            this.GetComponent<AICombat>().Initialize();
            AIComponent[] components = this.GetComponents<AIComponent>();
            for (int i = 0; i < components.Length; i++) components[i].Initialize();
        }

        public override void OnDestroy()
        {
            //if (!NetworkUtil.IsMaster()) return;
            base.OnDestroy();
            GameEvents.current.OnRangeTriggered -= SetRangeTriggered;
        }

        private void SetRangeTriggered(Character character, bool value)
        {
            if (character == this) this.rangeTriggered = value;
        }

        public override void Update()
        {
            base.Update();

            GenerateAggro();

            this.aggro.Clear();

            foreach(KeyValuePair<int, float[]> keypair in this.aggroList)
            {
                this.aggro.Add(keypair.Key + " -> Amount: " + keypair.Value[0] + " Factor: " + keypair.Value[1]);
            }

            //if (!NetworkUtil.IsMaster()) return;

            if (this.HasMainTarget() && this.isSleeping)
            {
                AnimatorUtil.SetAnimatorParameter(this.animator, "WakeUp");
                this.isSleeping = false;
            }
            else if (!this.HasMainTarget() && !this.isSleeping)
            {
                AnimatorUtil.SetAnimatorParameter(this.animator, "Sleep");
                this.isSleeping = true;
            }

            this.GetComponent<AICombat>().Updating();
            AIComponent[] components = this.GetComponents<AIComponent>();
            for (int i = 0; i < components.Length; i++) components[i].Updating();
        }

        public override void Regenerate()
        {
            if (!NetworkUtil.IsMaster()) return;
            RegenerateLifeMana();
        }

        public void changeState(CharacterState newState)
        {
            //if (!NetworkUtil.IsMaster()) return;

            if (this.values.currentState != newState) this.values.currentState = newState;
        }

        public void MoveCharacters(Vector2 position, float duration)
        {
            if (!NetworkUtil.IsMaster()) return;

            this.myRigidbody.DOMove(position, duration);
        }

        #endregion


        public void SetMaxAggro()
        {
            //if (!NetworkUtil.IsMaster()) return;

            foreach (Photon.Realtime.Player p in PhotonNetwork.PlayerList)
            {
                Player player = (Player)p.TagObject;
                SetMainTargetID(player.photonView.ViewID);
            }
        }

        public override void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            base.OnPhotonSerializeView(stream, info);

            if (stream.IsWriting)
            {
                stream.SendNext(this.aggroList);
                stream.SendNext(this.mainTargetID);
            }
            else
            {
                this.aggroList = (Dictionary<int,float[]>)stream.ReceiveNext();
                this.mainTargetID = (int)stream.ReceiveNext();
            }
        }


        #region Aggro

        private void GenerateAggro()
        {
            List<int> charactersToRemove = new List<int>();

            if (!this.IsValidTarget(this.GetMainTargetID()))
            {
                this.aggroList.Remove(this.GetMainTargetID());
                this.ClearMainTarget();
            }

            foreach (KeyValuePair<int, float[]> entry in this.aggroList)
            {
                int characterID = entry.Key;

                //                       amount                         factor
                this.UpdateAggro(characterID, (this.aggroList[characterID][1] * (Time.deltaTime * this.values.timeDistortion)));

                if (this.aggroList[characterID][0] >= this.needed) //Aggro max, Target!
                {
                    if (!this.HasMainTarget()) StartCoroutine(SwitchTargetCo(0, characterID)); //Hautpziel weg, neues Ziel suchen!
                    else
                    {
                        if (this.aggroList.ContainsKey(this.GetMainTargetID()) //Neues Ziel mehr Aggro als Hauptziel -> Switch
                         && this.aggroList[this.GetMainTargetID()][1] < this.aggroList[characterID][1])
                        {
                            if(this.aggroStats) StartCoroutine(SwitchTargetCo(this.aggroStats.targetChangeDelay, characterID));
                            else StartCoroutine(SwitchTargetCo(0.3f, characterID));
                        }
                    }
                }
                else if (this.aggroList[characterID][0] <= 0)
                {
                    this.aggroList[characterID][0] = 0; //aggro   

                    //Aggro lost, Target lost
                    if (this.GetMainTargetID() == characterID) this.ClearMainTarget();
                    charactersToRemove.Add(characterID);
                    this.HideClue();
                }
            }

            foreach (int character in charactersToRemove) this.RemoveFromAggroList(character);
        }


        private IEnumerator SwitchTargetCo(float delay, int character)
        {
            yield return new WaitForSeconds(delay);

            this.SetMainTargetID(character);
            this.ShowClue(MasterManager.markAttack);
        }


        public void _IncreaseAggroOnHit(Character newTarget, float damage)
        {
            if (aggroStats) _IncreaseAggroOnHit(newTarget, damage, this.aggroStats.aggroOnHitIncreaseFactor, this.aggroStats.firstHitMaxAggro);
            else _IncreaseAggroOnHit(newTarget, damage, 1);
        }

        public void _IncreaseAggro(Character newTarget)
        {
            if (aggroStats) _IncreaseAggro(newTarget, this.aggroStats.aggroIncreaseFactor);
        }

        public void _DecreaseAggro(Character newTarget)
        {
            if (aggroStats) _DecreaseAggro(newTarget, this.aggroStats.aggroDecreaseFactor);
        }

        public void _IncreaseAggroOnHit(Character newTarget, float damage, int factor, bool firstHit = false)
        {
            if (!IsValidTarget(newTarget)) return;
            this.photonView.RPC("RpcIncreaseAggroOnHit", RpcTarget.All, NetworkUtil.GetID(newTarget), damage, factor, firstHit);
        }

        public void _IncreaseAggro(Character newTarget, int factor)
        {
            if (!IsValidTarget(newTarget)) return;
            this.photonView.RPC("RpcIncreaseAggro", RpcTarget.All, NetworkUtil.GetID(newTarget), factor);
        }

        public void _DecreaseAggro(Character newTarget, int factor)
        {
            if (!IsValidTarget(newTarget)) return;
            this.photonView.RPC("RpcDecreaseAggro", RpcTarget.All, NetworkUtil.GetID(newTarget), factor);
        }

        [PunRPC]
        protected void RpcIncreaseAggroOnHit(int targetID, float damage, int factor, bool firstHit)
        {
            IncreaseAggroOnHit(targetID, damage, factor, firstHit);
        }

        [PunRPC]
        protected void RpcIncreaseAggro(int targetID, int aggro)
        {
            IncreaseAggro(targetID, (float)aggro);
        }

        [PunRPC]
        protected void RpcDecreaseAggro(int targetID, int aggro)
        {
            DecreaseAggro(targetID, aggro);
        }

        private void IncreaseAggroOnHit(int targetID, float damage, int factor, bool firstHit = false)
        {
            float aggro = factor * damage;
            if (firstHit) aggro = this.needed*100f;

            if (targetID > 0)
            {
                if (!this.aggroList.ContainsKey(targetID)) this.AddToAggroList(targetID, 0, aggro);                 

                if (!this.HasMainTarget()) this.ShowClue(MasterManager.markTarget);
            }
        }

        private void IncreaseAggro(int targetID, float aggroIncrease)
        {
            if (targetID > 0)
            {
                if (!this.aggroList.ContainsKey(targetID)) this.AddToAggroList(targetID, aggroIncrease, aggroIncrease);
                else ChangeAggroFactor(targetID, (int)aggroIncrease);

                if (!this.HasMainTarget()) this.ShowClue(MasterManager.markTarget);
            }
        }

        private void DecreaseAggro(int targetID, int aggroDecrease)
        {
            if (targetID > 0) this.ChangeAggroFactor(targetID, aggroDecrease);
        }

        public void ShowClue(AggroClue clue)
        {
            if (this.activeClue != null && clue.name == this.activeClue.name) return;

            HideClue();
            this.activeClue = Instantiate(clue, this.GetHeadPosition(), Quaternion.identity, this.transform);
            this.activeClue.name = clue.name;
        }

        public void HideClue()
        {
            if (this.activeClue != null) this.activeClue.Hide();
        }

        private void AddToAggroList(int character, float factor = 0, float startValue = 0)
        {
            float start = (float)startValue / 100f;
            float fac = (float)factor / 100f;
            this.aggroList.Add(character, new float[] { start, fac });            
        }

        public void UpdateAggro(int newTarget, float aggro)
        {
            if (newTarget > 0 && this.aggroList.ContainsKey(newTarget))
            {
                this.aggroList[newTarget][0] += aggro;

                if (this.aggroList[newTarget][0] > this.needed) this.aggroList[newTarget][0] = this.needed;
                else if (this.aggroList[newTarget][0] < 0) this.aggroList[newTarget][0] = 0;
            }
        }

        public void RemoveFromAggroList(int character)
        {
            if (this.aggroList.ContainsKey(character)) this.aggroList.Remove(character);
        }

        public void ChangeAggroFactor(int character, int factor)
        {
            if (this.aggroList.ContainsKey(character)) this.aggroList[character][1] = (float)factor/100f; //set factor of increase/decreasing aggro            
        }

        public void ClearAggro() => this.aggroList.Clear();

        public bool IsValidTarget(int ID)
        {
            if (ID <= 0) return false;

            Character target = NetworkUtil.GetCharacter(ID);

            if (target == null
                || !target.gameObject.activeInHierarchy
                || GameUtil.IsInvincibleNPC(target)  //ignore invincible NPCs
                || target.values.currentState == CharacterState.dead
                || target.values.currentState == CharacterState.respawning) return false;

            return true;
        }

        private bool IsValidTarget(Character newTarget)
        {
            return (!IsGuestPlayer(newTarget));
        }
        private bool IsGuestPlayer(Character character)
        {
            if (character != null && character.GetComponent<Player>() != null && !character.GetComponent<Player>().isLocalPlayer) return true;
            return false;
        }

        #endregion
    }
}
