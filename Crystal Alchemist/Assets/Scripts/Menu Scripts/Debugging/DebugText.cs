using TMPro;
using UnityEngine;

namespace CrystalAlchemist
{
    public class DebugText : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI textfield;

        [SerializeField]
        private DebugLog debug;

        private void OnEnable()
        {
            this.textfield.text = debug.lastError;
        }
    }
}
