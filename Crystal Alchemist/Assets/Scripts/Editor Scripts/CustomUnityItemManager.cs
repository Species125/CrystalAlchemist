#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System;

public class CustomUnityItemManager : EditorWindow
{
    string[] options;
    string itemName = "";
    int index = 0;
    int itemValue = 1;
    int inventorySlot;
    SimpleSignal signal;
    Sprite itemSprite;
    CostType type;

    bool createNewGroup;
    ItemGroup group;
    AudioClip soundEffect;
    bool canConsume = true;
    bool updateUI = false;
    int maxValue = 9999;

    string itemGroupName = "";
    Sprite itemGroupSprite;
    ShopPriceUI itemShopPrice;

    bool isItem = false;


    [MenuItem("Alchemist Menu/Wizard/Add New Item")]
    public static void ItemWizard()
    {
        GetWindow(typeof(CustomUnityItemManager));
    }

    private void OnGUI()
    {
        //Check if already exists

        this.options = Enum.GetNames(typeof(CostType));

        GUILayout.Space(10);
        GUILayout.Label("Item Stats", EditorStyles.boldLabel);
        this.itemName = EditorGUILayout.TextField("Item Name", this.itemName);
        this.itemSprite = (Sprite)EditorGUILayout.ObjectField("Sprite", this.itemSprite, typeof(Sprite), allowSceneObjects: true);
        this.itemValue = EditorGUILayout.IntField("Item Value", this.itemValue);
        this.index = EditorGUILayout.Popup("Item Type", index, options);
        this.type = (CostType)this.index;

        if (this.type == CostType.keyItem) SetInventoryInfo();        

        if (this.type == CostType.item)
        {
            //Items only
            GUILayout.Space(10);
            GUILayout.Label("Item Group", EditorStyles.boldLabel);

            this.createNewGroup = EditorGUILayout.Toggle("Make new group", this.createNewGroup);

            if (this.createNewGroup) SetNewItemGroup();
            else this.group = (ItemGroup)EditorGUILayout.ObjectField("Item Group", this.group, typeof(ItemGroup), allowSceneObjects: true);
        }
    }

    private void SetInventoryInfo()
    {
        GUILayout.Space(10);
        GUILayout.Label("Item Slot Info", EditorStyles.boldLabel);
        this.inventorySlot = EditorGUILayout.IntField("Inventory Slot", this.inventorySlot);
        this.signal = (SimpleSignal)EditorGUILayout.ObjectField("Key Item Signal", this.signal, typeof(SimpleSignal), allowSceneObjects: true);
    }

    private void SetNewItemGroup()
    {
        GUILayout.Space(10);
        GUILayout.Label("Item Group Info", EditorStyles.boldLabel);
        //Item Group Info
        this.itemGroupName = EditorGUILayout.TextField("Item Group Name", this.itemName);
        this.itemGroupSprite = (Sprite)EditorGUILayout.ObjectField("Item Group Sprite", this.itemSprite, typeof(Sprite), allowSceneObjects: true);
        this.soundEffect = (AudioClip)EditorGUILayout.ObjectField("Sound Effect", this.soundEffect, typeof(AudioClip), allowSceneObjects: true);

        GUILayout.Space(10);
        GUILayout.Label("Item Group Attributes", EditorStyles.boldLabel);
        //New Group
        this.maxValue = EditorGUILayout.IntField("Max Amount", this.maxValue);
        this.canConsume = EditorGUILayout.Toggle("Can Consume", this.canConsume);
        this.updateUI = EditorGUILayout.Toggle("Update Currency UI", this.updateUI);
        this.itemShopPrice = (ShopPriceUI)EditorGUILayout.ObjectField("Shop Price UI", this.itemShopPrice, typeof(ShopPriceUI), allowSceneObjects: true);

        SetInventoryInfo();
    }
}
#endif
