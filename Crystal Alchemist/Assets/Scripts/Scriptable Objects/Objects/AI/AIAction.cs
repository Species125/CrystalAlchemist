using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace CrystalAlchemist
{
    [System.Serializable]
    public class AIAction
    {
        public enum AbilityOverrideType
        {
            casttime,
            castbar,
            cooldown,
            indicator,
            repeat
        }

        [System.Serializable]
        public class AbilityOverride
        {
            public AbilityOverrideType type;

            [ShowIf("type", AbilityOverrideType.castbar)]            
            public bool value;

            [HideIf("type", AbilityOverrideType.indicator)]
            [HideIf("type", AbilityOverrideType.castbar)]
            [HideIf("type", AbilityOverrideType.repeat)]            
            public float time;

            [ShowIf("type", AbilityOverrideType.repeat)]            
            public int amount;

            [ShowIf("type", AbilityOverrideType.repeat)]
            [HorizontalGroup("Temp")]
            public bool keepCast;

            [ShowIf("type", AbilityOverrideType.repeat)]
            [HorizontalGroup("Temp")]
            public float delay = 0f;
        }


        public enum AIActionType
        {
            movement,
            dialog,
            wait,
            ability,
            startPhase,
            endPhase,
            invincible,
            kill,
            cannotDie,
            animation,
            signal,
            music,
            interruptAll
        }

        private enum MusicMode
        {
            play,
            stop
        }

        private enum TargetMode
        {            
            mainTarget,
            allSequence,
            allParallel
        }

        #region Attributes

        [BoxGroup("Properties")]
        [SerializeField]
        private AIActionType type;

        [ShowIf("type", AIActionType.movement)]
        [BoxGroup("Properties")]
        [SerializeField]
        private Vector2 position;

        //Movement: Fixed, follow, path, evade

        [ShowIf("type", AIActionType.dialog)]        
        [BoxGroup("Properties")]
        [SerializeField]
        private string translationID;

        [HideIf("type", AIActionType.startPhase)]
        [HideIf("type", AIActionType.animation)]
        [HideIf("type", AIActionType.cannotDie)]
        [HideIf("type", AIActionType.ability)]
        [HideIf("type", AIActionType.kill)]
        [HideIf("type", AIActionType.interruptAll)]
        [HideIf("type", AIActionType.signal)]
        [HideIf("type", AIActionType.invincible)]
        [HideIf("type", AIActionType.music)]
        [BoxGroup("Properties")]
        [SerializeField]
        private float duration = 4f;

        [ShowIf("type", AIActionType.startPhase)]
        [BoxGroup("Properties")]
        [SerializeField]
        private AIPhase nextPhase;

        [ShowIf("type", AIActionType.animation)]
        [BoxGroup("Properties")]
        [SerializeField]
        private string animations;

        [ShowIf("type", AIActionType.animation)]
        [BoxGroup("Properties")]
        [SerializeField]
        private bool isTrigger = true;

        [ShowIf("type", AIActionType.ability)]
        [BoxGroup("Properties")]
        [SerializeField]
        private Ability ability;

        [ShowIf("type", AIActionType.ability)]
        [BoxGroup("Properties")]
        [SerializeField]
        private TargetMode targetMode;

        [ShowIf("type", AIActionType.ability)]
        [BoxGroup("Properties")]
        [ShowIf("targetMode", TargetMode.allParallel)]
        [Tooltip("If Ability should fired with a delay between targets")]
        [SerializeField]
        private float multiDelay = 0f; //parellel only

        [ShowIf("type", AIActionType.ability)]
        [ShowIf("targetMode", TargetMode.allSequence)]
        [BoxGroup("Properties")]
        [Tooltip("If Ability should be casted between targets")]
        [SerializeField]
        private bool multiCast; //sequence only

        [BoxGroup("Properties")]
        [ShowIf("type", AIActionType.ability)]
        [SerializeField]
        private List<AbilityOverride> overrides = new List<AbilityOverride>();

        [HideIf("type", AIActionType.ability)]
        [HideIf("type", AIActionType.movement)]
        [HideIf("type", AIActionType.startPhase)]
        [HideIf("type", AIActionType.endPhase)]
        [HideIf("type", AIActionType.kill)]
        [HideIf("type", AIActionType.interruptAll)]
        [HideIf("type", AIActionType.dialog)]
        [HideIf("type", AIActionType.wait)]
        [HideIf("type", AIActionType.signal)]
        [HideIf("type", AIActionType.music)]
        [HideIf("isTrigger")]
        [BoxGroup("Properties")]
        [SerializeField]
        private bool value;

        [ShowIf("type", AIActionType.signal)]
        [BoxGroup("Properties")]
        [SerializeField]
        private SimpleSignal signal;

        [HideIf("type", AIActionType.wait)]
        [HideIf("type", AIActionType.kill)]
        [HideIf("type", AIActionType.interruptAll)]
        [BoxGroup("Properties")]
        [SerializeField]
        private float wait = 0f;

        [ShowIf("type", AIActionType.music)]
        [BoxGroup("Properties")]
        [SerializeField]
        private MusicMode mode = MusicMode.play;

        [ShowIf("type", AIActionType.music)]
        [HideIf("mode", MusicMode.stop)]
        [BoxGroup("Properties")]
        [SerializeField]
        private float fadeIn;

        [ShowIf("type", AIActionType.music)]
        [HideIf("mode", MusicMode.play)]
        [BoxGroup("Properties")]
        [SerializeField]
        private float fadeOut;

        [ShowIf("type", AIActionType.music)]
        [HideIf("mode", MusicMode.stop)]
        [BoxGroup("Properties")]
        [SerializeField]
        private MusicTheme music;

        #endregion

        private float elapsed = 0;
        private bool isActive = true;
        private Ability activeAbility;
        private int counter = 0;
        private int targetIndex = 0;
        private int _amount = 1;
        private bool _keepCast = false;
        private float _delay = 0;

        //TODO: Add status effect

        #region Main Functions

        public AIActionType GetActionType()
        {
            return this.type;
        }

        public AIAction(float duration, AI npc)
        {
            this.type = AIActionType.wait;
            this.duration = duration;
            Initialize(npc);
        }

        public void Initialize(AI npc)
        {
            this.isActive = true;

            switch (this.type)
            {
                case AIActionType.ability: StartSkill(npc); break;
                case AIActionType.kill: StartKill(npc); break;
                case AIActionType.animation: StartAnimation(npc); break;
                case AIActionType.cannotDie: StartCannotDie(npc); break;
                case AIActionType.invincible: StartInvinicible(npc); break;
                case AIActionType.wait: StartWait(); break;
                case AIActionType.dialog: StartDialog(npc); break;
                case AIActionType.startPhase: StartPhase(npc); break;
                case AIActionType.endPhase: EndPhase(npc); break;
                case AIActionType.signal: StartSignal(); break;
                case AIActionType.music: StartMusic(); break;
                case AIActionType.movement: StartMovement(npc); break;
                case AIActionType.interruptAll: StartInterrupt(); break;
            }
        }

        public void Updating(AI npc)
        {
            switch (this.type)
            {
                case AIActionType.ability: UpdateSkill(npc); break;
                case AIActionType.wait: UpdateWait(); break;
                case AIActionType.dialog: UpdateDialog(); break;
            }
        }

        public void Disable(AI npc)
        {
            switch (this.type)
            {
                case AIActionType.ability: DisableSkill(npc); break;
                case AIActionType.dialog: DisableDialog(); break;
            }
        }

        public bool isDialog()
        {
            return (this.type == AIActionType.dialog);
        }

        public bool getActive()
        {
            return this.isActive;
        }

        public float GetWait()
        {
            if (this.type == AIActionType.wait) return 0;
            return this.wait;
        }

        #endregion


        #region Ability

        private void StartSkill(AI npc)
        {
            this.targetIndex = 0;
            this.counter = 0;
            this._amount = 1;

            this.activeAbility = Object.Instantiate(this.ability);

            foreach(AbilityOverride _override in this.overrides)
            {
                if (_override.type == AbilityOverrideType.castbar)
                {
                    this.activeAbility.showCastbar = _override.value;
                }
                else if (_override.type == AbilityOverrideType.casttime)
                {
                    this.activeAbility.castTime = _override.time;
                    if (this.activeAbility.castTime > 0) this.activeAbility.hasCastTime = true;
                    else this.activeAbility.hasCastTime = false;
                }
                else if (_override.type == AbilityOverrideType.cooldown)
                {
                    this.activeAbility.cooldown = _override.time;
                }
                else if (_override.type == AbilityOverrideType.indicator)
                {
                    this.activeAbility.useIndicator = Ability.IndicatorType.None;
                }
                else if (_override.type == AbilityOverrideType.repeat)
                {
                    this._amount = _override.amount;
                    this._keepCast = _override.keepCast;
                    this._delay = _override.delay;
                }
            }

            this.activeAbility = AbilityUtil.InstantiateAbility(this.activeAbility, npc);            
        }

        private void UpdateSkill(AI npc)
        {
            this.activeAbility.Updating();

            if (npc.values.isCharacterStunned()) this.activeAbility.ResetCharge();

            if (this.elapsed > 0) this.elapsed -= Time.deltaTime;
            else
            {
                if (this.activeAbility.state == AbilityState.notCharged) Charge(npc);
                else if (this.activeAbility.state == AbilityState.targetRequired) CheckTargets(npc);
                else if (this.activeAbility.state == AbilityState.charged
                         || this.activeAbility.state == AbilityState.ready) UseSkill(npc);
            }

            if (this.counter >= this._amount)
            {
                if (this.targetMode != TargetMode.allSequence) DisableSkill(npc);
                else
                {
                    if(this.multiCast) npc.GetComponent<AICombat>().UnChargeAbility(this.activeAbility); //reset Charge
                    this.counter = 0;
                    this.targetIndex++;
                    if (this.targetIndex >= npc.GetTargets().Count) DisableSkill(npc);
                }
            }
        }

        private void Charge(AI npc)
        {
            AICombat combat = npc.GetComponent<AICombat>();
            if (combat == null) return;

            if (this.targetMode == TargetMode.mainTarget) combat.ChargeAbility(this.activeAbility, npc.GetTarget());
            else if (this.targetMode == TargetMode.allParallel) combat.ChargeAbility(this.activeAbility, npc.GetTargets());
            else combat.ChargeAbility(this.activeAbility, npc.GetTarget(this.targetIndex));

            if (this.activeAbility.IsTargetRequired())
                npc.GetComponent<AICombat>().ShowTargetingSystem(this.activeAbility);        //Show Targeting System when needed
        }

        private void CheckTargets(AI npc)
        {
            if (!this.activeAbility.IsTargetRequired() && npc.HasMainTarget())
                this.activeAbility.state = AbilityState.ready; //SingleTarget
            else this.activeAbility.state = AbilityState.ready; //Target from TargetingSystem                
        }

        private void UseSkill(AI npc)
        {
            AICombat combat = npc.GetComponent<AICombat>();
            if (combat == null) return;

            combat.HideCastBar();

            if (this.activeAbility.IsTargetRequired())
            {
                combat.UseAbilityOnTargets(this.activeAbility);
            }
            else
            {
                if (this.targetMode == TargetMode.mainTarget) combat.UseAbilityOnTarget(this.activeAbility, npc.GetTarget());
                else if (this.targetMode == TargetMode.allParallel) combat.UseAbilityOnTargets(this.activeAbility, npc.GetTargets(), this.multiDelay);
                else combat.UseAbilityOnTarget(this.activeAbility, npc.GetTarget(this.targetIndex));      

                combat.HideTargetingSystem(this.activeAbility);
            }

            this.elapsed = this._delay;
            this.counter++;

            if (!this._keepCast)
            {
                combat.HideTargetingSystem(this.activeAbility);
                combat.UnChargeAbility(this.activeAbility); //reset Charge
            }
        }        

        private void DisableSkill(AI npc)
        {
            npc.GetComponent<AICombat>().HideTargetingSystem(this.activeAbility);
            npc.GetComponent<AICombat>().UnChargeAbility(this.activeAbility); //reset Charge
            Deactivate();
        }

        #endregion
        

        #region Wait

        private void StartWait()
        {
            this.elapsed = this.duration;
        }

        private void UpdateWait()
        {
            if (this.elapsed > 0) this.elapsed -= Time.deltaTime;
            else Deactivate();
        }

        #endregion


        #region Kill

        private void StartKill(AI npc)
        {
            npc.KillIt();
            Deactivate();
        }

        #endregion


        #region Animation

        private void StartAnimation(AI npc)
        {
            if (this.isTrigger) npc.PlayAnimation(this.animations);
            else npc.PlayAnimation(this.animations, this.value);
            Deactivate();
        }

        #endregion


        #region Invincible

        private void StartInvinicible(AI npc)
        {
            npc.SetInvincible(this.value);
            Deactivate();
        }

        #endregion


        #region CannotDie

        private void StartCannotDie(AI npc)
        {
            npc.setCannotDie(this.value);
            Deactivate();
        }

        #endregion


        #region Dialog

        private void StartDialog(AI npc)
        {
            string text = FormatUtil.GetLocalisedText(this.translationID, LocalisationFileType.dialogs);
            npc.ShowMiniDialog(text, this.duration);
            this.elapsed = this.duration;
        }

        private void UpdateDialog()
        {
            if (this.elapsed > 0) this.elapsed -= Time.deltaTime;
            else Deactivate();
        }

        private void DisableDialog()
        {
            this.elapsed = 0;
        }

        #endregion


        #region PhaseTransition

        private void StartPhase(AI npc)
        {
            npc.GetComponent<AICombat>().StartPhase(this.nextPhase);
        }

        private void EndPhase(AI npc)
        {
            npc.GetComponent<AICombat>().EndPhase();
        }

        #endregion


        #region Signal

        private void StartSignal()
        {
            if (this.signal != null) this.signal.Raise();
            Deactivate();
        }

        #endregion


        #region Music

        private void StartMusic()
        {
            MusicEvents.current.StopMusic(this.fadeOut);
            if (this.mode == MusicMode.play) MusicEvents.current.PlayMusic(this.music, this.fadeIn);
            Deactivate();
        }

        #endregion

        private void Deactivate() => this.isActive = false;

        #region Movement

        private void StartMovement(AI npc) => npc.MoveCharacters(this.position, this.duration);

        #endregion


        #region Interrupt

        private void StartInterrupt()
        {
            GameEvents.current.DoInterrupt();
            Deactivate();
        }

        #endregion
    }
}