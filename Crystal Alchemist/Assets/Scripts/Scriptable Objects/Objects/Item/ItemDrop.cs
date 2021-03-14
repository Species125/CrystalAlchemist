using AssetIcons;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CrystalAlchemist
{
    [CreateAssetMenu(menuName = "Game/Items/Item Drop")]
    public class ItemDrop : NetworkScriptableObject
    {
        [BoxGroup("Required")]
        [Required]
        public ItemStats stats;

        [BoxGroup("Required")]
        [Required]
        public Collectable collectable;

        [BoxGroup("Time")]
        public bool hasSelfDestruction = true;

        [BoxGroup("Time")]
        [ShowIf("hasSelfDestruction")]
        [Tooltip("Destroy Collectable Gameobject x seconds after spawn")]
        public float duration = 60f;

        [BoxGroup("Progress")]
        public bool useProgress = false;

        [BoxGroup("Progress")]
        [ShowIf("useProgress")]
        [HideLabel]
        public ProgressValue progress;

        private int number;

        [AssetIcon]
        private Sprite GetSprite()
        {
            if (this.stats != null) return stats.getSprite();
            return null;
        }

        public ItemDrop Instantiate(int amount)
        {
            ItemDrop clone = Instantiate(this);
            clone.name = this.name;
            clone.Initialize(amount); //Set correct stats name for unique items

            return clone;
        }

        private void Initialize(int amount)
        {
            ItemStats temp = Instantiate(this.stats);
            temp.name = this.name;
            temp.Initialize(amount);
            this.stats = temp;
            this.number = Random.Range(0, 999);
        }

        public int GetNumber()
        {
            return this.number;
        }

        public bool HasItemAlready()
        {
            return (this.HasProgress() 
                    ||
                   (  (this.stats.itemType == ItemType.inventory && GameEvents.current.HasItemAlready(this.stats.inventoryItem))
                   || (this.stats.itemType == ItemType.ability && GameEvents.current.HasItemAlready(this.stats.ability))
                   || (this.stats.itemType == ItemType.outfit && GameEvents.current.HasItemAlready(this.stats.outfit))
                   )
                   );
        }

        private bool HasProgress()
        {
            if (!this.useProgress) return false;
            return GameEvents.current.HasProgress(this.progress);
        }
    }
}