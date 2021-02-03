using UnityEngine;

namespace CrystalAlchemist
{
    public class SkillModifier : MonoBehaviour
    {
        [HideInInspector]
        public Skill skill;

        private void Awake()
        {
            this.skill = this.GetComponent<Skill>();
        }
    }
}
