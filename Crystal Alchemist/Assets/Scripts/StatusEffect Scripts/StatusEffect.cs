﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

#region Enums
public enum StatusEffectType
{
    buff,
    debuff
}

public enum StatusEffectEndType
{
    time,
    mana
}
#endregion

public class StatusEffect : MonoBehaviour
{
    #region Attribute
    [Header("Statuseffekt Pflichtfelder")]
    [Tooltip("Name des Statuseffekts")]
    public string statusEffectName;
    [Tooltip("Icon des Statuseffekts für das UI")]
    public Sprite iconSprite;
    [Tooltip("Signal zum Update der UI")]
    public Signal updateUI;
    [Tooltip("Wann endet der Statuseffekt?")]
    public StatusEffectEndType endType = StatusEffectEndType.time;

    [Header("Statuseffekt Attribute")]
    [Tooltip("Beschreibung des Statuseffekts")]
    public string statusEffectDescription;
    [Tooltip("Dauer des Statuseffekts (nur bei Typ Time)")]
    [Range(1, Utilities.maxFloatSmall)]
    public float statusEffectDuration = 1;
    [Tooltip("Intervall, wann der Effekt eintrifft. 0 = 1x nur am Anfang")]
    [Range(0, Utilities.maxFloatSmall)]
    public float statusEffectInterval = 1;
    [Tooltip("Mana-Kosten des Effekts (nur bei Typ Mana)")]
    [Range(0, Utilities.maxFloatSmall)]
    public float statusEffectManaDrain = 0;
    [Tooltip("Intervall des Mana-Entzugs (nur bei Typ Mana)")]
    [Range(0, Utilities.maxFloatSmall)]
    public float statusEffectManaDrainInterval = 1;
    [Tooltip("Anzahl der maximalen gleichen Effekte (Stacks)")]
    [Range(1, Utilities.maxFloatSmall)]
    public float maxStacks = 1;
    [Tooltip("Darf der Statuseffekt einen gleichen Effekt überschreiben? (nur bei Typ Time)")]
    public bool canOverride = false;
    [Tooltip("Darf der Statuseffekt den gleichen Effekt deaktivieren? (nur bei Typ Mana)")]
    public bool canDeactivateIt = false;
    [Tooltip("Darf der Statuseffekt geheilt werden?")]
    public bool canBeDispelled = true;
    [Tooltip("Handelt es sich um einen positiven oder negativen Effekt?")]
    public StatusEffectType statusEffectType = StatusEffectType.debuff;
    [Tooltip("Farbe während der Dauer des Statuseffekts")]
    public Color statusEffectColor;

    [Header("Statuseffekt Wirkung")]
    public Script script;  
    
    [HideInInspector]
    public Character target;

    [HideInInspector]
    public float statusEffectTimeLeft;
    private float elapsed;
    private float elapsedMana;
    [HideInInspector]
    public float timeDistortion = 1;
    #endregion


    #region Start Funktionen (Init)
    void Start()
    {
        init();
    }

    //Signal?

    public void updateTimeDistortion(float distortion)
    {
        this.timeDistortion = 1 + (distortion/100);
    }

    private void init()
    {
        if (this.endType == StatusEffectEndType.time) this.statusEffectTimeLeft = this.statusEffectDuration;
        if (this.endType == StatusEffectEndType.mana) this.statusEffectTimeLeft = Mathf.Infinity;
        if (this.target != null && this.target.spriteRenderer.color == this.target.global.color) this.target.spriteRenderer.color = this.statusEffectColor;

        if (this.statusEffectInterval == 0)
        {
            //Einmalig
            doEffect();
            this.statusEffectInterval = Mathf.Infinity;
        }
        else
        {
            //erste Wirkung
            this.elapsed = this.statusEffectInterval;
            this.elapsedMana = this.statusEffectManaDrainInterval;
        }

        this.script = Utilities.setScript(this.GetComponent<Script>(), this.transform, this.target);        
    }

    #endregion


    #region Update

    private void Update()
    {
        //TODO: Performance?
        this.updateUI.Raise();

        if(this.endType == StatusEffectEndType.time) this.statusEffectTimeLeft -= (Time.deltaTime * this.timeDistortion);
        
        this.elapsed += (Time.deltaTime * this.timeDistortion);
        this.elapsedMana += (Time.deltaTime * this.timeDistortion);

        if (this.endType == StatusEffectEndType.mana
            && this.statusEffectManaDrainInterval > 0
            && this.elapsedMana >= statusEffectManaDrainInterval
            && this.target != null
            && this.target.mana - this.statusEffectManaDrain >= 0)
        {
            //Reduziere Mana solange der Effekt aktiv ist   
            this.target.updateMana(-this.statusEffectManaDrain);
            this.elapsedMana = 0;
        }

        if ((this.endType == StatusEffectEndType.time 
            && this.statusEffectInterval > 0 
            && this.statusEffectTimeLeft > 0 
            && this.elapsed >= statusEffectInterval)
            ||
            (this.endType == StatusEffectEndType.mana
            && this.statusEffectInterval > 0
            && this.elapsed >= statusEffectInterval
            && this.target != null
            && this.target.mana - this.statusEffectManaDrain >= 0))
        {
            //solange der Effekt aktiv ist und das Intervall erreicht ist, soll etwas passieren. Intervall zurück setzen
            doEffect();
            this.elapsed = 0;
        }        
        else if ((this.endType == StatusEffectEndType.time 
                 && this.statusEffectTimeLeft <= 0)
                 || (this.endType == StatusEffectEndType.mana
            && this.target != null
            && this.target.mana - this.statusEffectManaDrain < 0))
        {
            //Zerstöre Effekt, wenn die Zeit abgelaufen ist
            destroy();
        }        
    }

    public void destroy()
    {
        if (this.target != null)
        {
            //Charakter-Farbe zurücksetzen
            this.target.spriteRenderer.color = this.target.global.color;

            //Statuseffekt von der Liste entfernen
            if (this.statusEffectType == StatusEffectType.debuff)
            {
                this.target.GetComponent<Character>().debuffs.Remove(this);
            }
            else if (this.statusEffectType == StatusEffectType.buff)
            {
                this.target.GetComponent<Character>().buffs.Remove(this);
            }

            if(this.script != null) this.script.onDestroy();
        }

        //GUI updaten und Objekt kurz danach zerstören
        this.updateUI.Raise();
        Destroy(this.gameObject, 0.2f);
    }

    private void doEffect()
    {
        //Wirkung abhängig vom Script!
        if (this.target != null)
        {             
            if (this.script != null) this.script.onUpdate();

            this.target.spriteRenderer.color = this.statusEffectColor;
        }
    }

    #endregion
}
