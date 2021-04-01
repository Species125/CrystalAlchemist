using Photon.Pun;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CrystalAlchemist
{    
    public class NetworkBehaviour : MonoBehaviourPunCallbacks
    {
        [BoxGroup("Inspector")]
        [ReadOnly]
        public string path;

        [BoxGroup("Inspector")]
        [ReadOnly]
        public string stringID;

        private void Awake()
        {
            this.stringID = this.gameObject.name+" ["
                +this.transform.GetSiblingIndex()+"] "
                +this.gameObject.transform.position.x+":"+this.gameObject.transform.position.y;
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            this.path = UnityUtil.GetResourcePath(this);
        }
#endif


    }
}
