using System.Collections;
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

    public void Start()
    {
        this.RaceSubMenu();
    }

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
        foreach(CharacterCreatorSubMenuChild child in this.subMenu)
        {
            child.gameObject.SetActive(false);

            if (child.isEnabledByRace(this.mainMenu.creatorPreset.getRace())) child.gameObject.SetActive(true);
        }
    }
}
