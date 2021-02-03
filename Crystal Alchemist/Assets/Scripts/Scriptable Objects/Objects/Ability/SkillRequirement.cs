
using Sirenix.OdinInspector;
using UnityEngine;

namespace CrystalAlchemist
{
    [CreateAssetMenu(menuName = "Game/Ability/Skill Requirement")]
    public class SkillRequirement : ScriptableObject
    {
        public enum Type
        {
            none,
            teleport
        }

        [BoxGroup]
        [SerializeField]
        private Type type;

        [BoxGroup]
        [ShowIf("type", Type.teleport)]
        [SerializeField]
        private PlayerTeleportList playerTeleport;

        public bool Granted()
        {
            if (this.type == Type.teleport && this.playerTeleport.HasReturn()) return true;
            return false;
        }
    }
}
