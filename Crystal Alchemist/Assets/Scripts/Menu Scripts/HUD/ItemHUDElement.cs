using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

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

        private string dropName;
        private int amount = 0;
        private string itemName;
        private float elapsed;
        private bool isFading;

        public void SetElement(ItemDrop drop, int amount)
        {
            this.child.DOLocalMoveX(this.offset, 0);

            this.dropName = drop.name;
            this.itemName = drop.stats.getName();
            this.icon.sprite = drop.stats.getSprite();

            UpdateElement(amount);

            Color color = GameUtil.GetRarity(drop.stats.rarity);
            this.background.color = new Color(color.r, color.g, color.b, this.transparency);                     
        }

        public bool HasDrop(ItemDrop drop)
        {
            return this.dropName == drop.name;            
        }

        public void UpdateElement(int amount)
        {
            this.amount+=amount;
            UpdateAmount();            
        }

        private void UpdateAmount()
        {
            DOTween.Kill(this.background);
            DOTween.Kill(this.icon);
            DOTween.Kill(this.textField);

            this.background.DOFade(this.transparency, 0);
            this.icon.DOFade(1, 0);
            this.textField.DOFade(1, 0);

            this.isFading = false;
            this.elapsed = this.duration + this.fadeDuration;

            this.textField.text = this.itemName;
            if (this.amount > 1) this.textField.text = this.itemName + " x" + this.amount;
        }

        private void Start()
        {
            this.child.DOLocalMoveX(0, this.speed);
            this.elapsed = this.duration + this.fadeDuration + this.speed;
        }

        private void FixedUpdate()
        {
            this.elapsed -= Time.fixedDeltaTime;

            if(elapsed <= this.fadeDuration && !this.isFading)
            {
                this.background.DOFade(0, this.fadeDuration);
                this.icon.DOFade(0, this.fadeDuration);
                this.textField.DOFade(0, this.fadeDuration);
                this.isFading = true;
            }
            else if (elapsed < 0) Destroy(this.gameObject);
        }
    }
}
