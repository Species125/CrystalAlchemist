﻿using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(menuName = "Skills/Skill Book Info")]
public class SkillBookInfo : ScriptableObject
{
    [BoxGroup("Pflichtfelder")]
    [Tooltip("Beschreibung des Skills")]
    [TextArea]
    public string skillDescription;

    [BoxGroup("Pflichtfelder")]
    [Tooltip("Beschreibung des Skills")]
    [TextArea]
    public string skillDescriptionEnglish;

    [BoxGroup("Pflichtfelder")]
    [Tooltip("Beschreibung des Skills")]
    [EnumToggleButtons]
    public SkillType category = SkillType.magical;

    [BoxGroup("Pflichtfelder")]
    [Tooltip("Sortierung")]
    public int orderIndex = 10;

    [BoxGroup("Sound und Icons")]
    [Tooltip("Icon für den Spieler")]
    public Sprite icon;
}
