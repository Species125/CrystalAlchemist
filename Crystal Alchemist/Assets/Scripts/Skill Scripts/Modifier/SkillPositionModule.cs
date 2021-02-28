using Sirenix.OdinInspector;
using UnityEngine;

namespace CrystalAlchemist
{
    public class SkillPositionModule : SkillModifier
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
        [SerializeField]
        private Collider2D skillCollider;

        [BoxGroup("Position Offset")]
        [SerializeField]
        private float positionOffset = 0f;

        public void Initialize()
        {
            switch (this.type)
            {
                case PositionType.ground: this.transform.position = this.skill.sender.GetGroundPosition(); break;
                case PositionType.center: this.transform.position = this.skill.sender.GetShootingPosition(); break;
                case PositionType.custom: this.transform.position = new Vector2(this.transform.position.x, this.transform.position.y + positionHeight); break;
            }            
        }

        public void LateInitialize()
        {
            if (this.skillCollider != null) this.skillCollider.transform.position = this.skill.sender.GetGroundPosition();

            Vector3 direction = (Vector3)this.skill.GetDirection();
            this.transform.position += (direction * positionOffset);            
        }        
    }
}
