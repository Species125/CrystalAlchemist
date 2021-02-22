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
            GameUtil.SetPreset(this.playerPreset, this.backup);
        }

        public void Abort()
        {
            Undo();
            base.ExitMenu();
        }

        public void Undo()
        {
            GameUtil.SetPreset(this.backup, this.playerPreset);
            UpdatePreview();
        }

        public override void ExitMenu()
        {
            GameEvents.current.DoSaveGame(false);
            base.ExitMenu();
        }

        public void UpdatePreview() => GameEvents.current.DoPresetChange();
    }
}
