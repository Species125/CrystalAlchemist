
using TMPro;
using UnityEngine;

namespace CrystalAlchemist
{
    public class ExceptionUI : MonoBehaviour
    {
        [SerializeField]
        private DebugLog log;

        [SerializeField]
        private DebugSettings settings;

        [SerializeField]
        private TextMeshProUGUI errorText;

        [SerializeField]
        private TextMeshProUGUI errorCount;
    }
}
