using UnityEngine;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using AssetIcons;

[CreateAssetMenu(menuName = "Game/Items/Item Stats")]
public class ItemStats : ScriptableObject
{
    [BoxGroup("Attributes")]
    [SerializeField]
    private int value = 1;

    [BoxGroup("Attributes")]
    public CostType resourceType;

    [BoxGroup("Attributes")]
    [ShowIf("resourceType", CostType.none)]
    [SerializeField]
    private Ability ability;

    [BoxGroup("Attributes")]
    [ShowIf("resourceType", CostType.none)]
    [SerializeField]
    public List<StatusEffect> statusEffects = new List<StatusEffect>();

    [BoxGroup("Inventory")]
    [SerializeField]
    [HideIf("resourceType", CostType.none)]
    [HideIf("resourceType", CostType.keyItem)]
    [Tooltip("Needed if an item have to be grouped. Normal items only!")]
    public ItemGroup itemGroup;

    [BoxGroup("Inventory")]
    [Tooltip("Info is need to load names, icons and discriptions")]
    [SerializeField]
    [Required]
    public ItemInfo info;

    [BoxGroup("Inventory")]
    [SerializeField]
    [Required]
    [HideIf("resourceType", CostType.none)]
    [ShowIf("resourceType", CostType.keyItem)]
    [Tooltip("Needed to show the item in the inventory. Only for key items!")]
    public ItemSlotInfo inventoryInfo;

    [HideInInspector]
    public int amount = 1;

    [BoxGroup("Signals")]
    [SerializeField]
    private AudioClip collectSoundEffect;


    public ItemInfo getInfo()
    {
        //if (this.itemGroup != null) return this.itemGroup.info;
        return this.info;
    }

    public bool isKeyItem()
    {
        return this.resourceType == CostType.keyItem ;
    }

    public bool isID(int ID)
    {
        if (this.itemGroup != null) return this.itemGroup.isID(ID);
        else if (this.inventoryInfo != null) return this.inventoryInfo.isID(ID);
        return false;
    }

    public void Initialize(int amount)
    {
        this.amount = amount;
    }

    public string getItemGroup()
    {
        if (this.itemGroup != null) return this.itemGroup.getName();
        else return "";
    }

    public int getMaxAmount()
    {
        if (this.itemGroup != null) return this.itemGroup.maxAmount;
        return 0;
    }

    [AssetIcon]
    public Sprite getSprite()
    {
        if(this.info != null) return this.info.getSprite();
        return null;
    }

    public int getTotalAmount()
    {
        return this.value * this.amount;
    }

    public AudioClip getSoundEffect()
    {
        return this.collectSoundEffect;
    }

    public string getName()
    {
        return this.info.getName();
    }

    public string getDescription()
    {
        return this.info.getDescription();
    }    
}
