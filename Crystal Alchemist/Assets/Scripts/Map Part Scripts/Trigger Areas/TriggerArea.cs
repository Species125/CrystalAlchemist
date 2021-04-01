using UnityEngine;
using Photon.Pun;
using UnityEngine.Events;

namespace CrystalAlchemist
{
    public class TriggerArea : NetworkBehaviour
    {
        [SerializeField]
        private UnityEvent OnEnterEvent;

        [SerializeField]
        private UnityEvent OnExitEvent;

        private void OnTriggerEnter2D (Collider2D collision)
        {
            this.photonView.RPC("RpcTriggerAreaEnter2D", RpcTarget.All);
        }

        [PunRPC]
        protected void RpcTriggerAreaEnter2D()
        {
            OnEnterEvent?.Invoke();
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            this.photonView.RPC("RpcTriggerAreaExit2D", RpcTarget.All);
        }

        [PunRPC]
        protected void RpcTriggerAreaExit2D()
        {
            OnExitEvent?.Invoke();
        }
    }
}
