
using UnityEngine;

namespace CrystalAlchemist
{
    [CreateAssetMenu(menuName = "Game/Menu/SavePointInfo")]
    public class SavePointInfo : ScriptableObject
    {
        public TeleportStats stats;
    }
}
