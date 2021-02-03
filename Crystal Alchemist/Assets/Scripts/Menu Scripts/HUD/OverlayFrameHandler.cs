using UnityEngine;

namespace CrystalAlchemist
{
    public class OverlayFrameHandler : MonoBehaviour
    {
        [SerializeField]
        private GameObject blackScreen;

        private void Start() => GameEvents.current.OnMenuOverlay += BlackScreenActive;

        private void OnDestroy() => GameEvents.current.OnMenuOverlay -= BlackScreenActive;

        private void BlackScreenActive(bool value)
        {
            if (this.blackScreen != null) this.blackScreen.SetActive(value);
        }
    }
}
