using TMPro;
using UnityEngine;

namespace CrystalAlchemist
{
    public class GetVersion : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI versionText;

        void Start()
        {
            this.versionText.text = "Version: " + Application.version + " (Pre-Alpha)";
        }

    }
}
