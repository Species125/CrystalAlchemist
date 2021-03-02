




using Sirenix.OdinInspector;
using UnityEngine;

namespace CrystalAlchemist
{
    public class ShopItem : Rewardable
    {
        [BoxGroup("Shop-Item Attribute")]
        [SerializeField]
        private SpriteRenderer childSprite;

        [BoxGroup("Loot")]
        [SerializeField]
        [HideLabel]
        private Reward reward;

        [SerializeField]
        [BoxGroup("Mandatory")]
        [Required]
        private ShopPrice shopPrice;

        [BoxGroup("Easy Access")]
        [SerializeField]
        private Animator anim;

        private new void Start()
        {
            base.Start();
            this.setLoot();
            this.shopPrice.Initialize(this.costs);

            this.childSprite.sprite = this.itemDrop.stats.getSprite();
            if (this.itemDrop == null) Destroy(this.gameObject);
        }

        private void setLoot()
        {
            this.itemDrop = this.reward.GetItemDrop();
        }

        public override void DoOnSubmit()
        {
            if (this.player.CanUseInteraction(this.costs))
            {
                this.player.ReduceResource(this.costs);

                ShowDialog(DialogTextTrigger.success, this.itemDrop.stats);
                if (this.itemDrop != null) GameEvents.current.DoCollect(this.itemDrop);
            }
            else
            {
                ShowDialog(DialogTextTrigger.failed);
            }
        }
    }
}
