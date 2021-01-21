using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CrystalAlchemist
{
    public class NetworkManager : MonoBehaviour
    {
        [SerializeField]
        private NetworkSettings settings;

        private void Start()
        {
            if (!PhotonNetwork.IsConnected)
            {
                GameEvents.current.DoChangeScene(SceneManager.GetActiveScene().name);
                return;
            }
            if (PhotonNetwork.IsConnected && PhotonNetwork.InRoom) Instantiate();
        }        

        private void Instantiate()
        {
            GameObject player = PhotonNetwork.Instantiate(this.settings.playerPrefab.path, Vector2.zero, Quaternion.identity, 0);
            player.GetComponent<Player>().uniqueID = PhotonNetwork.LocalPlayer.UserId;
        }
    }
}
