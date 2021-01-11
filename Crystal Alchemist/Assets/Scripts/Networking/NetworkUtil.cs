using Photon.Pun;
using System;
using UnityEngine;

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

    public static bool IsMaster()
    {
        return PhotonNetwork.IsMasterClient;
    }
}
