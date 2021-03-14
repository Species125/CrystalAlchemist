using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace CrystalAlchemist
{
    public enum CostType : byte
    {
        none = 0,
        life,
        mana,
        item,
        keyItem,
        statusEffect
    }

    [System.Serializable]
    public class BaseResource
    {
        public CostType resourceType = CostType.none;

        [ShowIf("resourceType", CostType.item)]
        public InventoryItem item;

        [ShowIf("resourceType", CostType.keyItem)]
        public ItemDrop keyItem;

        [ShowIf("resourceType", CostType.statusEffect)]
        public StatusEffect statusEffect;
    }

    [System.Serializable]
    public class CharacterResource : BaseResource
    {
        [HideIf("resourceType", CostType.none)]
        [HideIf("resourceType", CostType.keyItem)]
        public float amount = 1;

        public CharacterResource(CostType costType, float amount)
        {
            this.resourceType = costType;
            this.amount = amount;
        }

        public CharacterResource (BaseResource resource, float amount)
        {
            this.resourceType = resource.resourceType;
            this.amount = amount;
            this.item = resource.item;
            this.keyItem = resource.keyItem;
            this.statusEffect = resource.statusEffect;
        }

        public CharacterResource(string info)
        {
            string[] array = info.Split(':');
            string type = array[0];
            float amount = (float)Convert.ToDouble(array[1]);
            string path = array[2];

            Enum.TryParse(type, out CostType costType);

            this.resourceType = costType;
            this.amount = amount;

            if (this.resourceType == CostType.item) this.item = Resources.Load<InventoryItem>(path);
            else if (this.resourceType == CostType.keyItem) this.keyItem = Resources.Load<ItemDrop>(path);
            else if (this.resourceType == CostType.statusEffect) this.statusEffect = Resources.Load<StatusEffect>(path);
        }

        public string GetAsString()
        {
            string path = "";

            if (this.resourceType == CostType.item) path = this.item.path;
            if (this.resourceType == CostType.keyItem) path = this.keyItem.path;
            if (this.resourceType == CostType.statusEffect) path = this.statusEffect.path;

            return resourceType.ToString() + ":" + amount + ":" + path;
        }
    }

    [System.Serializable]
    public class Costs : BaseResource
    {
        [HideIf("resourceType", CostType.none)]
        [HideIf("resourceType", CostType.keyItem)]
        [MinValue(0)]
        public float amount = 1;

        public CharacterResource Convert()
        {
            return new CharacterResource(this, -amount);
        }
    }
}