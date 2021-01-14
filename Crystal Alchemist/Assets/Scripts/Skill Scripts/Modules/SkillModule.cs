using UnityEngine;

namespace CrystalAlchemist
{
    [RequireComponent(typeof(Skill))]
    public class SkillModule : MonoBehaviour
    {
        [HideInInspector]
        public Skill skill;

        private void Awake()
        {
            this.skill = this.GetComponent<Skill>();
        }

        public virtual void Initialize() { }

        public virtual void Updating() { }
    }
}
