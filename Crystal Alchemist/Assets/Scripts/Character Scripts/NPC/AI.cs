using DG.Tweening;
using Photon.Pun;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace CrystalAlchemist
{
    public class AI : NonPlayer
    {
        [BoxGroup("Events")]
        [SerializeField]
        public UnityEvent onTarget;

        [BoxGroup("Debug")]
        [ReadOnly]
        [SerializeField]
        private int mainTargetID;

        [BoxGroup("Debug")]
        [ReadOnly]
        public Dictionary<int, float[]> aggroList = new Dictionary<int, float[]>();

        [BoxGroup("Debug")]
        [ReadOnly]
        public Character partner;

        [BoxGroup("Debug")]
        [ReadOnly]
        public bool rangeTriggered;
        
        private bool isSleeping = true;

        public override void Awake()
        {
            base.Awake();
            ClearMainTarget();
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
                this.onTarget?.Invoke();
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


        public void SetMaxAggro(float aggro)
        {
            //if (!NetworkUtil.IsMaster()) return;

            foreach (Photon.Realtime.Player p in PhotonNetwork.PlayerList)
            {
                Player player = (Player)p.TagObject;
                GameEvents.current.DoAggroIncrease(this, player, aggro);
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

    }
}
