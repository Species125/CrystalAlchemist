using AssetIcons;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace CrystalAlchemist
{

    [CreateAssetMenu(menuName = "Game/Items/Item Drop")]
    public class ItemDrop : NetworkScriptableObject
    {
        [BoxGroup("Items")]
        [SerializeField]
        [Required]
        private Sprite icon;

        [BoxGroup("Items")]
        public Collectable collectable;

        [BoxGroup("Items")]
        public ItemRarity rarity;

        [BoxGroup("Items")]
        public List<ItemStats> items = new List<ItemStats>();

        [BoxGroup("Items")]
        public LocalisationFileType type = LocalisationFileType.items;

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
        
        [BoxGroup("Audio")]
        public AudioClip collectSoundEffect;

        private int number;

        [AssetIcon]
        public Sprite GetSprite()
        {
            return this.icon;
        }

        public string GetDescription()
        {
            return FormatUtil.GetLocalisedText(this.name + "_Description", this.type);
        }

        public string GetName()
        {
            return FormatUtil.GetLocalisedText(this.name + "_Name", this.type);
        }

        public bool IsConsumable()
        {
            foreach(ItemStats stat in this.items)
            {
                if (stat.itemType != ItemType.consumable) return false;
            }

            return true;
        }

        
        [Button]
        public void SetDrop()
        {
            string dir = Path.GetDirectoryName(AssetDatabase.GetAssetPath(this));
            string parent = dir.Split('\\')[dir.Split('\\').Length-1];
            
            ItemStats stats = Resources.Load<ItemStats>("Scriptable Objects/Items/Item Stats/"+parent+"/" + this.name);
            Collectable coll = Resources.Load<Collectable>("Prefabs/Items/" + parent + "/" + this.name);

            this.items.Clear();
            this.items.Add(stats);
            this.rarity = stats.rarity;
            this.icon = stats.icon;

            this.collectable = coll;
            this.collectable.SetItem(this);
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
            List<ItemStats> temp = new List<ItemStats>();

            foreach(ItemStats origin in this.items)
            {
                ItemStats newStats = Instantiate(origin);
                newStats.name = this.name;
                newStats.Initialize(amount);
                temp.Add(newStats);
            }

            this.items = temp;            
            this.number = Random.Range(0, 999);
        }

        public int GetAmount(PlayerInventory inventory)
        {
            int highestAmount = 0;

            foreach(ItemStats stat in this.items)
            {
                int amount = inventory.GetAmount(stat.inventoryItem);
                if (amount > highestAmount) highestAmount = amount;
            }

            return highestAmount;
        }

        public int GetMaxAmount()
        {
            int highestAmount = 1;

            foreach (ItemStats stat in this.items)
            {
                if (stat.inventoryItem == null) continue;

                int amount = stat.inventoryItem.maxAmount;
                if (amount > highestAmount) highestAmount = amount;
            }

            return highestAmount;
        }

        public int GetNumber()
        {
            return this.number;
        }

        public bool HasItemAlready()
        {
            if (this.items.Count > 1 || this.items.Count < 1) return false;

            return (this.HasProgress() 
                    ||
                   (  (this.items[0].itemType == ItemType.inventory && GameEvents.current.HasItemAlready(this.items[0].inventoryItem))
                   || (this.items[0].itemType == ItemType.ability && GameEvents.current.HasItemAlready(this.items[0].ability))
                   || (this.items[0].itemType == ItemType.outfit && GameEvents.current.HasItemAlready(this.items[0].outfit))
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