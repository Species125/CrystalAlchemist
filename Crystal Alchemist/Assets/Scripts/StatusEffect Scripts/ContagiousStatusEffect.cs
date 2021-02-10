using Sirenix.OdinInspector;
using UnityEngine;

namespace CrystalAlchemist
{
    public class ContagiousStatusEffect : MonoBehaviour
    {
        [BoxGroup("Statuseffekt Pflichtfelder")]
        [Required]
        [SerializeField]
        private Collider2D effectCollider;

        private StatusEffect activeEffect;

        private void Start()
        {
            this.activeEffect = this.GetComponent<StatusEffectGameObject>().getEffect();

            CollisionUtil.AddColliderCopy(this.activeEffect.GetTarget(), this.gameObject);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!collision.isTrigger)
            {
                Character character = collision.GetComponent<Character>();
                if (character != null) StatusEffectUtil.AddStatusEffect(this.activeEffect, character);
            }
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            if (!collision.isTrigger)
            {
                Character character = collision.GetComponent<Character>();
                if (character != null) StatusEffectUtil.AddStatusEffect(this.activeEffect, character);
            }
        }
    }
}
