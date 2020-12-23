using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCreatorButtonHandler : MonoBehaviour
{
    [Required]
    public CharacterCreatorMenu mainMenu;

    [HideInInspector]
    public List<CharacterCreatorButton> buttons = new List<CharacterCreatorButton>();

    private void Start()
    {
        SetSelection();
    }

    public void UpdatePreview()
    {
        this.mainMenu.updatePreview();
    }

    public void SetSelection()
    {
        foreach(CharacterCreatorButton button in this.buttons) button.SetSelection();        
    }
}
