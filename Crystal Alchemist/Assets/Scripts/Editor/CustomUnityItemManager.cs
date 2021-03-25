using UnityEngine;
using System;
using UnityEditor;

namespace CrystalAlchemist
{
    public class CustomUnityItemManager : EditorWindow
    {

        int folderindex;
        string[] folders = { "Consumables", "Outfits", "Abilities", "Inventory Items", "Currencies", "Housing Items", "Crafting Items", "Key Items" };
        string parent = "Assets/Resources/Scriptable Objects/Items/";

        SimpleSignal signal;

        bool createNewGroup;

        InventoryItem inventoryItem;
        Ability ability;
        CharacterCreatorProperty outfit;

        AudioClip uiSoundEffect;
        bool canBeSold = true;
        bool updateUI = false;
        int maxValue = 9999;
        int shopValue = 1;

        string itemGroupName = "";
        Sprite itemGroupSprite;
        ShopPriceUI itemShopPrice;

        string[] rarities;

        string[] itemtypes;
        string itemName = "";
        int itemTypeIndex = 0;
        int itemValue = 1;
        Sprite itemSprite;
        ItemType itemType;
        int itemRarityIndex;
        ItemRarity itemRarity;

        bool isKeyItem = false;
        int inventoryTypeIndex;
        string[] inventoryTypes;
        InventoryType inventoryType;
        int inventoryRarityIndex;
        ItemRarity inventoryRarity;

        bool same = true;

        //TODO: Check if Name already exists


        [MenuItem("Alchemist Menu/Tools/Item Wizard")]
        public static void ItemWizard()
        {
            CustomUnityItemManager window =
                (CustomUnityItemManager)GetWindow(typeof(CustomUnityItemManager), true, "Item Creation");
        }

        private void OnGUI()
        {
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            GUILayout.Space(10);
            GUILayout.Label("Item Stats", EditorStyles.boldLabel);
            this.itemName = EditorGUILayout.TextField("Item Name", this.itemName);
            this.itemValue = EditorGUILayout.IntField(new GUIContent("Item Value", "The Value of the item"), this.itemValue);

            this.itemtypes = Enum.GetNames(typeof(ItemType));
            this.itemTypeIndex = EditorGUILayout.Popup("Item Type", this.itemTypeIndex, this.itemtypes);
            this.itemType = (ItemType)this.itemTypeIndex;

            this.rarities = Enum.GetNames(typeof(ItemRarity));
            this.itemRarityIndex = EditorGUILayout.Popup("Item Rarity", this.itemRarityIndex, this.rarities);
            this.itemRarity = (ItemRarity)this.itemRarityIndex;

            //this.soundEffect = (AudioClip)EditorGUILayout.ObjectField("Sound Effect", this.soundEffect,
            //    typeof(AudioClip), allowSceneObjects: true);

            if (this.itemType == ItemType.inventory)
            {
                //Items only
                EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
                GUILayout.Space(10);
                GUILayout.Label("Inventory Item", EditorStyles.boldLabel);

                this.itemSprite =
                                (Sprite)EditorGUILayout.ObjectField("Sprite", this.itemSprite, typeof(Sprite),
                                    allowSceneObjects: true);

                this.createNewGroup = EditorGUILayout.Toggle(new GUIContent("New Inventory Item", "Create new Inventory Item"), this.createNewGroup);

                if (this.createNewGroup) SetNewInventoryItem();
                else
                    this.inventoryItem = (InventoryItem)EditorGUILayout.ObjectField("Inventory Item", this.inventoryItem, typeof(InventoryItem),
                        allowSceneObjects: true);
            }
            else if (this.itemType == ItemType.outfit)
            {
                this.folderindex = 1;
                this.outfit = (CharacterCreatorProperty)EditorGUILayout.ObjectField("Outfit", this.outfit, typeof(CharacterCreatorProperty),
                        allowSceneObjects: true);
            }
            else if (this.itemType == ItemType.ability)
            {
                this.folderindex = 2;
                this.ability = (Ability)EditorGUILayout.ObjectField("Ability", this.ability, typeof(Ability),
                        allowSceneObjects: true);
            }
            else this.folderindex = 0;

            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            GUILayout.Space(20);
            this.folderindex = EditorGUILayout.Popup("Target Folder", this.folderindex, this.folders);

            bool create = GUILayout.Button("Create Item");

            if (create)
            {
                CreateItem();
            }
        }

