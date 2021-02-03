using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

namespace CrystalAlchemist
{
    public class CameraDistancePixelPerfect : MonoBehaviour
    {
        private PixelPerfectCamera cam;

        private void Start()
        {
            SettingsEvents.current.OnCameraChanged += ChangeSize;
            this.cam = this.GetComponent<PixelPerfectCamera>();
            ChangeSize();
        }

        private void ChangeSize()
        {
            int value = MasterManager.settings.cameraDistance - 1;
            this.cam.refResolutionY = 180 + (value * 20);
        }

        private void OnDestroy()
        {
            SettingsEvents.current.OnCameraChanged -= ChangeSize;
        }
    }
}
