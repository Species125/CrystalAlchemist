﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Sirenix.OdinInspector;

public enum TreasureType
{
    normal,
    lootbox
}

public class Treasure : Rewardable
{
    #region Attribute   

    [FoldoutGroup("Treasure Options", expanded: false)]
    [Required]
    public Animator anim;

    [BoxGroup("Mandatory")]
    [Required]
    public GameObject showItem;

    [FoldoutGroup("Treasure Options", expanded: false)]
    public AudioClip soundEffectTreasure;

    [FoldoutGroup("Treasure Options", expanded: false)]
    [EnumToggleButtons]
    public TreasureType treasureType = TreasureType.normal;

    [FoldoutGroup("TextMeshPro Options", expanded: false)]
    public TextMeshPro priceText;

    [FoldoutGroup("TextMeshPro Options", expanded: false)]
    public Color fontColor;

    [FoldoutGroup("TextMeshPro Options", expanded: false)]
    public Color outlineColor;

    [FoldoutGroup("TextMeshPro Options", expanded: false)]
    public float outlineWidth = 0.25f;




    #endregion


    #region Start Funktionen

    public override void Start()
    {
        base.Start();

        CustomUtilities.Format.set3DText(this.priceText, this.price + "", true, this.fontColor, this.outlineColor, this.outlineWidth);
    }

    #endregion


    #region Update Funktion

    public override void doOnUpdate()
    {
        //Close when opened
        if (this.currentState == objectState.opened && this.player != null && this.player.currentState == CharacterState.interact)
        {           
            //Entferne Item aus der Welt und leere die Liste
            foreach (Item item in this.inventory)
            {
                Destroy(item.gameObject);
            }
            this.inventory.Clear();

            if (this.treasureType == TreasureType.lootbox)
            {
                CustomUtilities.UnityUtils.SetAnimatorParameter(this.anim, "isOpened", false);
                this.currentState = objectState.normal;
                CustomUtilities.Items.setItem(this.lootTable, this.multiLoot, this.inventory, this.lootParentObject);
            }
        }
        /*
        if (this.context != null)
        {
            //Wenn Spieler in Reichweite ist und Truhe zu ist -> Context Clue anzeigen! Ansonsten nicht.
            if (this.currentState == objectState.opened) this.context.SetActive(false);
            else if (this.currentState != objectState.opened && this.isPlayerInRange) this.context.SetActive(true);
            else this.context.SetActive(false);
        }*/
    }

    public override void doSomethingOnSubmit()
    {        
        if (this.currentState != objectState.opened)
        {
            if (this.treasureType == TreasureType.normal)
            {
                canOpenChest();
            }
            else if (this.treasureType == TreasureType.lootbox)
            {
                canOpenChest();
            }
        }              
    }

    #endregion


    #region Treasure Chest Funktionen (open, show Item)

    private void OpenChest()
    {        
        CustomUtilities.UnityUtils.SetAnimatorParameter(this.anim, "isOpened", true);
        this.currentState = objectState.opened;

        if (this.soundEffect != null && this.inventory.Count > 0)
        {
            //Spiele Soundeffekte ab
            CustomUtilities.Audio.playSoundEffect(this.audioSource, this.soundEffect);
            CustomUtilities.Audio.playSoundEffect(this.audioSource, this.soundEffectTreasure);

            //Zeige Item
            this.showTreasureItem();

            //OLD, muss besser gehen!
            //Gebe Item dem Spieler
            foreach (Item item in this.inventory)
            {
                this.player.collect(item, false);
                CustomUtilities.DialogBox.showDialog(this, this.player, DialogTextTrigger.success, item);
            }                
        }
        else
        {
            //Kein Item drin
            CustomUtilities.DialogBox.showDialog(this, this.player, DialogTextTrigger.empty);
        }
    }


    private void canOpenChest()
    {
        if (CustomUtilities.Items.canOpenAndUpdateResource(this.currencyNeeded, this.item, this.player, this.price)) OpenChest();
        else CustomUtilities.DialogBox.showDialog(this, this.player, DialogTextTrigger.failed);
    }

    public void showTreasureItem()
    {
        for (int i = 0; i < this.inventory.Count; i++)
        {
            //Item instanziieren und der Liste zurück geben und das Item anzeigen
            
            this.showItem.SetActive(true);
            Item item = Instantiate(this.inventory[i], this.showItem.transform.position, Quaternion.identity, this.showItem.transform);
            item.GetComponent<BoxCollider2D>().enabled = false;
            if (item.shadowRenderer != null) item.shadowRenderer.enabled = false;

            //this.inventory[i].GetComponent<Item>().showFromTreasure();
        }
    }

    #endregion


    /*
    #region Treasure specific Function
    public void showFromTreasure()
    {
        //Als Kisten-Item darf es nicht einsammelbar sein und muss als Position die Kiste haben

        this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + 0.5f);
        //SortingGroup group = this.GetComponent<SortingGroup>();
        //if (group != null) group.sortingOrder = 1;
        this.GetComponent<BoxCollider2D>().enabled = false;
        this.anim.enabled = true;
       // if (this.shadowRenderer != null) this.shadowRenderer.enabled = false;
    }

    #endregion*/
}
