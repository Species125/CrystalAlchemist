using Sirenix.OdinInspector;
using UnityEngine;

namespace CrystalAlchemist
{
    public class SkillPositionZModule : SkillModifier
    {
        public enum PositionType
        {
            none,
            ground,
            center,
            custom
        }

        [BoxGroup("Position Z")]
        [SerializeField]
        private PositionType type = PositionType.custom;

        [BoxGroup("Position Z")]
        [Range(-1, 2)]
        [Tooltip("Positions-Höhe")]
        [ShowIf("type", PositionType.custom)]
        [SerializeField]
        private float positionHeight = 0f;

        [BoxGroup("Position Z")]
        [Tooltip("Schattencollider Höhe")]
        [Range(-1, 0)]
        [SerializeField]
        private float colliderHeightOffset = -0.5f;

        public void Initialize()
        {
            switch (this.type)
            {
                case PositionType.ground: this.transform.position = this.skill.sender.GetGroundPosition(); break;
                case PositionType.center: this.transform.position = this.skill.sender.GetShootingPosition(); break;
                case PositionType.custom: this.transform.position = new Vector2(this.transform.position.x, this.transform.position.y + positionHeight); break;
            }
        }
    }
}
