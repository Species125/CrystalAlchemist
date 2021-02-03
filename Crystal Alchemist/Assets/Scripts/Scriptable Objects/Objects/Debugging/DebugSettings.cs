using UnityEngine;

namespace CrystalAlchemist
{
    [CreateAssetMenu(menuName = "Game/Settings/Debug Values")]
    public class DebugSettings : ScriptableObject
    {
        public bool activateLight = true;
        public bool showTargetPosition = true;
        public bool showDebbuging = true;
    }
}
