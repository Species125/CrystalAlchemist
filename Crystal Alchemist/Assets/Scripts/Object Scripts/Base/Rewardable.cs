using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace CrystalAlchemist
{
    public class Rewardable : Interactable
    {
        [BoxGroup("Loot")]
        [SerializeField]
        private bool useLootTable = false;

        [BoxGroup("Loot")]
        [HideIf("useLootTable")]
        [SerializeField]
        [HideLabel]
        private Reward reward;

        [BoxGroup("Loot")]
        [ShowIf("useLootTable")]
        [SerializeField]
        private LootTable lootTable;

        [HideInInspector]
        public List<ItemDrop> itemDrops = new List<ItemDrop>();

        public void SetLoot()
        {
            this.itemDrops.Clear();
            if (this.useLootTable) this.itemDrops = this.lootTable.GetItemDrops();
            else this.itemDrops.Add(this.reward.GetItemDrop());
        }
    }
}
