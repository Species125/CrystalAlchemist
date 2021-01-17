using DG.Tweening;
using Photon.Pun;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CrystalAlchemist
{
    public class AI : NonPlayer
    {
        [BoxGroup("Debug")]
        public int targetID;

        [BoxGroup("Debug")]
        public Dictionary<int, float[]> aggroList = new Dictionary<int, float[]>();

        [BoxGroup("Debug")]
        public Character partner;

        [HideInInspector]
        public bool rangeTriggered;


        private bool isSleeping = true;

        public override void Awake()
        {
            base.Awake();
            this.targetID = 0;
        }

        #region Animation, StateMachine

        public void InitializeAddSpawn(int target, bool hasMaxDuration, float maxDuration)
        {
            this.InitializeAddSpawn(hasMaxDuration, maxDuration);
            this.targetID = target;
        }

        public Character GetTarget()
        {
            return NetworkUtil.GetCharacter(this.targetID);
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
            GameEvents.current.OnRangeTriggered += SetRangeTriggered;

            this.GetComponent<AICombat>().Initialize();
            AIComponent[] components = this.GetComponents<AIComponent>();
            for (int i = 0; i < components.Length; i++) components[i].Initialize();
        }

        public override void OnDestroy()
        {
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

            if (this.targetID > 0 && this.isSleeping)
            {
                AnimatorUtil.SetAnimatorParameter(this.animator, "WakeUp");
                this.isSleeping = false;
            }
            else if (this.targetID <= 0 && !this.isSleeping)
            {
                AnimatorUtil.SetAnimatorParameter(this.animator, "Sleep");
                this.isSleeping = true;
            }

            this.GetComponent<AICombat>().Updating();
            AIComponent[] components = this.GetComponents<AIComponent>();
            for (int i = 0; i < components.Length; i++) components[i].Updating();
        }

        public void changeState(CharacterState newState)
        {
            if (this.values.currentState != newState) this.values.currentState = newState;
        }

        public void MoveCharacters(Vector2 position, float duration)
        {
            this.myRigidbody.DOMove(position, duration);
        }

        #endregion


        public void SetMaxAggro(float aggro)
        {
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
                stream.SendNext(this.targetID);
            }
            else
            {
                this.aggroList = (Dictionary<int,float[]>)stream.ReceiveNext();
                this.targetID = (int)stream.ReceiveNext();
            }
        }

    }
}