        private void SetNewInventoryItem()
        {
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            GUILayout.Space(10);
            GUILayout.Label("Inventory Item Info", EditorStyles.boldLabel);

            this.same = EditorGUILayout.Toggle(new GUIContent("Use Stat Values", "Create Inventory Item based on Stat"), this.same);

            this.inventoryTypes = Enum.GetNames(typeof(InventoryType));
            this.inventoryTypeIndex = EditorGUILayout.Popup("Inventory Type", this.inventoryTypeIndex, this.inventoryTypes);
            this.inventoryType = (InventoryType)this.inventoryTypeIndex;

            if (!this.same)
            {
                //Item Group Info
                this.itemGroupName = EditorGUILayout.TextField("Inventory Item Name", this.itemGroupName);
                this.itemGroupSprite = (Sprite)EditorGUILayout.ObjectField("Inventory Item Sprite", this.itemGroupSprite,
                    typeof(Sprite), allowSceneObjects: true);

                this.inventoryRarityIndex = EditorGUILayout.Popup("Inventory Rarity", this.inventoryRarityIndex, this.rarities);
                this.inventoryRarity = (ItemRarity)this.inventoryRarityIndex;
            }
            else
            {
                this.itemGroupName = this.itemName + "s";
                this.itemGroupSprite = this.itemSprite;
                this.inventoryRarity = this.itemRarity;
            }

            this.isKeyItem = false;

            if (inventoryType == InventoryType.item) this.folderindex = 3;
            else if (inventoryType == InventoryType.currency) this.folderindex = 4;
            else if (inventoryType == InventoryType.housing) this.folderindex = 2;
            else if (inventoryType == InventoryType.crafting) this.folderindex = 6;
            else
            {
                this.isKeyItem = true;
                this.folderindex = 7;
            }

            GUILayout.Space(10);
            GUILayout.Label("Inventory Item Attributes", EditorStyles.boldLabel);
            //New Group

            if (!this.isKeyItem)
            {
                this.maxValue = EditorGUILayout.IntField("Max Amount", this.maxValue);
                this.canBeSold = EditorGUILayout.Toggle("Can used in Shop", this.canBeSold);

                if (this.inventoryType == InventoryType.currency)
                {
                    this.updateUI = EditorGUILayout.Toggle("Update Currency UI", this.updateUI);
                    if (this.updateUI)
                        this.uiSoundEffect = (AudioClip)EditorGUILayout.ObjectField("UI Sound Effect", this.uiSoundEffect,
                            typeof(AudioClip), allowSceneObjects: true);
                    else this.uiSoundEffect = null;
                }
            }
            else
            {
                this.maxValue = 1;
                this.canBeSold = false;
                this.updateUI = false;
                this.uiSoundEffect = null;
            }

            if (this.canBeSold)
            {
                this.itemShopPrice = (ShopPriceUI)EditorGUILayout.ObjectField("Shop Price UI", this.itemShopPrice,
                    typeof(ShopPriceUI), allowSceneObjects: true);

                this.shopValue = EditorGUILayout.IntField("Shop Value", this.shopValue);
            }
            else
            {
                this.itemShopPrice = null;
                this.shopValue = 1;
            }

            if (this.inventoryType == InventoryType.artifacts)
            {
                this.signal = (SimpleSignal)EditorGUILayout.ObjectField("Signal", this.signal, typeof(SimpleSignal),
                        allowSceneObjects: true);
            }
        }

        private void CreateItem()
        {
            InventoryItem inventoryItem = this.inventoryItem;

            if (this.itemType == ItemType.inventory && this.createNewGroup) inventoryItem = CreateInventoryItem();

            ItemStats stats = CreateStats(inventoryItem);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            EditorGUIUtility.PingObject(stats);
            this.Close();
        }

        private InventoryItem CreateInventoryItem()
        {
            InventoryItem group = CreateInstance<InventoryItem>();
            group.SetGroup(this.maxValue, this.canBeSold, this.updateUI, this.uiSoundEffect, this.itemShopPrice, this.shopValue, this.inventoryRarity, this.inventoryType, this.itemGroupSprite);

            CreateAsset(group, "Inventory Items", this.itemGroupName);
            return group;
        }

        private ItemStats CreateStats(InventoryItem inventoryItem)
        {
            ItemStats stats = CreateInstance<ItemStats>();
            stats.SetStats(this.itemValue, this.itemType, this.itemRarity, this.itemSprite, inventoryItem, this.ability, this.outfit);
           
            CreateAsset(stats, "Item Stats", this.itemName);
            return stats;
        }


        private void CreateAsset(UnityEngine.Object obj, string mainFolder, string fileName)
        {
            string folder = this.parent + mainFolder + "/" + this.folders[this.folderindex];
            string path = folder + "/" + fileName + ".asset";

            try
            {             
                bool validFolder = AssetDatabase.IsValidFolder(folder);
                if (!validFolder) AssetDatabase.CreateFolder(this.parent + mainFolder, this.folders[this.folderindex]);

                AssetDatabase.CreateAsset(obj, path);
            }
            catch (Exception ex)
            {
                Debug.LogError("Problem creating " + path +" -> " + ex.ToString());
            }
        }
    }
}
