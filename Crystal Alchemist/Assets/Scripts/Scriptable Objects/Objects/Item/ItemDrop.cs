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

        public void Initialize(int amount)
        {
            ItemStats temp = Instantiate(this.stats);
            temp.name = this.name;
            temp.Initialize(amount);
            this.stats = temp;
        }

        public bool HasKeyItem()
        {
            return (this.HasProgress() ||
                   (this.stats.isKeyItem() && GameEvents.current.HasKeyItem(this.name)));
        }

        private bool HasProgress()
        {
            if (!this.useProgress) return false;
            return GameEvents.current.HasProgress(this.progress);
        }
    }
}