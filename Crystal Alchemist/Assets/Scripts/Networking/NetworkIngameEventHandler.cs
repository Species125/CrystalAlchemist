using Photon.Pun;
using UnityEngine;
using UnityEngine.Events;

namespace CrystalAlchemist
{
    public class NetworkIngameEventHandler : NetworkBehaviour
    {
        [SerializeField]
        private UnityEvent OnClick;

        public void DoEvents()
        {
            this.photonView.RPC("RpcDoNetworkEvents", RpcTarget.All);
        }

        [PunRPC]
        protected void RpcDoNetworkEvents()
        {
            OnClick?.Invoke();
        }
    }
}
