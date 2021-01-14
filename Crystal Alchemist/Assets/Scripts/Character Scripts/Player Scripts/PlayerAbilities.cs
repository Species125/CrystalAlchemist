using Photon.Pun;
using Sirenix.OdinInspector;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace CrystalAlchemist
{
    public class PlayerAbilities : CharacterCombat
    {
        [SerializeField]
        [Required]
        private PlayerSkillset skillSet;

        [SerializeField]
        [Required]
        private PlayerButtons buttons;

        [SerializeField]
        [Required]
        private FloatValue timeLeftValue;

        private bool isPressed;
        private float timer;
        private Player player;
        private PhotonView photonView;

        private void Awake()
        {
            this.photonView = PhotonView.Get(this);
        }


        private void Start() => GameEvents.current.OnCancel += DisableAbilities;

        private void OnDestroy() => GameEvents.current.OnCancel -= DisableAbilities;

        public override void Initialize()
        {
            base.Initialize();

            this.player = this.character.GetComponent<Player>();
            if (!NetworkUtil.IsLocal(this.player)) return;

            this.SetTimeValue(this.timeLeftValue);

            //this.skillSet.Initialize();
            this.skillSet.SetSender(this.character);
        }

        public override void Updating()
        {
            if (!NetworkUtil.IsLocal(this.player)) return;

            base.Updating();
            this.skillSet.Updating();
            this.buttons.Updating(this.player);

            if (this.isPressed) ButtonHold(this.buttons.currentAbility);
        }

        public void SelectTargetInput(InputAction.CallbackContext ctx)
        {
            if (!NetworkUtil.IsLocal(this.player)) return;

            if (ctx.performed) this.GetTargetingSystem().SetTargetChange(ctx.ReadValue<Vector2>());
        }

        public void OnHoldingCallback(InputAction.CallbackContext context)
        {
            if (!NetworkUtil.IsLocal(this.player)) return;

            if (context.started)
            {
                this.isPressed = true;

                Ability ability = GetAbility(context);
                if (ability != null && this.buttons.currentAbility == null)
                {
                    this.buttons.currentAbility = ability;
                    ButtonDown(this.buttons.currentAbility);
                }
            }
            else if (context.canceled)
            {
                this.isPressed = false;

                Ability ability = GetAbility(context);
                if (ability != null && this.buttons.currentAbility == ability)
                {
                    ButtonUp(this.buttons.currentAbility);
                    this.buttons.currentAbility = null;
                }
            }
        }

        private Ability GetAbility(InputAction.CallbackContext context)
        {
            foreach (enumButton item in Enum.GetValues(typeof(enumButton)))
            {
                if (item.ToString().ToUpper() == context.action.name.ToString().ToUpper())
                    return this.buttons.GetAbilityFromButton(item);
            }

            return null;
        }

        private void ButtonHold(Ability ability)
        {
            if (ability == null || !ability.enabled) return;
            if (ability.state == AbilityState.notCharged)
            {
                ChargeAbility(ability, this.player);
            }
            else if (ability.isRapidFire)
            {
                if (ability.state == AbilityState.charged) UseAbilityOnTarget(ability, null); //use rapidFire when charged
                else if (ability.state == AbilityState.ready) UseAbilityOnTarget(ability, null); //use rapidFire
                else if (ability.state == AbilityState.targetRequired) ShowTargetingSystem(ability); //show TargetingSystem
                else if (ability.state == AbilityState.lockOn) UseAbilityOnTargets(ability); //use TargetingSystem rapidfire  
            }
        }

        private void ButtonUp(Ability ability)
        {
            if (ability == null) return;

            if (ability.enabled && ability.state == AbilityState.charged && !ability.isRapidFire) UseAbilityOnTarget(ability, null); //use Skill when charged
            else if (ability.state == AbilityState.lockOn && ability.isRapidFire) HideTargetingSystem(ability); //hide Targeting System when released

            UnChargeAbility(ability);
        }

        private void ButtonDown(Ability ability)
        {
            if (ability == null || !ability.enabled) return;
            if (ability.state == AbilityState.ready) UseAbilityOnTarget(ability, null); //use Skill
            else if (ability.state == AbilityState.targetRequired) ShowTargetingSystem(ability); //activate Targeting System
            else if (ability.state == AbilityState.lockOn)
            {
                UseAbilityOnTargets(ability);//use Skill on locked Targets and hide Targeting System 
                HideTargetingSystem(ability);
            }
        }

        private void DisableAbilities()
        {
            this.skillSet.EnableAbility(false);
            Invoke("EnableAbilities", this.skillSet.deactiveDelay);
        }

        private void EnableAbilities()
        {
            this.skillSet.EnableAbility(true);
        }
    }
}
