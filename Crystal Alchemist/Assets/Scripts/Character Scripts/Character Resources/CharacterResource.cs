using Sirenix.OdinInspector;

namespace CrystalAlchemist
{
    public enum CostType
    {
        none,
        life,
        mana,
        item,
        keyItem,
        statusEffect,
        ability,
        outfit
    }

    [System.Serializable]
    public class BaseResource
    {
        public CostType resourceType = CostType.none;

        [ShowIf("resourceType", CostType.item)]
        public ItemGroup item;

        [ShowIf("resourceType", CostType.keyItem)]
        public ItemDrop keyItem;
    }

    [System.Serializable]
    public class CharacterResource : BaseResource
    {
        [HideIf("resourceType", CostType.none)]
        [HideIf("resourceType", CostType.keyItem)]
        public float amount = 1;
    }

    [System.Serializable]
    public class Costs : BaseResource
    {
        [HideIf("resourceType", CostType.none)]
        [HideIf("resourceType", CostType.keyItem)]
        [MinValue(0)]
        public float amount = 1;
    }
}