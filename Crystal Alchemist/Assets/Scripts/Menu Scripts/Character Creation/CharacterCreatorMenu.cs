using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class CharacterCreatorMenu : MenuBehaviour
{
    [BoxGroup("Character Creator")]
    [Required]
    public CharacterPreset creatorPreset;

    [BoxGroup("Character Creator")]
    [Required]
    [SerializeField]
    private PlayerSaveGame saveGame;

    [BoxGroup("Character Creator")]
    [Required]
    [SerializeField]
    private SimpleSignal presetSignal;

    [BoxGroup("Character Creator")]
    [Required]
    [SerializeField]
    private List<CharacterCreatorPartProperty> properties = new List<CharacterCreatorPartProperty>();

    public override void Start()
    {
        base.Start();
        GameUtil.setPreset(this.saveGame.playerPreset, this.creatorPreset);
        updateGear();
        updatePreview();
    }

    public void Confirm()
    {
        GameUtil.setPreset(this.creatorPreset, this.saveGame.playerPreset); //save Preset 
        updatePreview();
        base.ExitMenu();
    }

    public void updatePreview()
    {
        this.presetSignal.Raise();
    }

    public void updateGear()
    {
        List<CharacterCreatorGear> gearButtons = new List<CharacterCreatorGear>();
        UnityUtil.GetChildObjects<CharacterCreatorGear>(this.transform, gearButtons);

        foreach (CharacterCreatorPartProperty part in this.properties)
        {
            //enableGearButton(gearButtons, part);
            CharacterPartData data = this.creatorPreset.GetCharacterPartData(part.parentName, part.partName);
            bool enableIt = part.enableIt(this.creatorPreset.getRace(), data);

            if (enableIt) this.creatorPreset.AddCharacterPartData(part.parentName, part.partName);
            else this.creatorPreset.RemoveCharacterPartData(part.parentName, part.partName);
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
