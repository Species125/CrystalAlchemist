



using Sirenix.OdinInspector;
using UnityEngine;

namespace CrystalAlchemist
{
    public class CharacterCreatorMenu : MenuBehaviour
    {
        [BoxGroup("Character Creator")]
        [Required]
        public CharacterPreset playerPreset;

        private CharacterPreset backup;


        public override void Start()
        {
            base.Start();

            this.backup = ScriptableObject.CreateInstance<CharacterPreset>();
            GameUtil.setPreset(this.playerPreset, this.backup);
        }

        public void Abort()
        {
            Undo();
            base.ExitMenu();
        }

        public void Undo()
        {
            GameUtil.setPreset(this.backup, this.playerPreset);
            UpdatePreview();
        }

        public void UpdatePreview() => GameEvents.current.DoPresetChange();
    }
}
