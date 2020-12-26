using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCreatorButtonHandler : MonoBehaviour
{
    [Required]
    public CharacterCreatorMenu mainMenu;

    [HideInInspector]
    public List<CharacterCreatorButton> buttons = new List<CharacterCreatorButton>();

    [SerializeField]
    private bool setFirst = false;

    private void Start()
    {
        SetSelection();
    }

    public void UpdatePreview()
    {
        this.mainMenu.updatePreview();
        SetSelection();
    }

    public void SetSelection()
    {
        foreach(CharacterCreatorButton button in this.buttons) button.SetSelection();        
    }

    public void SetFirst(CharacterCreatorButton button, int index)
    {
        if (index == 0 && setFirst)
        {
            button.GetComponent<ButtonExtension>().SetAsFirst();
            button.GetComponent<ButtonExtension>().ReSelect();
        }
    }
}
