using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CrystalAlchemist
{
    public class TutorialPage : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI textfield;

        [SerializeField]
        private Image firstImage;

        [SerializeField]
        private Image secondsImage;

        public void Initialize(string text, Sprite firstImage, Sprite secondImage)
        {
            this.textfield.text = text;
            this.firstImage.sprite = firstImage;
            this.secondsImage.sprite = secondImage;

            if (this.secondsImage.sprite == null) this.secondsImage.gameObject.SetActive(false);
        }
    }
}
