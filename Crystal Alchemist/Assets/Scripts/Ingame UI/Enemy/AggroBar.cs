using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CrystalAlchemist
{
    public class AggroBar : MonoBehaviour
    {
        private AI enemy;

        [SerializeField]
        private Image charging;
        [SerializeField]
        private TextMeshProUGUI attacking;
        [SerializeField]
        private TextMeshProUGUI zielName;

        public void setEnemy(AI enemy)
        {
            this.enemy = enemy;
        }

        private void LateUpdate()
        {
            if (enemy != null)
            {
                float percent = 0f;
                string ziel = "";
                //enemy.aggroGameObject.getHighestAggro(out percent, out ziel);

                if (percent <= 0f) this.zielName.text = "";

                else this.zielName.text = "> " + ziel;
                this.charging.fillAmount = percent;

                if (percent >= 1f)
                {
                    this.charging.color = new Color(255, 0, 0); //red
                    this.attacking.gameObject.SetActive(true);
                }
                else
                {
                    this.charging.color = new Color(255, 255, 255); //white
                    this.attacking.gameObject.SetActive(false);
                }
            }
        }
    }
}
