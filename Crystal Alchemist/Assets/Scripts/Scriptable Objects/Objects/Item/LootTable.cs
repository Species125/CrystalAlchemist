using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
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
            if (this.hasAlternative && this.firstLoot.item.HasItemAlready()) this.loot = this.alternativeLoot;
            else this.loot = this.firstLoot;

            ItemDrop result = this.loot.item.Instantiate(this.loot.amount);
            return result;
        }
    }

    [CreateAssetMenu(menuName = "Game/Items/Loot Table")]
    public class LootTable : ScriptableObject
    {
        public enum LootType
        {
            single,
            multi,
            all
        }

        [SerializeField]
        private List<LootTableEntry> entries = new List<LootTableEntry>();

        [BoxGroup("Options")]
        [SerializeField]
        private LootType type;

        [BoxGroup("Options")]
        [SerializeField]
        [Min(1)]
        [Tooltip("additional amount of loot")]
        [ShowIf("type", LootType.multi)]
        private int additionalLoot = 1;

        public List<ItemDrop> GetItemDrops()
        {
            int randomNumber = Random.Range(1, 100);
            List<ItemDrop> possibleRewards = new List<ItemDrop>();

            foreach (LootTableEntry entry in this.entries)
            {
                if (entry.dropRate > randomNumber || !entry.hasDropRate) possibleRewards.Add(entry.reward.GetItemDrop());
            }

            if (this.type == LootType.all) return possibleRewards;

            return GetItemDropsLimited(possibleRewards);
        }

        private List<ItemDrop> GetItemDropsLimited(List<ItemDrop> possibleRewards)
        {
            List<ItemDrop> rewards = new List<ItemDrop>();
            possibleRewards.OrderBy(x => x.rarity).ThenBy(x => x.GetNumber());

            if (possibleRewards.Count > 0)
            {
                rewards.Add(possibleRewards[0]); //Get Highest Rarity garanteed

                if (this.type == LootType.single) return rewards;

                int maxLoot = Random.Range(0, this.additionalLoot); //How many additional loot?
                int i = 0;

                while (i < maxLoot)
                {
                    int index = Random.Range(0, possibleRewards.Count);
                    ItemDrop drop = possibleRewards[index];

                    rewards.Add(drop);
                    i++;
                }
            }

            return rewards;
        }
    }
}