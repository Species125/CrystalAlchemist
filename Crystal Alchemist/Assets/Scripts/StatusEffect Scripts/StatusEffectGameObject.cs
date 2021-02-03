

using Sirenix.OdinInspector;
using UnityEngine;

namespace CrystalAlchemist
{
    public class StatusEffectGameObject : NetworkBehaviour
    {
        [SerializeField]
        private bool endFromAnimator = false;

        [SerializeField]
        [ShowIf("endFromAnimator")]
        private Animator anim;

        private StatusEffect activeEffect;

        public bool isActive = true;

        public void Initialize(StatusEffect effect)
        {
            this.activeEffect = effect;
        }

        public StatusEffect getEffect()
        {
            return this.activeEffect;
        }

        public void Deactivate()
        {
            if (this.anim != null && this.endFromAnimator) AnimatorUtil.SetAnimatorParameter(this.anim, "End");
            else DestroyIt();
        }

        public void DestroyIt()
        {
            Destroy(this.gameObject);
            //this.isActive = false;
        }

        public void PlaySoundEffect(AudioClip audioClip)
        {
            AudioUtil.playSoundEffect(this.gameObject, audioClip);
        }
    }
}
