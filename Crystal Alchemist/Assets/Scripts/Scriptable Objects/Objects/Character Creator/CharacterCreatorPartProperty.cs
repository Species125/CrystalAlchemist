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
    public Color glow;
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
