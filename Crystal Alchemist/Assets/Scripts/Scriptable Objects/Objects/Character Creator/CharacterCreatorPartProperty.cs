using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using AssetIcons;

public enum EnableMode
{
    always,
    name,
    race,
    nameAndRace
}

public enum RaceRestriction
{
    include,
    exclude
}

[System.Serializable]
public struct ColorTable
{
    public ColorGroup colorGroup;
    public Color highlight;
    public Color main;
    public Color shadows;
    public Color lines;
}

[System.Serializable]
public class ColorEffect
{
    public bool addGlow = false;

    [ShowIf("addGlow")]
    public Color glow;

    [ShowIf("addGlow")]
    [ColorUsage(true, true)]
    public Color default_glow;
}

[CreateAssetMenu(menuName = "Game/CharacterCreation/Property")]
public class CharacterCreatorPartProperty : ScriptableObject
{
    [BoxGroup("Enable Info")]
    [SerializeField]
    private EnableMode enableMode = EnableMode.always;

    [HideIf("enableMode", EnableMode.always)]
    [HideIf("enableMode", EnableMode.name)]
    [BoxGroup("Enable Info")]
    [SerializeField]
    private RaceRestriction restriction = RaceRestriction.include;

    [HideIf("enableMode", EnableMode.always)]
    [HideIf("enableMode", EnableMode.name)]
    [BoxGroup("Enable Info")]
    [SerializeField]
    private List<Race> races = new List<Race>();

    [AssetIcon]
    [PreviewField]
    [BoxGroup("Sprites")]
    [HorizontalGroup("Sprites/Preview")]
    [VerticalGroup("Sprites/Preview/Left")]
    [SerializeField]
    private Sprite front;

    [PreviewField]
    [BoxGroup("Sprites")]
    [VerticalGroup("Sprites/Preview/Right")]
    [SerializeField]
    private Sprite back;

    [BoxGroup("Color Info")]
    public bool canBeColored = true;

    [BoxGroup("Color Info")]
    [ShowIf("canBeColored")]
    [SerializeField]
    private List<ColorTable> colorTables = new List<ColorTable>();

    [BoxGroup("Color Info")]
    [SerializeField]
    [HideLabel]
    private ColorEffect colorEffect;

    [BoxGroup("Part Info")]
    public string category = "Head";

    [BoxGroup("Part Info")]
    public string parentName = "Ears";

    [BoxGroup("Part Info")]
    public string partName = "Elf Ears";

    [BoxGroup("Part Info")]
    [SerializeField]
    private Vector2Int size = new Vector2Int(32, 48);

    public Sprite GetSprite(bool isFront)
    {
        if (isFront) return this.front;
        else return this.back;
    }

    public Vector2Int GetSize()
    {
        return this.size;
    }

    public List<ColorTable> GetColorTable()
    {
        if (this.canBeColored) return this.colorTables;
        else return new List<ColorTable>();
    }

    public ColorEffect GetEffect()
    {
        return this.colorEffect;
    }

    public bool mandatory()
    {
        return this.enableMode == EnableMode.always;
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

    private bool raceEnabled(Race race)
    {
        if (this.restriction == RaceRestriction.include && this.races.Contains(race)) return true;
        else if (this.restriction == RaceRestriction.exclude && !this.races.Contains(race)) return true;
        return false;
    }
}
