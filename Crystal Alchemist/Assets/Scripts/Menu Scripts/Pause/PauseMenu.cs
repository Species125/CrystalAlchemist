using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CrystalAlchemist
{
    public class PauseMenu : MenuBehaviour
    {
        [BoxGroup("Error Log")]
        [SerializeField]
        private DebugLog logging;

        [BoxGroup("Error Log")]
        [SerializeField]
        private InfoNumber errorCount;

        public override void Start()
        {
            base.Start();
            MenuEvents.current.OnPause += ExitMenu;

            errorCount.gameObject.SetActive(false);

            if (logging.errorCount > 0)
            {
                errorCount.gameObject.SetActive(true);
                errorCount.SetValue(logging.errorCount);
            }
        }


        public override void OnDestroy()
        {
            base.OnDestroy();
            MenuEvents.current.OnPause -= ExitMenu;
        }

        public void ExitGame() => GameEvents.current.DoTitleScreen();        

        public void SaveSettings() => SaveSystem.SaveOptions();
    }
}
