using System.Collections;
using UnityEngine;

namespace CrystalAlchemist
{
    [RequireComponent(typeof(UnityEngine.Camera))]
    public class CameraStartUp : MonoBehaviour
    {
        private float delay = 0.2f;
        private UnityEngine.Camera cam;

        private void Awake()
        {
            this.cam = this.GetComponent<UnityEngine.Camera>();
            this.cam.enabled = false;
            StartCoroutine(delayCo());
        }

        private IEnumerator delayCo()
        {
            yield return new WaitForSeconds(this.delay);
            this.cam.enabled = true;
        }
    }
}
