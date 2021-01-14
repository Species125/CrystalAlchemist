using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace CrystalAlchemist
{
    public class AI : NonPlayer
    {
        [BoxGroup("Debug")]
        public Character target;

        [BoxGroup("Debug")]
        public Dictionary<Character, float[]> aggroList = new Dictionary<Character, float[]>();

        [BoxGroup("Debug")]
        public Character partner;

        [HideInInspector]
        public bool rangeTriggered;


        private bool isSleeping = true;

        public override void Awake()
        {
            base.Awake();
            this.target = null;
        }
        #region Animation, StateMachine

        public void InitializeAddSpawn(Character target, bool hasMaxDuration, float maxDuration)
        {
            this.InitializeAddSpawn(hasMaxDuration, maxDuration);
            this.target = target;
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

            if (this.target != null && this.isSleeping)
            {
                AnimatorUtil.SetAnimatorParameter(this.animator, "WakeUp");
                this.isSleeping = false;
            }
            else if (this.target == null && !this.isSleeping)
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

    }
}
