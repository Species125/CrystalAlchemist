using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace CrystalAlchemist
{
    [CreateAssetMenu(menuName = "Game/Player/Player Skillset")]
    public class PlayerSkillset : ScriptableObject
    {
        [SerializeField]
        private List<Ability> abilities = new List<Ability>();

        public float deactiveDelay = 0.3f;

        public void Clear() => this.abilities.Clear();       

        public bool Exists(Ability ability)
        {
            foreach (Ability elem in this.abilities)
                if (elem != null && ability.name == elem.name) return true;
            return false;
        }

        public void SetSender(Character sender)
        {
            this.abilities.RemoveAll(item => item == null);
            foreach (Ability ability in this.abilities) ability.SetSender(sender);
        }

        public void Updating()
        {
            this.abilities.RemoveAll(item => item == null);
            foreach (Ability ability in this.abilities) ability.Updating();
        }

        public Ability getAbilityByName(string name)
        {
            foreach (Ability ability in this.abilities)
            {
                if (name == ability.name) return ability;
            }

            return null;
        }

        public Ability getSkillByID(int ID, SkillType category)
        {
            foreach (Ability ability in this.abilities)
            {
                SkillBookInfo info = ability.info;

                if (info != null
                    && category == info.category
                    && ID == info.orderIndex) return ability;
            }

            return null;
        }

        [Button]
        public void AddAbility(Ability ability, Character character = null)
        {
            if (ability == null) return;

            foreach(Ability existingAbility in this.abilities)
            {
                if (existingAbility.name == ability.name) return;
            }

            Ability newAbility = AbilityUtil.InstantiateAbility(ability, character);
            this.abilities.Add(newAbility);
            GameEvents.current.DoSaveGame(false);
        }

        public void EnableAbility(bool value)
        {
            foreach (Ability ability in this.abilities) ability.active = value;
        }

        public List<string> GetSkillSet()
        {
            List<string> skillset = new List<string>();
            foreach (Ability ability in this.abilities)
            {
                skillset.Add(ability.name);
            }
            return skillset;
        }
    }
}
