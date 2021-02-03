

using Sirenix.OdinInspector;
using UnityEngine;

namespace CrystalAlchemist
{
    public class CharacterCreatorPreset : CharacterCreatorButton
    {
        [Required]
        [SerializeField]
        private CharacterCreatorMenu mainMenu;

        [SerializeField]
        private CharacterPreset preset;

        public override void Click()
        {
            GameUtil.SetPreset(this.preset, this.mainMenu.playerPreset);
            this.mainMenu.UpdatePreview();
            base.Click();
        }
    }
}
