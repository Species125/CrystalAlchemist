using UnityEngine;

namespace CrystalAlchemist
{
    public class StatusEffectAnalyseModule : MonoBehaviour, StatusEffectModule
    {
        [SerializeField]
        private BoolValue isActive;

        private StatusEffect activeEffect;

        private void Start()
        {
            this.activeEffect = this.GetComponent<StatusEffectGameObject>().getEffect();
            this.transform.position = this.activeEffect.GetTarget().GetGroundPosition();
        }

        public void DoAction() => this.isActive.setValue(true);

        public void DoDestroy() => this.isActive.setValue(false);

        void FixedUpdate()
        {
            float angle = (Mathf.Atan2(this.activeEffect.GetTarget().values.direction.y, this.activeEffect.GetTarget().values.direction.x) * Mathf.Rad2Deg) + 90;
            Vector3 rotation = new Vector3(0, 0, angle);

            this.transform.rotation = Quaternion.Euler(rotation);
        }
    }
}
