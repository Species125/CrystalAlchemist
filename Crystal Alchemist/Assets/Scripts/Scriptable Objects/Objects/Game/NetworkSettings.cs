using UnityEngine;

namespace CrystalAlchemist
{
    [CreateAssetMenu(menuName = "Game/Settings/Network Settings")]
    public class NetworkSettings : ScriptableObject
    {
        public bool offlineMode = false;
        public int sendRate = 10;
        public int serializationRate = 10;
        public string nickname = "Gungnir";
        public int uniqueID = 4711;
        public string version = "0.0.1";
        public byte maxPlayers = 4;
        public string roomName = "Online";
        public Player playerPrefab;
    }
}