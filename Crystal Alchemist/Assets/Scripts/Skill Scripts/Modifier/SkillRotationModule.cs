
using Sirenix.OdinInspector;
using UnityEngine;

namespace CrystalAlchemist
{
    public class SkillRotationModule : SkillModifier
    {
        public enum RotationMode
        {
            normal,
            identity,
            snap
        }

        [BoxGroup("Rotations")]
        [Tooltip("Soll der Projektilsprite passend zur Flugbahn rotieren?")]
        [SerializeField]
        private RotationMode mode = RotationMode.normal;

        [BoxGroup("Rotations")]
        [Tooltip("Welche Winkel sollen fest gestellt werden. 0 = frei. 45 = 45° Winkel")]
        [Range(0, 90)]
        [SerializeField]
        [ShowIf("mode", RotationMode.snap)]
        private float snapRotationInDegrees = 0f;

        public void Initialize()
        {
            if (this.mode == RotationMode.identity) this.transform.rotation = Quaternion.identity;
            else if (this.mode == RotationMode.snap)
            {
                float angle = RotationUtil.SetAngle(this.skill.GetDirection(), snapRotationInDegrees);
                this.skill.SetDirection(RotationUtil.DegreeToVector2(angle));
                this.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
            }
            else if (this.mode == RotationMode.normal)
            {
                float angle = RotationUtil.SetAngle(this.skill.GetDirection(), 0);
                this.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
            }
        }


    }
}
