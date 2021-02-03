using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace CrystalAlchemist
{
    public class CharacterCreatorSubMenu : MonoBehaviour
    {
        [Required]
        public CharacterCreatorMenu mainMenu;

        [SerializeField]
        private List<CharacterCreatorSubMenuChild> subMenu = new List<CharacterCreatorSubMenuChild>();

        public void OnEnable() => this.RaceSubMenu();

        public void OnDisable() => this.RaceSubMenu();

        public void RaceSubMenu()
        {
            foreach (CharacterCreatorSubMenuChild child in this.subMenu)
            {
                child.gameObject.SetActive(false);

                if (child.isEnabledByRace(this.mainMenu.playerPreset.getRace())) child.gameObject.SetActive(true);
            }
        }
    }
}
