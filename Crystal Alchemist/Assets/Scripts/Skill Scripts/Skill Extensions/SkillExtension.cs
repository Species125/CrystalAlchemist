using UnityEngine;

namespace CrystalAlchemist
{
    public class SkillExtension : MonoBehaviour
    {
        [HideInInspector]
        public Skill skill;

        private void Awake()
        {
            this.skill = this.GetComponent<Skill>();
        }

        public virtual void Initialize()
        {

        }

        public virtual void Updating()
        {

        }
    }
}
