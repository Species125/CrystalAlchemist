using Photon.Pun;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace CrystalAlchemist
{
    public class Switch : Interactable
    {
        [DetailedInfoBox("A switch", "This requires a photon view!", InfoMessageType.Info)]
        [SerializeField]
        private UnityEvent OnClick;

        public override void DoOnSubmit()
        {
            this.photonView.RPC("RpcActivateSwitch", RpcTarget.All);
        }

        [PunRPC]
        protected void RpcActivateSwitch()
        {
            OnClick?.Invoke();
        }
    }
}
