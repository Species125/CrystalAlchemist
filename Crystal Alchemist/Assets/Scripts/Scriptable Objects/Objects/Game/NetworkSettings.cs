using UnityEngine;

namespace CrystalAlchemist
{
    [CreateAssetMenu(menuName = "Game/Settings/Network Settings")]
    public class NetworkSettings : ScriptableObject
    {
        public BoolValue offlineMode;
        public BoolValue gotKicked;

        public int sendRate = 10;
        public int serializationRate = 10;        
        public string nickname = "Gungnir";
        public int uniqueID = 4711;
        public string version = "0.0.1";

        public StringValue currentScene;

        private void OnEnable() => this.offlineMode.SetValue(true);        
    }
}