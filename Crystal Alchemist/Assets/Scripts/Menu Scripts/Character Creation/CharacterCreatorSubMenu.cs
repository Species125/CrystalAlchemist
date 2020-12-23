using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class CharacterCreatorSubMenu : MonoBehaviour
{
    [Required]
    [SerializeField]
    private CharacterCreatorMenu mainMenu;

    [SerializeField]
    private List<CharacterCreatorSubMenuChild> subMenu = new List<CharacterCreatorSubMenuChild>();

    public void OnEnable()
    {
        this.RaceSubMenu();
    }

    public void OnDisable()
    {
        this.RaceSubMenu();
    }

    public void RaceSubMenu()
    {
        if (this.mainMenu.creatorPreset == null) return;

        foreach(CharacterCreatorSubMenuChild child in this.subMenu)
        {
            child.gameObject.SetActive(false);

            if (child.isEnabledByRace(this.mainMenu.creatorPreset.getRace())) child.gameObject.SetActive(true);
        }
    }
}
