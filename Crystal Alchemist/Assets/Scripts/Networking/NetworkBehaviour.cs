using Photon.Pun;
using Sirenix.OdinInspector;

namespace CrystalAlchemist
{    
    public class NetworkBehaviour : MonoBehaviourPunCallbacks
    {
        [BoxGroup("Inspector")]
        [ReadOnly]
        public string path;

#if UNITY_EDITOR
        private void OnValidate()
        {
            this.path = UnityUtil.GetResourcePath(this);
        }
#endif
    }
}
