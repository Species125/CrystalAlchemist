using System.Diagnostics;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CrystalAlchemist
{
    public class DebugText : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI textfield;

        [SerializeField]
        private Selectable button;

        [SerializeField]
        private DebugLog debug;

        private string path;

        private void OnEnable()
        {
            this.path = debug.path;
            this.button.interactable = File.Exists(this.path);

            this.textfield.text = debug.lastError;            
        }

        public void OpenLog() => Process.Start(path);        
    }
}
