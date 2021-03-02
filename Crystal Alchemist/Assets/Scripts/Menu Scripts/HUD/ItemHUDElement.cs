using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using System.Collections;

namespace CrystalAlchemist
{
    public class ItemHUDElement : MonoBehaviour
    {
        [SerializeField]
        private Image background;

        [SerializeField]
        private Image icon;

        [SerializeField]
        private TMP_Text textField;

        [SerializeField]
        private float duration = 2f;

        [SerializeField]
        private float fadeDuration = 0.5f;

        [SerializeField]
        [Min(0)]
        private float transparency = 0.3f;

        [SerializeField]
        private Transform child;

        [SerializeField]
        private float offset = 400;

        [SerializeField]
        private float speed = 0.5f;

        public void SetElement(ItemDrop drop)
        {
            this.child.DOLocalMoveX(this.offset, 0);

            this.icon.sprite = drop.stats.getSprite();
            this.textField.text = drop.stats.getName();
            Color color = drop.stats.GetRarity();
            this.background.color = new Color(color.r, color.g, color.b, this.transparency);                     
        }

        private void Start()
        {
            StartCoroutine(FadeCo());
        }

        private IEnumerator FadeCo()
        {
            this.child.DOLocalMoveX(0, this.speed);

            yield return new WaitForSeconds(this.duration+this.speed);

            this.background.DOFade(0, this.fadeDuration);
            this.icon.DOFade(0, this.fadeDuration);
            this.textField.DOFade(0, this.fadeDuration);

            Destroy(this.gameObject, this.fadeDuration+0.1f);
        }
    }
}
