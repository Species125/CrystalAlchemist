using UnityEngine;

namespace CrystalAlchemist
{
    public class CastingAnimation : MonoBehaviour
    {
        [SerializeField]
        private Animator anim;

        [SerializeField]
        private bool matchProgress;

        private float time;

        public void Initialize(float time)
        {
            this.time = time;
            if (matchProgress) this.anim.speed = 1 / time;
        }

        public void DestroyIt()
        {
            this.anim.speed = 1f;
            if (this.anim != null) AnimatorUtil.SetAnimatorParameter(this.anim, "Destroy");
            else Destroy();
        }

        private void Destroy()
        {
            Destroy(this.gameObject);
        }
    }
}
