using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CrystalAlchemist
{
    [RequireComponent(typeof(Character))]
    public class CharacterCombat : MonoBehaviour
    {
        private CastBar activeCastBar;
        private TargetingSystem targetingSystem;

        [HideInInspector] public Character character;

        public virtual void Initialize()
        {
            this.character = this.GetComponent<Character>();
            this.targetingSystem = Instantiate(MasterManager.targetingSystem, this.character.transform.position,
                Quaternion.identity, this.character.transform);
            this.targetingSystem.Initialize(this.character);
            this.targetingSystem.name = MasterManager.targetingSystem.name;
            this.targetingSystem.gameObject.SetActive(false);
        }

        public virtual void Updating()
        {
        }

        public TargetingSystem GetTargetingSystem()
        {
            return this.targetingSystem;
        }

        public void SetTimeValue(FloatValue timeValue)
        {
            if (this.targetingSystem != null) this.targetingSystem.SetTimeValue(timeValue);
        }

        public void ChargeAbility(Ability ability)
        {
            ability.Charge(); //charge Skill when not full        
            ShowCastBar(ability); //Show Castbar
            ability.ShowCastingAnimation(); //Show Animation and stuff
            setSpeedDuringCasting(ability); //Set Speed during casting
        }

        public void ChargeAbility(Ability ability, Character target)
        {
            ChargeAbility(ability);
            ability.ShowCastingIndicator(target);
        }

        public void ChargeAbility(Ability ability, List<Character> targets)
        {
            ChargeAbility(ability);
            ability.ShowCastingIndicator(targets);
        }

        public void UnChargeAbility(Ability ability)
        {
            ability.ResetCharge(); //reset charge when not full             
            HideCastBar(); //Hide Castbar
            ability.HideCastingAnimation(); //Hide Animation and stuff
            DeactivateAbility(ability); //deactivate Skill when button up, Player only            
            resetSpeedAfterCasting(); //set Speed to normal
            ability.HideIndicator();
        }

        private void setSpeedDuringCasting(Ability ability)
        {
            SkillSenderModule senderModule = ability.skill.GetComponent<SkillSenderModule>();
            if (senderModule != null)
                character.UpdateSpeedPercent(senderModule.speedDuringCasting, senderModule.affectAnimation);
        }

        private void resetSpeedAfterCasting()
        {
            character.UpdateSpeedPercent(0); //Set Speed to normal
        }

        public virtual void ClearCurrentAbility()
        {

        }

        public virtual void GlobalCooldown(Ability ability)
        {

        }

        public virtual void DeactivateAbility(Ability ability)
        {
            
        }

        public void DeactivateSkill(Ability ability)
        {
            int deactivatedSkills = DeactivateSkills(ability);
            if (deactivatedSkills > 0) ability.ResetCoolDown(); //prevent Cooldown when not used skill
        }

        private int DeactivateSkills(Ability ability)
        {
            int counter = 0;
            this.character.values.activeSkills.RemoveAll(item => item == null);

            for (int i = 0; i < this.character.values.activeSkills.Count; i++)
            {
                if (this.character.values.activeSkills[i].name == ability.skill.name)
                {
                    this.character.values.activeSkills[i].DeactivateIt();
                    counter++;
                }
            }

            return counter;
        }


        public void ShowCastBar(Ability ability)
        {
            if (this.activeCastBar == null && ability.showCastbar && ability.hasCastTime)
            {
                this.activeCastBar =
                    Instantiate(MasterManager.castBar, character.GetHeadPosition(), Quaternion.identity);
                this.activeCastBar.setCastBar(character, ability);
            }
        }

        public void HideCastBar()
        {
            if (this.activeCastBar != null) this.activeCastBar.destroyIt();
        }


        public virtual void ShowTargetingSystem(Ability ability)
        {
            if (this.targetingSystem != null)
            {
                if (!this.targetingSystem.gameObject.activeInHierarchy)
                {
                    this.targetingSystem.setParameters(ability);
                    this.targetingSystem.gameObject.SetActive(true);
                }
                else if(this.targetingSystem.gameObject.activeInHierarchy && ability.isRapidFire)
                {
                    ability.SetLockOnState();
                }
            }
        }

        public void HideTargetingSystem(Ability ability)
        {
            if (this.targetingSystem != null && this.targetingSystem.gameObject.activeInHierarchy)
            {
                this.targetingSystem.Deactivate();
            }
        }

        public float GetTargetingDelay()
        {
            if (this.targetingSystem != null) return this.targetingSystem.getDelay();
            return 0f;
        }

        public virtual List<Character> GetTargetsFromTargeting()
        {
            if (this.targetingSystem != null) return this.targetingSystem.getTargets();
            return null;
        }

        #region useAbility

        public virtual void UseAbilityOnTarget(Ability ability, Character target)
        {
            if (ability.HasEnoughResourceAndAmount())
            {
                GlobalCooldown(ability);
                NetworkEvents.current.InstantiateSkill(ability, this.character, target);

                if (!ability.deactivateButtonUp && !ability.remoteActivation) ability.ResetCoolDown();
            }
            else ClearCurrentAbility();
        }

        public virtual void UseAbilityOnTargets(Ability ability)
        {
            List<Character> targets = new List<Character>();
            targets.AddRange(this.GetTargetsFromTargeting());

            UseAbilityOnTargets(ability, targets, this.GetTargetingDelay());
        }

        public virtual void UseAbilityOnTargets(Ability ability, List<Character> targets, float delay)
        {
            if (targets.Count > 0) ability.ResetCoolDown();

            if (ability.HasEnoughResourceAndAmount())
            {
                GlobalCooldown(ability);
                StartCoroutine(UseSkill(ability, targets, delay));
            }
            else ClearCurrentAbility();
        }

        public IEnumerator UseSkill(Ability ability, List<Character> targets, float delay)
        {
            float damageReduce = targets.Count;

            foreach (Character target in targets)
            {
                if (target.values.currentState != CharacterState.dead
                    && target.values.currentState != CharacterState.respawning)
                {
                    NetworkEvents.current.InstantiateAoESkill(ability, this.character, target, damageReduce);
                    yield return new WaitForSeconds(delay);
                }
            }
        }

        #endregion
    }
}