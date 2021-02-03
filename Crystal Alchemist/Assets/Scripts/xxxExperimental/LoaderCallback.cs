using UnityEngine;

namespace CrystalAlchemist
{
    public class LoaderCallback : MonoBehaviour
    {
        private bool isFirstUpdate = true;

        private void Update()
        {
            if (this.isFirstUpdate)
            {
                this.isFirstUpdate = false;
                SceneLoader.LoaderCallback();
            }
        }
    }
}
