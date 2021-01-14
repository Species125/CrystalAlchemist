using UnityEngine;

namespace CrystalAlchemist
{
    [RequireComponent(typeof(UnityEngine.Camera))]
    public class CameraMinimap : MonoBehaviour
    {
        private UnityEngine.Camera cam;
        public Shader shader;

        private void Start()
        {
            this.cam = this.GetComponent<UnityEngine.Camera>();
            this.cam.SetReplacementShader(shader, "");
        }
    }
}
