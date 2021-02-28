using Photon.Pun;
using Sirenix.OdinInspector;
using System;
using System.Collections;
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
        private Player player;

        private void Start() 
        {
            if (!NetworkUtil.IsLocal(this.player)) return;
            GameEvents.current.OnMenuClosed += OnMenuClosed;
        }

        private void OnDestroy()
        {
            if (!NetworkUtil.IsLocal(this.player)) return;
            GameEvents.current.OnMenuClosed -= OnMenuClosed;
        }

        public override void Initialize()
        {
            base.Initialize();

            this.player = this.character.GetComponent<Player>();
            if (!NetworkUtil.IsLocal(this.player)) return;

            this.SetTimeValue(this.timeLeftValue);

            this.skillSet.SetSender(this.character);
            ClearCurrentAbility();
            this.buttons.ResetAbilities();
        }

        public override void Updating()
        {
            if (!this.player.isLocalPlayer) return;

            base.Updating();
            this.skillSet.Updating();
            this.buttons.Updating(this.player);

            if (this.buttons.currentAbility == null) this.player.values.isAttacking = false;
            else this.player.values.isAttacking = true;

            if (this.isPressed) ButtonHold(this.buttons.currentAbility);
        }

        public void OnHoldingCallback(InputAction.CallbackContext context)
        {
            if (!NetworkUtil.IsLocal(this.player)) return;
            if (!this.player.values.CanUseAbilities()) return;

            if (context.started)
            {
                this.isPressed = true;

                Ability ability = GetAbility(context);
                if (ability != null && (this.buttons.currentAbility == null || this.buttons.currentAbility == ability)) ButtonDown(ability);                
            }

            if (context.canceled)
            {
                this.isPressed = false;

                Ability ability = GetAbility(context);
                if (ability != null && this.buttons.currentAbility == ability) ButtonUp(ability);        
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
            if (!NetworkUtil.IsLocal(this.player)) return;
            if (!this.player.values.CanUseAbilities()) return;
            if (ability == null || !ability.enabled) return;

            if (ability.state == AbilityState.notCharged) ChargeAbility(ability); //CHANGED: ChargeAbility(ability, this.player);            
            else if (ability.isRapidFire)
            {
                if (ability.state == AbilityState.charged) UseAbilityOnTarget(ability, null); //use rapidFire when charged
                else if (ability.state == AbilityState.ready) UseAbilityOnTarget(ability, null); //use rapidFire
                else if (ability.state == AbilityState.targetRequired) ShowTargetingSystem(ability); //show TargetingSystem
                else if (ability.state == AbilityState.lockOn) UseAbilityOnTargets(ability); //use TargetingSystem rapidfire  
            }
        }

        private void ButtonDown(Ability ability)
        {         
            if (!NetworkUtil.IsLocal(this.player)) return;
            if (ability == null || !ability.enabled) return;

            if (ability.state != AbilityState.onCooldown) SetAbility(ability);

            if (ability.state == AbilityState.ready) UseAbilityOnTarget(ability, null); //use Skill
            else if (ability.state == AbilityState.targetRequired) ShowTargetingSystem(ability); //activate Targeting System
            else if (ability.state == AbilityState.lockOn)
            {
                UseAbilityOnTargets(ability);//use Skill on locked Targets and hide Targeting System 
                HideTargetingSystem(ability);
            }
        }

        private void OnMenuClosed()
        {
            Ability ability = this.buttons.currentAbility;
            if (ability == null) return; 

            HideTargetingSystem(ability);
            UnChargeAbility(ability);
            GlobalCooldownUp(ability);

            ability.SetStartParameters();
            ClearCurrentAbility();

            StartCoroutine(DelayCo());
        }

        private void ButtonUp(Ability ability)
        {
            if (!NetworkUtil.IsLocal(this.player)) return;
            if (ability == null) return;

            if (ability.enabled && ability.state == AbilityState.charged && !ability.isRapidFire) UseAbilityOnTarget(ability, null); //use Skill when charged
            if (ability.useTargetSystem && ability.isRapidFire) HideTargetingSystem(ability); //hide Targeting System when released

            UnChargeAbility(ability);
            GlobalCooldownUp(ability);            
        }

        public override void GlobalCooldown(Ability ability)
        {
            if (ability.isRapidFire || ability.deactivateButtonUp) return;
            Invoke("ClearCurrentAbility", ability.globalCooldown);          
        }

        private void GlobalCooldownUp(Ability ability)
        {
            if (ability.state == AbilityState.notCharged) ClearCurrentAbility();
            if (!ability.isRapidFire && !ability.deactivateButtonUp) return;
            
            Invoke("ClearCurrentAbility", ability.globalCooldown);
        }

        private void SetGlobalCooldown(Ability ability) //Experimental
        {
            ClearCurrentAbility();
            this.buttons.SetGlobalCooldown(ability);
        }

        private void SetAbility(Ability ability)
        {
            this.buttons.currentAbility = ability;
            //this.player.values.currentState = CharacterState.attack;
        }

        public override void ClearCurrentAbility()
        {
            this.buttons.currentAbility = null;
            //if (this.player.values.CanOpenMenu()) this.player.values.currentState = CharacterState.idle;
        }

        private IEnumerator DelayCo()
        {
            this.skillSet.EnableAbility(false);
            yield return new WaitForSeconds(this.skillSet.deactiveDelay);
            this.skillSet.EnableAbility(true);
        }

        public override void DeactivateAbility(Ability ability)
        {
            if (!this.player.isLocalPlayer || !ability.deactivateButtonUp) return;

            DeactivateSkill(ability);
            this.player.photonView.RPC("RpcDeactivateSkill", RpcTarget.Others, ability.path);
        }

        [PunRPC]
        protected void RpcDeactivateSkill(string path)
        {
            Ability ability = Resources.Load<Ability>(path);
            DeactivateSkill(ability);
        }
    }
}
