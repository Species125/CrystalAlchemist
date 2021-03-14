




using Sirenix.OdinInspector;
using UnityEngine;

namespace CrystalAlchemist
{
    public class ShopItem : Rewardable
    {
        [BoxGroup("Shop-Item Attribute")]
        [SerializeField]
        private SpriteRenderer childSprite;

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
            this.SetLoot();
            this.shopPrice.Initialize(this.costs);

            if (this.itemDrops.Count <= 0) Destroy(this.gameObject);
            else this.childSprite.sprite = this.itemDrops[0].stats.getSprite();
            
        }                

        public override void DoOnSubmit()
        {
            if (this.itemDrops.Count <= 0) return;

            if (this.player.CanUseInteraction(this.costs))
            {
                this.player.ReduceResource(this.costs);
                ShowDialog(DialogTextTrigger.success, this.itemDrops[0].stats);

                foreach(ItemDrop drop in this.itemDrops) GameEvents.current.DoCollect(drop);
            }
            else
            {
                ShowDialog(DialogTextTrigger.failed);
            }
        }
    }
}
