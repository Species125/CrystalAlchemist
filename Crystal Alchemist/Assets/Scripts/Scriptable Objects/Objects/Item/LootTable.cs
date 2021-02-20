using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace CrystalAlchemist
{
    [System.Serializable]
    public class Loot
    {
        [HorizontalGroup(0.5f, MarginRight = 0.1f)]
        [HideLabel]
        public ItemDrop item;

        [MaxValue(99)]
        [MinValue(1)]
        [HorizontalGroup(0.1f)]
        [LabelWidth(60)]
        [ShowIf("item")]
        public int amount = 1;
    }

    [System.Serializable]
    public class LootTableEntry
    {
        [SerializeField]
        [HideLabel]
        public Reward reward;

        [HorizontalGroup(0.5f, MarginRight = 0.1f)]
        public bool hasDropRate = false;

        [ShowIf("hasDropRate")]
        [HorizontalGroup(0.1f)]
        [LabelWidth(60)]
        [MaxValue(100)]
        [MinValue(1)]
        public int dropRate = 100;
    }

    [System.Serializable]
    public class Reward
    {
        [SerializeField]
        [HideLabel]
        private Loot firstLoot;

        [SerializeField]
        [HorizontalGroup(0.5f, MarginRight = 0.1f)]
        private bool hasAlternative = false;

        [ShowIf("hasAlternative")]
        [SerializeField]
        [HideLabel]
        private Loot alternativeLoot;

        private Loot loot;

        public ItemDrop GetItemDrop()
        {
            if (this.hasAlternative && this.firstLoot.item.HasKeyItem()) this.loot = this.alternativeLoot;
            else this.loot = this.firstLoot;

            ItemDrop result = this.loot.item.Instantiate(this.loot.amount);
            return result;
        }
    }

    [CreateAssetMenu(menuName = "Game/Items/Loot Table")]
    public class LootTable : ScriptableObject
    {
        [SerializeField]
        private List<LootTableEntry> entries = new List<LootTableEntry>();

        public ItemDrop GetItemDrop()
        {
            int randomNumber = Random.Range(1, 100);
            List<Reward> possibleRewards = new List<Reward>();

            foreach (LootTableEntry entry in this.entries)
            {
                if (entry.dropRate > randomNumber || !entry.hasDropRate) possibleRewards.Add(entry.reward);
            }

            if (possibleRewards.Count > 0) return possibleRewards[Random.Range(0, possibleRewards.Count)].GetItemDrop();
            else return null;
        }
    }
}