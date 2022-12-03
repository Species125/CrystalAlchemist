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

        private float inputDelay = 0.3f;

        [HideInInspector]
        public bool inputPossible = false;

        public virtual void Start()
        {
            MenuEvents.current.OnCloseMenu += ExitMenu;
            GameEvents.current.DoMenuOpen();

            StartInputDelay();

            if (MasterManager.globalValues.openedMenues.Count == 0)
            {
                MasterManager.globalValues.lastState = this.playerValues.currentState;
                if (!this.playerValues.CanOpenMenu()) MasterManager.globalValues.lastState = CharacterState.idle;

                GameEvents.current.DoChangeState(CharacterState.inMenu);

                if (this.showBlackBackground) GameEvents.current.DoMenuOverlay(true);

                if (this.changeVolume) MusicEvents.current.ChangeVolume(MasterManager.settings.GetMenuVolume());
            }

            MasterManager.globalValues.openedMenues.Add(this.gameObject);
        }

        public virtual void StartInputDelay()
        {
            this.inputPossible = false;
            Invoke("StopInputDelay", this.inputDelay);
        }

        public virtual void StopInputDelay()
        {
            this.inputPossible = true;
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
                GameEvents.current.DoMenuClose();

                if (this.changeVolume) MusicEvents.current.ChangeVolume(MasterManager.settings.backgroundMusicVolume);
            }
        }

        public virtual void Cancel()
        {
            if (!this.inputPossible) return;
            if (this.infoBox != null && this.infoBox.gameObject.activeInHierarchy) this.infoBox.Hide();
            else ExitMenu();
        }

        public virtual void ExitMenu()
        {
            if (!this.inputPossible) return;
            SceneManager.UnloadSceneAsync(this.gameObject.scene);
        }
    }
}
