
using TMPro;
using UnityEngine;

namespace CrystalAlchemist
{
    public class MiniGameCard : MonoBehaviour
    {
        [SerializeField]
        private Animator anim;

        [SerializeField]
        private TextMeshProUGUI number;

        public void setValue(int value)
        {
            this.number.text = value + "";
        }

        public void show()
        {
            AnimatorUtil.SetAnimatorParameter(this.anim, "Show");
        }
    }
}
