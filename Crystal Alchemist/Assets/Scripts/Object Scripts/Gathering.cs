using UnityEngine;
using Sirenix.OdinInspector;

namespace CrystalAlchemist
{
    public class Gathering : Treasure
    {
        [BoxGroup("Mandatory")]
        [SerializeField]
        private GameObject glimmer;

        [BoxGroup("Mandatory")]
        [SerializeField]
        private MiniMapMarkerExtension mapMarker;

        public override void SetEnabled(bool enable)
        {
            base.SetEnabled(enable);
            
            if (this.glimmer != null) this.glimmer.SetActive(enable);
            if (this.mapMarker != null) this.mapMarker.GetMarker().SetActive(enable);
        }
    }
}
