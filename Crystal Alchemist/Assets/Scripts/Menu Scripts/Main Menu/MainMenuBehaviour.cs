using UnityEngine;
using UnityEngine.SceneManagement;
using Sirenix.OdinInspector;

namespace CrystalAlchemist
{
    public class MainMenuBehaviour : MonoBehaviour
    {
        [Required]
        [SerializeField]
        private StringValue lastMenu;

        [SerializeField]
        private float inputDelay = 0.3f;

        private bool inputPossible = false;

        private void Start()
        {
            Cursor.visible = true;
            StartInputDelay();
        }

        private void StartInputDelay()
        {
            this.inputPossible = false;
            Invoke("StopInputDelay", this.inputDelay);
        }

        private void StopInputDelay()
        {
            this.inputPossible = true;
        }

        public void SaveSettings()
        {
            SaveSystem.SaveOptions();
        }

        public void OpenMenu(string name)
        {
            if (!this.inputPossible) return;
            
            SceneManager.LoadSceneAsync(name, LoadSceneMode.Additive);
            this.inputPossible = false;

            if (this.lastMenu == null) return;
            this.lastMenu.SetValue(this.gameObject.scene.name);
            SceneManager.UnloadSceneAsync(this.gameObject.scene.name);
                        
            //AsyncOperation operation = new AsyncOperation();
            //operation.completed += OperationOnCompleted;
        }

        private void OperationOnCompleted(AsyncOperation obj)
        {
            obj.allowSceneActivation = true;
        }

        public void Return()
        {
            if (!this.inputPossible) return;
            if (this.lastMenu == null) return;
            OpenMenu(this.lastMenu.GetValue());
        }

        public void ExitGame()
        {
            if (!this.inputPossible) return;

#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif

            Application.Quit();
        }
    }
}
