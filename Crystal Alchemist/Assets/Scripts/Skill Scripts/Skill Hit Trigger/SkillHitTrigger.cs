using Sirenix.OdinInspector;
using UnityEngine;

namespace CrystalAlchemist
{
    public class SkillHitTrigger : MonoBehaviour
    {
        [InfoBox("Set this manually if Skill is not in the same gameobject", InfoMessageType.Warning)]
        public Skill skill;

        private void Awake()
        {
            if (this.skill == null) this.skill = this.GetComponent<Skill>();
        }

        private void Start() => Initialize();

        private void Update() => Updating();

        public virtual void Initialize() { }

        public virtual void Updating() { }
    }
}

