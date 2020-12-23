using UnityEngine;
using Sirenix.OdinInspector;

public class CharacterCreatorPreset : CharacterCreatorButton
{
    [Required]
    [SerializeField]
    private CharacterCreatorMenu mainMenu;

    [SerializeField]
    private CharacterPreset preset;

    public override void Click()
    {
        GameUtil.setPreset(this.preset, this.mainMenu.creatorPreset);
        this.mainMenu.updateGear();
        this.mainMenu.updatePreview();
        base.Click();
    }
}
