using UnityEngine;

namespace CrystalAlchemist
{
    public class SkillStartEffectModule : SkillModule
    {
        [SerializeField]
        private GameObject effect;

        public override void Initialize()
        {
            base.Initialize();
            Instantiate(this.effect, this.transform.position, Quaternion.identity);
        }
    }
}
