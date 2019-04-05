﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Treasure : Interactable
{
    #region Attribute   

    [Header("Truhen-Attribute")]
    public AudioClip soundEffectTreasure;

    #endregion


    #region Start Funktionen

    private void Start()
    {
        init();        
    }

    #endregion


    #region Update Funktion

    private void Update()
    {
        if (this.isPlayerInRange && this.currentState != objectState.opened && Input.GetButtonDown("A-Button"))
        {
            OpenChest();           
        }
        else if (this.isPlayerInRange && Input.GetButtonDown("A-Button"))
        {
            //Wenn Truhe geöffnet wurde und der Dialog weggeklickt wird
            //TODO, geht noch besser

            if (this.dialogScript != null)
            {
                this.dialogScript.showDialog(this.character, "");
                this.dialogScript = null;                
            }

            //Entferne Item aus der Welt und leere die Liste
            foreach(GameObject item in this.items)
            {
                Destroy(item);
            }
            this.items.Clear();
        }

        if (this.context != null)
        {
            //Wenn Spieler in Reichweite ist und Tür zu ist -> Context Clue anzeigen! Ansonsten nicht.
            if (this.currentState == objectState.opened) this.context.SetActive(false);
            else if (this.currentState != objectState.opened && this.isPlayerInRange) this.context.SetActive(true);
            else this.context.SetActive(false);
        }
    }

    #endregion


    #region Treasure Chest Funktionen (open, show Item)

    private void OpenChest()
    {        
        Utilities.SetParameter(this.animator, "isOpened", true);
        this.currentState = objectState.opened;

        if (this.soundEffect != null && this.items.Count > 0)
        {
            //Spiele Soundeffekte ab
            Utilities.playSoundEffect(this.audioSource, this.soundEffect, this.soundEffectVolume);
            Utilities.playSoundEffect(this.audioSource, this.soundEffectTreasure, this.soundEffectVolume);

            //Zeige Item
            this.showTreasureItem();

            //OLD, muss besser gehen!
            //Gebe Item dem Spieler
            foreach (GameObject item in this.items) item.GetComponent<Item>().collect(this.character.GetComponent<PlayerMovement>(), false);
        }
        else
        {
            //Kein Item drin
            this.text = "Die Kiste ist leer... .";
        }

        if (this.dialogScript != null)
        {
            this.dialogScript.showDialog(this.character, this.text);
        }        
    }

    public void showTreasureItem()
    {
        for (int i = 0; i < this.items.Count; i++)
        {
            //Item instanziieren und der Liste zurück geben und das Item anzeigen
            this.items[i] = Instantiate(this.items[i], this.transform.position, Quaternion.identity, this.transform);
            this.items[i].GetComponent<Item>().showFromTreasure();
        }
    }

    #endregion
}
