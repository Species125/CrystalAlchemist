using UnityEngine;

namespace CrystalAlchemist
{
    public class MiniMapMarkerExtension : MonoBehaviour
    {
        public GameObject marker;
        private GameObject activeMarker;

        public virtual void Start()
        {
            if (this.marker != null) activeMarker = Instantiate(this.marker, this.transform);
        }

        public GameObject GetMarker()
        {
            return this.activeMarker;
        }
    }
}
