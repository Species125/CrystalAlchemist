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

        public void Clear()
        {
            Initialize();
        }

        [Button]
        public void Initialize()
        {
            this.abilities.Clear();
            foreach (Ability ability in MasterManager.abilities) AddAbility(ability);
        }

        public void TestInitialize(Character sender)
        {
            this.abilities.RemoveAll(item => item == null);

            if (this.abilities.Count <= 0)
            {
                Initialize();
                SetSender(sender);
            }
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
        private void AddAbility(Ability ability)
        {
            Ability newAbility = AbilityUtil.InstantiateAbility(ability);
            this.abilities.Add(newAbility);
        }

        public void EnableAbility(bool value)
        {
            foreach (Ability ability in this.abilities) ability.active = value;
        }


    }
}
