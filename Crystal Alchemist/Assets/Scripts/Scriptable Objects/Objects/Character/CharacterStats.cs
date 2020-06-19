﻿using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;


#region Enums

public enum CharacterType
{
    Friend,
    Enemy,
    Object
}

public enum Gender
{
    male,
    female,
    none
}

#endregion

[CreateAssetMenu(menuName = "Game/Characters/Character Stats")]
public class CharacterStats : ScriptableObject
{
    [BoxGroup("Pflichtfelder")]
    [SerializeField]
    private CharacterType characterType = CharacterType.Object;

    ////////////////////////////////////////////////////////////////

    [TabGroup("Start-Values")]
    [OnValueChanged("UpdateLifeRespawn")]
    public bool isInvincible = false;

    [TabGroup("Start-Values")]
    public bool cannotDie = false;

    [TabGroup("Start-Values")]
    [OnValueChanged("UpdateStats")]
    [Tooltip("Leben, mit dem der Spieler startet")]
    [HideIf("isInvincible")]
    public float startLife = 1;

    [TabGroup("Start-Values")]
    [OnValueChanged("UpdateStats")]
    [Tooltip("Mana, mit dem der Spieler startet")]
    public float startMana = 1;

    [TabGroup("Start-Values")]
    [Tooltip("Movement-Speed in %, mit dem der Spieler startet")]
    [MinValue(0)]
    public float startSpeed = 100;

    [TabGroup("Start-Values")]
    [Tooltip("Geschwindigkeitsmodifier in % von Cooldown und Castzeit")]
    [MinValue(1)]
    public float startSpellSpeed = 100;

    [TabGroup("Start-Values")]
    public bool isMassive = false;

    [Space(10)]
    [TabGroup("Start-Values")]
    [Tooltip("Immunität von Statuseffekten")]
    public bool isImmuneToAllDebuffs = false;
    
    [TabGroup("Start-Values")]
    [HideIf("isImmuneToAllDebuffs")]
    [Tooltip("Immunität von Statuseffekten")]
    public List<StatusEffect> immunityToStatusEffects = new List<StatusEffect>();


    [Space(10)]
    [TabGroup("Spawn Values")]
    [Tooltip("Respawn-Zeit")]
    [HideIf("isInvincible")]
    public bool hasRespawn = true;

    [TabGroup("Spawn Values")]
    [Tooltip("Respawn-Zeit")]
    [ShowIf("hasRespawn")]
    [HideIf("isInvincible")]
    public float respawnTime = 30;

    [TabGroup("Spawn Values")]
    [Tooltip("Respawn-Zeit")]
    [ShowIf("hasRespawn")]
    [HideIf("isInvincible")]
    [MaxValue(100)]
    [MinValue(1)]
    public int respawnChance = 100;

    [TabGroup("Spawn Values")]
    [Tooltip("Respawn-Animation")]
    public DeathAnimation deathAnimation;


    ////////////////////////////////////////////////////////////////

    [BoxGroup("Upgrades")]
    [Tooltip("Maximales Life")]
    [OnValueChanged("UpdateStats")]
    [MinValue(1)]
    [HideIf("isInvincible")]
    public float maxLife = 1;

    [BoxGroup("Upgrades")]
    [Tooltip("Maximales Mana")]
    [OnValueChanged("UpdateStats")]
    [MinValue(0)]
    public float maxMana = 1;

    [Space(10)]
    [BoxGroup("Upgrades")]
    public bool canRegenerate = false;

    [BoxGroup("Upgrades")]
    [ShowIf("canRegenerate")]
    [Tooltip("Höhe der Lebensregeneration")]
    public float lifeRegeneration = 0;

    [BoxGroup("Upgrades")]
    [ShowIf("canRegenerate")]
    [Tooltip("Höhe der Manaregeneration")]
    public float manaRegeneration = 0;

    [BoxGroup("Upgrades")]
    [ShowIf("canRegenerate")]
    [Tooltip("Intervall der Regeneration")]
    [Range(0.1f, 3)]
    public float regenerationInterval = 1f;

    [Space(10)]
    [BoxGroup("Upgrades")]
    public bool canChangeBuffs = false;

    [BoxGroup("Upgrades")]
    [Range(0,100)]
    [ShowIf("canChangeBuffs")]
    [Tooltip("Verlängerung von Buffs in Prozent")]
    public int buffPlus = 0;

    [BoxGroup("Upgrades")]
    [Range(-100, 0)]
    [ShowIf("canChangeBuffs")]
    [Tooltip("Verkürzung von Debuffs in Prozent")]
    public int debuffMinus = 0;

    ////////////////////////////////////////////////////////////////

    [BoxGroup("Schaden")]
    public bool showDamageNumbers = true;

    [BoxGroup("Schaden")]
    [Tooltip("Wie stark (-) oder schwach (+) kann das Objekt zurück gestoßen werden?")]
    public float antiKnockback = 0;

    [BoxGroup("Schaden")]
    [Tooltip("Unverwundbarkeitszeit")]
    [Range(0, 10)]
    public float cannotBeHitTime = 0.3f;

    [Space(10)]
    [BoxGroup("Schaden")]
    [Tooltip("Farbe, wenn Gegner getroffen wurde")]
    public bool showHitcolor = true;

    [BoxGroup("Schaden")]
    [ShowIf("showHitcolor")]
    [ColorUsage(true,true)]
    public Color hitColor = Color.white;


    ////////////////////////////////////////////////////////////////


    [FoldoutGroup("Loot", expanded: false)]
    [Tooltip("Items und deren Wahrscheinlichkeit zwischen 1 und 100")]
    public LootTable lootTable;


    ////////////////////////////////////////////////////////////////


    [FoldoutGroup("Sound", expanded: false)]
    [Tooltip("Soundeffekt, wenn Gegner getroffen wurde")]
    public AudioClip hitSoundEffect;


    ////////////////////////////////////////////////////////////////

    public CharacterType GetCharacterType()
    {
        return this.characterType;
    }

    public string GetCharacterName()
    {
        return FormatUtil.GetLocalisedText(this.name+"_Name", LocalisationFileType.characters);
    }

    private void UpdateStats()
    {
        if (this.maxLife < this.startLife) this.maxLife = this.startLife;
        if (this.maxMana < this.startMana) this.maxMana = this.startMana;
    }

    private void UpdateLifeRespawn()
    {
        if (this.isInvincible) this.hasRespawn = false;
    }
}
