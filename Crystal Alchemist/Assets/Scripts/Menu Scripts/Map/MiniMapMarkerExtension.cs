using UnityEngine;

namespace CrystalAlchemist
{
    public class MiniMapMarkerExtension : MonoBehaviour
    {
        [SerializeField]
        private GameObject marker;

        private void Start()
        {
            if (this.marker != null) Instantiate(this.marker, this.transform);
        }
    }
}
