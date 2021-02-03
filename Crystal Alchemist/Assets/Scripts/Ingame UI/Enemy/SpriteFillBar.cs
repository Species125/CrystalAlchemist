using UnityEngine;

namespace CrystalAlchemist
{
    public class SpriteFillBar : MonoBehaviour
    {
        public void fillAmount(float fillAmount)
        {
            float amount = fillAmount;
            if (amount > 1) amount = 1;
            else if (amount < 0) amount = 0;

            this.transform.localScale = new Vector3(fillAmount, 1, 1);
        }
    }
}
