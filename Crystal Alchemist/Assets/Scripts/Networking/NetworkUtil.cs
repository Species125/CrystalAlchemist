using Photon.Pun;
using UnityEngine;

namespace CrystalAlchemist
{
    public class NetworkUtil : MonoBehaviour
    {
        //Photon-Views on Mechanics and Characters
        //Network-Behaviour on all instantiations (path)

        public const byte AGGRO_ON_HIT = 10;
        public const byte AGGRO_INCREASE = 11;
        public const byte AGGRO_DECREASE = 12;

        public const byte HIDE_CHARACTER = 15;
        public const byte SHOW_CHARACTER = 16;

        public const byte SKILL = 20;
        public const byte BOSSSKILL = 21;

        public const byte MECHANIC_CHARACTER = 23;
        public const byte MECHANIC_AI = 24;
        public const byte MECHANIC_ADDSPAWN = 25;
        public const byte MECHANIC_SKILL = 26;
        public const byte MECHANIC_ABILITY = 27;
        public const byte MECHANIC_OBJECT = 28;

        public const byte ITEMDROP = 30;
        public const byte ITEMDROP_MASTER = 31;

        public const byte READY_SHOW = 41;
        public const byte READY_SET = 42;

        public const byte SKILL_AFFECTIONS = 101;

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

        public static int GetID(Character character)
        {
            return character.photonView.ViewID;
        }

        public static int GetID(GameObject gameObject)
        {
            return gameObject.GetPhotonView().ViewID;
        }

        public static Character GetCharacter(int ID)
        {
            PhotonView view = PhotonView.Find(ID);
            if (!view) return null;

            return view.GetComponent<Character>();
        }

        public static GameObject GetGameObject(int ID)
        {
            PhotonView view = PhotonView.Find(ID);
            if (!view) return null;

            return view.gameObject;
        }
    }
}
