﻿using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using AssetIcons;

public enum EnableMode
{
    name,
    race,
    nameAndRace
}

[System.Serializable]
public struct ColorTable
{
    public ColorGroup colorGroup;
    public Color highlight;
    public Color main;
    public Color shadows;
    public Color lines;
    public Color glow;
}

[CreateAssetMenu(menuName = "Game/CharacterCreation/Property")]
public class CharacterCreatorPartProperty : ScriptableObject
{
    [BoxGroup("Enable Info")]
    public bool neverDisable = false;

    [HideIf("neverDisable", true)]
    [BoxGroup("Enable Info")]
    [SerializeField]
    private EnableMode enableMode;

    [HideIf("neverDisable", true)]
    [HideIf("enableMode", EnableMode.name)]
    [BoxGroup("Enable Info")]
    [SerializeField]
    private List<Race> restrictedRaces = new List<Race>();

    [AssetIcon]
    [PreviewField]
    [HorizontalGroup("Preview")]
    [VerticalGroup("Preview/Left")]
    [SerializeField]
    private Sprite front;

    [PreviewField]
    [VerticalGroup("Preview/Right")]
    [SerializeField]
    private Sprite back;

    [BoxGroup("Color Info")]
    public List<ColorTable> colorTables = new List<ColorTable>();

    [BoxGroup("Part Info")]
    public string category = "Head";

    [BoxGroup("Part Info")]
    public string parentName = "Ears";

    [BoxGroup("Part Info")]
    public string partName = "Elf Ears";


    public Sprite GetSprite(bool isFront)
    {
        if (isFront) return this.front;
        else return this.back;
    }

    public string getFullPath()
    {
        return this.category + "/" + this.parentName + "/" + this.partName + ".png";
    }

    public bool enableIt(Race race, CharacterPartData data)
    {
        if ((this.enableMode == EnableMode.race && raceEnabled(race))
         || (this.enableMode == EnableMode.nameAndRace && raceEnabled(race) && data != null)
         || (this.enableMode == EnableMode.name && data != null)) return true;

        return false;
    }

    public bool raceEnabled(Race race)
    {
        if (this.restrictedRaces.Count == 0 || this.restrictedRaces.Contains(race)) return true;
        return false;
    }
}
