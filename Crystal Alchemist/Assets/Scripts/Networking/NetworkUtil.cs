using Photon.Pun;
using UnityEngine;

namespace CrystalAlchemist
{
    public class NetworkUtil : MonoBehaviour
    {
        public static bool IsLocal()
        {
            return PhotonNetwork.LocalPlayer.IsLocal;
        }

        public static bool IsLocal(PhotonView view)
        {
            return (view.IsMine || !PhotonNetwork.IsConnected);
        }

        public static bool IsLocal(Player player)
        {
            return (player != null && player.isLocalPlayer);
        }

        public static bool IsMaster()
        {
            return PhotonNetwork.IsMasterClient;
        }

        public static Player GetLocalPlayer()
        {
            return (Player)PhotonNetwork.LocalPlayer.TagObject;
        }
    }
}
