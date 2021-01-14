
using Sirenix.OdinInspector;
using UnityEngine;

namespace CrystalAlchemist
{
    public class SkillCameraShake : MonoBehaviour
    {
        [SerializeField]
        [MinValue(0.001f)]
        private float strength = 1f;

        [SerializeField]
        [MinValue(0.001f)]
        private float duration = 1f;

        [SerializeField]
        [MinValue(0.001f)]
        private float speed = 1f;

        public void ShakeCamera() => GameEvents.current.DoCameraShake(this.strength, this.duration, this.speed);

        public void StabilizeCamera() => GameEvents.current.DoCameraStill(this.speed);

        public void StabilizeCamera(float speed) => GameEvents.current.DoCameraStill(speed);
    }
}
