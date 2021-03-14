﻿using UnityEngine;
using System;
using UnityEditor;

namespace CrystalAlchemist
{
    public class CustomUnityItemManager : EditorWindow
    {
        string[] options;
        int folderindex;
        string[] folders = {"Consumables", "Currencies", "Inventory Items", "Key Items"};
        string parent = "Assets/Resources/Scriptable Objects/Items/";
        string itemName = "";
        int index = 0;
        int itemValue = 1;
        int inventorySlot;
        SimpleSignal signal;
        Sprite itemSprite;
        ItemType type;

        bool showInInventory;
        bool createNewGroup;
        InventoryItem group;
        AudioClip soundEffect;
        AudioClip uiSoundEffect;
        bool canConsume = true;
        bool updateUI = false;
        int maxValue = 9999;

        string itemGroupName = "";
        Sprite itemGroupSprite;
        ShopPriceUI itemShopPrice;

        bool isItem = false;


        //TODO: Check if Name already exists


        [MenuItem("Alchemist Menu/Tools/Item Wizard")]
        public static void ItemWizard()
        {
            CustomUnityItemManager window =
                (CustomUnityItemManager) GetWindow(typeof(CustomUnityItemManager), true, "Item Creation");
        }

        private void OnGUI()
        {
            this.options = Enum.GetNames(typeof(CostType));

            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            GUILayout.Space(10);
            GUILayout.Label("Item Stats", EditorStyles.boldLabel);
            this.itemName = EditorGUILayout.TextField("Item Name", this.itemName);
            this.itemSprite =
                (Sprite) EditorGUILayout.ObjectField("Sprite", this.itemSprite, typeof(Sprite),
                    allowSceneObjects: true);
            this.itemValue = EditorGUILayout.IntField("Item Value", this.itemValue);
            this.index = EditorGUILayout.Popup("Item Type", index, options);
            this.type = (ItemType) this.index;
            this.soundEffect = (AudioClip) EditorGUILayout.ObjectField("Sound Effect", this.soundEffect,
                typeof(AudioClip), allowSceneObjects: true);

            if (this.type == ItemType.inventory) //key item
            {
                SetInventoryInfo();
                this.group = null;
            }

            if (this.type == ItemType.inventory)
            {
                //Items only
                EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
                GUILayout.Space(10);
                GUILayout.Label("Item Group", EditorStyles.boldLabel);

                this.createNewGroup = EditorGUILayout.Toggle("Make new group", this.createNewGroup);

                if (this.createNewGroup) SetNewItemGroup();
                else
                    this.group = (InventoryItem) EditorGUILayout.ObjectField("Item Group", this.group, typeof(InventoryItem),
                        allowSceneObjects: true);
            }

            if (this.type == ItemType.inventory) this.folderindex = 3;//key item
            else if (this.type == ItemType.inventory) this.folderindex = 2;
            else this.folderindex = 0;

            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            GUILayout.Space(20);
            this.folderindex = EditorGUILayout.Popup("Target Folder", this.folderindex, this.folders);

            bool create = GUILayout.Button("Create Item");

            if (create) CreateItem();
        }

        private void SetInventoryInfo()
        {
            this.inventorySlot = EditorGUILayout.IntField("Inventory Slot Index", this.inventorySlot);
            if (this.type == ItemType.inventory)//key item
                this.signal = (SimpleSignal) EditorGUILayout.ObjectField("Key Item Signal", this.signal,
                    typeof(SimpleSignal), allowSceneObjects: true);
            this.showInInventory = true;
        }

        private void SetNewItemGroup()
        {
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            GUILayout.Space(10);
            GUILayout.Label("Item Group Info", EditorStyles.boldLabel);
            //Item Group Info
            this.itemGroupName = EditorGUILayout.TextField("Item Group Name", this.itemGroupName);
            this.itemGroupSprite = (Sprite) EditorGUILayout.ObjectField("Item Group Sprite", this.itemGroupSprite,
                typeof(Sprite), allowSceneObjects: true);

            GUILayout.Space(10);
            GUILayout.Label("Item Group Attributes", EditorStyles.boldLabel);
            //New Group
            this.maxValue = EditorGUILayout.IntField("Max Amount", this.maxValue);
            this.canConsume = EditorGUILayout.Toggle("Can Consume", this.canConsume);

            this.updateUI = EditorGUILayout.Toggle("Update Currency UI", this.updateUI);
            if (this.updateUI)
                this.uiSoundEffect = (AudioClip) EditorGUILayout.ObjectField("UI Sound Effect", this.uiSoundEffect,
                    typeof(AudioClip), allowSceneObjects: true);
            else this.uiSoundEffect = null;
            if (this.canConsume)
                this.itemShopPrice = (ShopPriceUI) EditorGUILayout.ObjectField("Shop Price UI", this.itemShopPrice,
                    typeof(ShopPriceUI), allowSceneObjects: true);
            else this.itemShopPrice = null;

            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            GUILayout.Space(10);
            GUILayout.Label("Inventory Stats", EditorStyles.boldLabel);
            this.showInInventory = EditorGUILayout.Toggle("Show Item in Inventory", this.showInInventory);
            if (this.showInInventory) SetInventoryInfo();
        }

        private void CreateItem()
        {
            InventoryItem itemGroup = this.group;

            ItemInfo info = CreateInstance<ItemInfo>();
            info.SetInfo(this.itemSprite);
            string infoPath = parent + "Item Infos/Item Stat Info/" + this.itemName + " Info.asset";
            CreateAsset(info, infoPath);

            if (this.type == ItemType.inventory)
            {
                if (this.showInInventory)
                {
                    string slotName = this.itemName;
                    if (this.type == ItemType.inventory) slotName = this.itemGroupName;
                    string itemSlotPath = parent + "Item Slot Infos/" + this.folders[this.folderindex] + "/" +
                                          slotName + ".asset";
                }

                if (this.createNewGroup)
                {
                    ItemInfo groupinfo = CreateInstance<ItemInfo>();
                    groupinfo.SetInfo(this.itemGroupSprite);
                    string groupinfoPath = parent + "Item Infos/Item Group Info/" + this.itemGroupName +
                                           " Group Info.asset";
                    CreateAsset(groupinfo, groupinfoPath);

                    InventoryItem group = CreateInstance<InventoryItem>();
                    group.SetGroup(this.maxValue, this.canConsume, this.updateUI, this.soundEffect, this.itemShopPrice);
                    group.info = groupinfo;
                    string groupPath = parent + "Item Groups/" + this.folders[this.folderindex] + "/" +
                                       this.itemGroupName + ".asset";
                    CreateAsset(group, groupPath);

                    itemGroup = group;
                }
            }

            ItemStats stats = CreateInstance<ItemStats>();
            //stats.SetStats(this.itemValue, this.type, this.soundEffect, info);
            if (this.type == ItemType.inventory) stats.inventoryItem = itemGroup;
            string statsPath = parent + "Item Stats/" + this.folders[this.folderindex] + "/" + this.itemName + ".asset";
            CreateAsset(stats, statsPath);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            EditorGUIUtility.PingObject(stats);
            this.Close();
        }

        private void CreateAsset(UnityEngine.Object obj, string strPath)
        {
            string path = strPath;
            //string[] results = AssetDatabase.FindAssets(path);
            //if (results.Length > 0) path = path.Replace(".asset", " "+results.Length+".asset");
            AssetDatabase.CreateAsset(obj, path);
        }
    }
}
