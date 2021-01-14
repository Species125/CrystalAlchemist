
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CrystalAlchemist
{
    public class MiniGameTimeBar : MonoBehaviour
    {
        [BoxGroup("Easy Access")]
        [SerializeField]
        [Required]
        private TextMeshProUGUI timeField;

        [BoxGroup("Easy Access")]
        [SerializeField]
        [Required]
        private Image timeImage;

        [BoxGroup("Easy Access")]
        [SerializeField]
        [Required]
        private FloatValue time;

        [BoxGroup("Easy Access")]
        [SerializeField]
        [Required]
        private FloatValue maxTime;


        private void FixedUpdate()
        {
            this.timeField.text = (int)(time.GetValue() + 1) + "s";
            this.timeImage.fillAmount = time.GetValue() / this.maxTime.GetValue();
        }
    }
}
