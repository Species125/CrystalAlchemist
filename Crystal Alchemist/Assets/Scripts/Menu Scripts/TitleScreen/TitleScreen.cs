
using UnityEngine;

namespace CrystalAlchemist
{
    public class TitleScreen : MonoBehaviour
    {
        [SerializeField]
        private GameObject mainFrame;
        [SerializeField]
        private GameObject darkFrame;

        private void Start()
        {
            this.mainFrame.SetActive(true);
            if (this.darkFrame != null) this.darkFrame.SetActive(false);

            Cursor.visible = true;
        }

        public virtual void Update()
        {
        }

        public void exitGame()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif

            Application.Quit();
        }

        public void showBackground(bool dark)
        {
            this.mainFrame.SetActive(false);
            if (this.darkFrame != null) this.darkFrame.SetActive(false);

            if (dark && this.darkFrame != null) this.darkFrame.SetActive(true);
            else if (!dark) this.mainFrame.SetActive(true);
        }

        public void SaveSettings()
        {
            SaveSystem.SaveOptions();
        }
    }
}
