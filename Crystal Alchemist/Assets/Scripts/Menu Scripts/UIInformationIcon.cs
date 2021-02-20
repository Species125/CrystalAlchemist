using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace CrystalAlchemist
{
    public class UIInformationIcon : MonoBehaviour
    {
        [SerializeField]
        private Image image;

        [SerializeField]
        private float duration = 3f;

        private void Awake()
        {
            this.image.DOFade(0, 0);
            GameEvents.current.OnSaveGame += ShowImage;
        }

        private void OnDestroy()
        {
            GameEvents.current.OnSaveGame -= ShowImage;
        }

        private void ShowImage(bool value)
        {
            this.image.DOFade(0.5f, 0);
            this.image.DOFade(0, this.duration);
        }
    }
}
