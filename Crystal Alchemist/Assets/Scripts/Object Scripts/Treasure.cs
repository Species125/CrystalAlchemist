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
        FormatUtil.set3DText(this.priceText, this.price + "", true, this.fontColor, this.outlineColor, this.outlineWidth);

        if (this.itemDrop != null && this.treasureType == TreasureType.normal) changeTreasureState(true);
    }

    #endregion


    #region Update Funktion

    public override void doOnUpdate()
    {
        //Close when opened
        if (this.currentState == objectState.opened && this.player != null && this.player.currentState == CharacterState.interact)
        {
            Destroy(this.itemDrop);

            if (this.treasureType == TreasureType.lootbox)
            {
                changeTreasureState(false);
                this.setLoot();
            }

            //Verstecke gezeigtes Item wieder
            for (int i = 0; i < this.showItem.transform.childCount; i++)
            {
                Destroy(this.showItem.transform.GetChild(i).gameObject);
            }
            this.showItem.SetActive(false);
        }
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

    private void changeTreasureState(bool openChest)
    {
        if (openChest)
        {
            AnimatorUtil.SetAnimatorParameter(this.anim, "isOpened", true);
            this.currentState = objectState.opened;
            this.context.SetActive(false);
        }
        else
        {
            AnimatorUtil.SetAnimatorParameter(this.anim, "isOpened", false);
            this.currentState = objectState.normal;
            this.context.SetActive(true);
        }
    }

    private void OpenChest()
    {
        this.player.reduceCurrency(this.price);
        changeTreasureState(true);
        AudioUtil.playSoundEffect(this.gameObject, this.soundEffect);

        if (this.soundEffect != null && this.itemDrop != null)
        {
            //Spiele Soundeffekte ab            
            AudioUtil.playSoundEffect(this.gameObject, this.soundEffectTreasure, GlobalValues.backgroundMusicVolume);

            //Zeige Item
            this.showTreasureItem();

            this.itemDrop.stats.CollectIt(this.player);
            this.player.GetComponent<PlayerDialog>().showDialog(this, DialogTextTrigger.success, this.itemDrop.stats);
        }
        else
        {
            //Kein Item drin
            this.player.GetComponent<PlayerDialog>().showDialog(this, DialogTextTrigger.empty);
        }
    }


    private void canOpenChest()
    {
        if (this.player.canUseIt(this.price)) OpenChest();
        else this.player.GetComponent<PlayerDialog>().showDialog(this, DialogTextTrigger.failed);
    }

    public void showTreasureItem()
    {
        //Item instanziieren und der Liste zurück geben und das Item anzeigen            
        this.showItem.SetActive(true);

        Collectable collectable = this.itemDrop.Instantiate(this.showItem.transform.position);
        collectable.SetAsTreasureItem(this.showItem.transform);
    }

    #endregion
}
