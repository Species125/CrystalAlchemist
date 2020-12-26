using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class CharacterCreatorMenu : MenuBehaviour
{
    [HideInInspector]
    public CharacterPreset creatorPreset;

    private CharacterPreset backup;

    [BoxGroup("Character Creator")]
    [Required]
    [SerializeField]
    private PlayerSaveGame saveGame;

    [BoxGroup("Character Creator")]
    [Required]
    [SerializeField]
    private SimpleSignal signal;

    [BoxGroup("Character Creator")]
    [Required]
    [SerializeField]
    private List<CharacterCreatorPartProperty> properties = new List<CharacterCreatorPartProperty>();
    

    public override void Start()
    {
        base.Start();

        this.creatorPreset = ScriptableObject.CreateInstance<CharacterPreset>();
        this.backup = ScriptableObject.CreateInstance<CharacterPreset>();

        GameUtil.setPreset(this.saveGame.playerPreset, this.backup);
        GameUtil.setPreset(this.saveGame.playerPreset, this.creatorPreset);
        updateGear();
    }

    [Button]
    public void AddProperties()
    {
        this.properties.Clear();
        this.properties.AddRange(Resources.LoadAll<CharacterCreatorPartProperty>("Scriptable Objects/Character Creation/Properties/"));
    }

    public void Abort()
    {
        GameUtil.setPreset(this.backup, this.saveGame.playerPreset);
        this.signal.Raise();
        base.ExitMenu();
    }

    public void updatePreview()
    {
        GameUtil.setPreset(this.creatorPreset, this.saveGame.playerPreset); //save Preset 
        this.signal.Raise();
    }

    public void updateGear()
    {
        List<CharacterCreatorGear> gearButtons = new List<CharacterCreatorGear>();
        UnityUtil.GetChildObjects<CharacterCreatorGear>(this.transform, gearButtons);

        //Only those who needed by name / name and race
        foreach (CharacterCreatorPartProperty part in this.properties)
        {
            CharacterPartData data = this.creatorPreset.GetCharacterPartData(part.parentName, part.partName);
            bool enableIt = part.enableIt(this.creatorPreset.getRace(), data);

            if (enableIt) this.creatorPreset.AddCharacterPartData(part.parentName, part.partName);
            else this.creatorPreset.RemoveCharacterPartData(part.parentName, part.partName);

            //check if colorgroup exists
        }
    }

    public CharacterCreatorPartProperty GetProperty(string name, string parent)
    {
        foreach (CharacterCreatorPartProperty part in this.properties)
        {
            if (part.partName == name && part.parentName == parent) return part;
        }
        return null;
    }
}
