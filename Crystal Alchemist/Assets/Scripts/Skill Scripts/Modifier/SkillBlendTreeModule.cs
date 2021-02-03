using UnityEngine;

namespace CrystalAlchemist
{
    public class SkillBlendTreeModule : SkillModifier
    {
        [Tooltip("Welche Winkel sollen fest gestellt werden. 0 = frei. 45 = 45° Winkel")]
        [Range(0, 90)]
        [SerializeField]
        private float snapRotationInDegrees = 0f;

        public void Initialize()
        {
            //Vector2Int full = Vector2Int.RoundToInt(this.skill.GetDirection());

            float angle = RotationUtil.SetAngle(this.skill.GetDirection(), snapRotationInDegrees);
            this.skill.SetDirection(RotationUtil.DegreeToVector2(angle));

            AnimatorUtil.SetAnimDirection(this.skill.GetDirection(), this.skill.animator);
        }
    }
}
