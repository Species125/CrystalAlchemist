using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CrystalAlchemist
{
    public class NetworkUtil : MonoBehaviour
    {
        //Photon-Views on Mechanics and Characters
        //Network-Behaviour on all instantiations (path)

        public const byte PLAYER_JOINED = 2;
        public const byte SCENE_CHANGE = 5;        

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

        public const byte SET_NEXT_TELEPORT = 41;

        public const byte READY_SHOW = 42;
        public const byte READY_SET = 43;
        public const byte READY_CANCEL = 44;

        public const byte JUKEBOX_PLAY = 45;
        public const byte JUKEBOX_PAUSE = 46;
        public const byte JUKEBOX_STOP = 47;

        public const byte TIME_STARTUP = 51;
        public const byte TIME_CHANGED = 52;
        public const byte TIME_RESET = 53;

        public const byte PLAYERS_DEATH = 66;

        public const byte SKILL_AFFECTIONS = 81;

        public const byte DISCONNECT = 91;


        public static RaiseEventOptions TargetAll()
        {
            return new RaiseEventOptions()
            {
                Receivers = ReceiverGroup.All
            };
        }

        public static RaiseEventOptions TargetMaster()
        {
            return new RaiseEventOptions()
            {
                Receivers = ReceiverGroup.MasterClient
            };
        }

        public static RaiseEventOptions TargetOther()
        {
            return new RaiseEventOptions()
            {
                Receivers = ReceiverGroup.Others
            };
        }
        public static RaiseEventOptions TargetOtherBuffered()
        {
            return new RaiseEventOptions()
            {
                Receivers = ReceiverGroup.Others,
                CachingOption = EventCaching.AddToRoomCache
            };
        }

        public static bool IsSolo()
        {
            return (PhotonNetwork.OfflineMode || PhotonNetwork.PlayerList.Length <= 1);
        }

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
            if(character && character.photonView) return character.photonView.ViewID;
            return -1;
        }

        public static int GetID(GameObject gameObject)
        {
            return gameObject.GetPhotonView().ViewID; //ERROR
        }

        public static Character GetCharacter(int ID)
        {
            if (ID <= 0) return null;
            PhotonView view = PhotonView.Find(ID);
            if (!view) return null;

            return view.GetComponent<Character>();
        }

        public static Player GetPlayer(int ID)
        {
            if (ID <= 0) return null;
            PhotonView view = PhotonView.Find(ID);
            if (!view) return null;

            return view.GetComponent<Player>();
        }

        public static GameObject GetGameObject(int ID)
        {
            PhotonView view = PhotonView.Find(ID);
            if (!view) return null;

            return view.gameObject;
        }

        public static void SetRoomStatus(bool value)
        {
            if (IsMaster() && PhotonNetwork.InRoom)
            {
                PhotonNetwork.CurrentRoom.IsVisible = value;
                PhotonNetwork.CurrentRoom.IsOpen = value;
            }
        }

        public static void LoadLevel(string scene)
        {
            if (IsMaster())
            {
                //PhotonNetwork.LoadLevel(scene);
                NetworkEvents.current.RaiseSceneChangeEvent(scene);
            }
        }

        public static void LoadConnect() => SceneManager.LoadScene("Loading");        

        public static void CreateRoom(string roomName, byte maxPlayers = 1, bool isOpen = false, bool privat = false, string password = "")
        {
            if (!PhotonNetwork.IsConnected) return;

            RoomOptions options = new RoomOptions();
            options.PublishUserId = true;
            options.MaxPlayers = maxPlayers;
            options.IsVisible = true;
            options.IsOpen = isOpen;
            options.CustomRoomPropertiesForLobby = new string[] { "Private","Password" };
            options.CustomRoomProperties = new ExitGames.Client.Photon.Hashtable { { "Private", privat }, {"Password", password } };

            PhotonNetwork.CreateRoom(roomName, options, TypedLobby.Default);
        }
    }
}
