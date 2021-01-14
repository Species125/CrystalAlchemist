using UnityEngine;

namespace CrystalAlchemist
{
    public class BlindUIManager : MonoBehaviour
    {
        public Canvas canvas;
        public Camera cam;

        void Start()
        {
            cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
            canvas.worldCamera = cam;
        }

    }
}
