﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public enum DoorType
{    
    normal,    
    enemy,
    button,
    closed
}

public class Door : Interactable
{

    [FoldoutGroup("Tür-Attribute", expanded: false)]
    [EnumToggleButtons]
    [SerializeField]
    private DoorType doorType = DoorType.closed;

    private bool isOpen;
    private BoxCollider2D boxCollider;

    private void Start()
    {
        base.Start();
        this.boxCollider = GetComponent<BoxCollider2D>();

        if (this.isOpen) Utilities.UnityUtils.SetAnimatorParameter(this.animator, "isOpened", true);
    }

    public override void doOnUpdate()
    {
        if (!this.isPlayerInRange && this.isOpen && this.doorType == DoorType.normal)
        {
            //Normale Tür fällt von alleine wieder zu
            OpenCloseDoor(false, this.context);
        }
    }

    public override void doSomething()
    {
        if (this.doorType != DoorType.enemy && this.doorType != DoorType.button)
        {
            if (!this.isOpen)           
            {
                 if (this.doorType == DoorType.normal)
                {
                    //Normale Tür, einfach aufmachen
                    if (Utilities.Items.canOpenAndUpdateResource(this.currencyNeeded, this.item, this.player, this.price))
                    {
                        OpenCloseDoor(true, this.context);
                    }
                }
                else
                {
                    if (this.player != null) this.player.showDialogBox(this.text);
                }
            }                       
        }
        else if (this.doorType == DoorType.enemy)
        {
            //Wenn alle Feinde tot sind, OpenDoor()
        }
        else if (this.doorType == DoorType.button)
        {
            //Wenn Knopf gedrückt wurde, OpenDoor()
        }        
    }

    private void OpenCloseDoor(bool isOpen)
    {
        OpenCloseDoor(isOpen, null);
    }

    private void OpenCloseDoor(bool isOpen, GameObject contextClueChild)
    {
        this.isOpen = isOpen;
        Utilities.UnityUtils.SetAnimatorParameter(this.animator, "isOpened", this.isOpen);
        this.boxCollider.enabled = !this.isOpen;

        if (contextClueChild != null)
        {
            //Wenn Spieler in Reichweite ist und Tür zu ist -> Context Clue anzeigen! Ansonsten nicht.
            if (this.isOpen) contextClueChild.SetActive(false);
            else if (!this.isOpen && this.isPlayerInRange) contextClueChild.SetActive(true);
            else contextClueChild.SetActive(false);
        }

        Utilities.Audio.playSoundEffect(this.audioSource, this.soundEffect);
    }
}
