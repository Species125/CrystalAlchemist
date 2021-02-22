using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

namespace CrystalAlchemist
{
    public class MenuBehaviour : MonoBehaviourPunCallbacks
    {
        [BoxGroup("Menu")]
        [SerializeField]
        private bool changeVolume;

        [Required]
        [BoxGroup("Menu")]
        public CharacterValues playerValues;

        [BoxGroup("Menu")]
        [SerializeField]
        private bool showBlackBackground = true;

        [BoxGroup("Menu")]
        public InfoBox infoBox;

        public virtual void Start()
        {
            MenuEvents.current.OnCloseMenu += ExitMenu;
            GameEvents.current.DoMenuOpen();

            if (MasterManager.globalValues.openedMenues.Count == 0)
            {
                MasterManager.globalValues.lastState = this.playerValues.currentState;
                GameEvents.current.DoChangeState(CharacterState.inMenu);

                if (this.showBlackBackground) GameEvents.current.DoMenuOverlay(true);

                if (this.changeVolume) MusicEvents.current.ChangeVolume(MasterManager.settings.GetMenuVolume());
            }

            MasterManager.globalValues.openedMenues.Add(this.gameObject);
        }

        public virtual void Update()
        {
        }

        public virtual void OnDestroy()
        {
            MenuEvents.current.OnCloseMenu -= ExitMenu;
            MasterManager.globalValues.openedMenues.Remove(this.gameObject);

            if (MasterManager.globalValues.openedMenues.Count <= 0)
            {
                GameEvents.current.DoChangeState(MasterManager.globalValues.lastState);
                GameEvents.current.DoMenuOverlay(false);

                if (this.changeVolume) MusicEvents.current.ChangeVolume(MasterManager.settings.backgroundMusicVolume);
            }
        }

        public virtual void Cancel()
        {
            if (this.infoBox != null && this.infoBox.gameObject.activeInHierarchy) this.infoBox.Hide();
            else ExitMenu();
        }

        public virtual void ExitMenu()
        {            
            SceneManager.UnloadSceneAsync(this.gameObject.scene);
        }
    }
}
