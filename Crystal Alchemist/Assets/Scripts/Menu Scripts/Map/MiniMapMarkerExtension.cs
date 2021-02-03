using UnityEngine;

namespace CrystalAlchemist
{
    public class MiniMapMarkerExtension : MonoBehaviour
    {
        public GameObject marker;

        public virtual void Start()
        {
            if (this.marker != null) Instantiate(this.marker, this.transform);
        }
    }
}
