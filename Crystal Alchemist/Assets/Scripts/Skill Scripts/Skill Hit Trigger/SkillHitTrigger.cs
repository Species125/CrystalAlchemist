using UnityEngine;

namespace CrystalAlchemist
{
    public class SkillHitTrigger : MonoBehaviour
    {
        [HideInInspector]
        public Skill skill;

        public virtual void Awake()
        {
            if (this.skill == null) this.skill = this.GetComponentInParent<Skill>();
        }

        private void Start() => Initialize();

        private void Update() => Updating();

        public virtual void Initialize() { }

        public virtual void Updating() { }
    }
}

