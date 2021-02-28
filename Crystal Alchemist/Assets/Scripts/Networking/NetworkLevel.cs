using Photon.Pun;
using UnityEngine;

namespace CrystalAlchemist
{
    public class NetworkLevel : MonoBehaviour
    {
        [SerializeField]
        private bool canJoin = false;

        private void Awake()
        {
            CanJoinGroup(this.canJoin);
        }

        public void CanJoinGroup(bool value)
        {
            if (NetworkUtil.IsMaster()) PhotonNetwork.CurrentRoom.IsOpen = value;
        }
    }
}
