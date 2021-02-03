
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace CrystalAlchemist
{
    public class SkillEventHit : SkillHitTrigger
    {
        [InfoBox("Fires Event after a time > 0 or onCollisionEnter > 0 or both")]
        [BoxGroup("Timed Event")]
        [SerializeField]
        private float time;

        [BoxGroup("Timed Event")]
        [SerializeField]
        private UnityEvent OnTime;

        [BoxGroup("Collision Event")]
        [SerializeField]
        private int amount;

        [BoxGroup("Collision Event")]
        [SerializeField]
        private UnityEvent OnCollision;

        private float elapsed;
        private int counter;

        public override void Initialize()
        {
            elapsed = time;
        }

        public override void Updating()
        {
            if (time > 0)
            {
                if (elapsed > 0) elapsed -= (Time.deltaTime * this.skill.getTimeDistortion());
                else
                {
                    OnTime?.Invoke();
                    elapsed = time;
                }
            }

            if (amount > 0)
            {
                if (counter >= amount)
                {
                    OnCollision?.Invoke();
                    counter = 0;
                }
            }
        }


        private void OnTriggerEnter2D(Collider2D hittedCharacter)
        {
            if (CollisionUtil.CheckCollision(hittedCharacter, this.skill)) counter++;
        }
    }
}
