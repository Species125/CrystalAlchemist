using UnityEngine;

namespace CrystalAlchemist
{
    public class SkillColliderOffset : SkillCollider
    {
        public enum Mode
        {
            none,
            center,
            ground
        }

        [SerializeField]
        private Mode mode = Mode.center;

        private void Start() => this.transform.position = GetPosition();

        public override Vector2 GetPosition()
        {
            if (this.mode == Mode.center)
            {
                float offsetX = ((this.skill.sender.GetGroundPosition().x - this.transform.position.x) / 2);
                float offsetY = ((this.skill.sender.GetGroundPosition().y - this.transform.position.y) / 2);

                float x = this.skill.sender.GetGroundPosition().x - offsetX;
                float y = this.skill.sender.GetGroundPosition().y - offsetY;

                return new Vector2(x, y);
            }
            else if (this.mode == Mode.ground) return this.skill.sender.GetGroundPosition();
            return this.transform.position;
        }
    }
}
