

using Sirenix.OdinInspector;
using UnityEngine;

namespace CrystalAlchemist
{
    public class SkillBoomerang : SkillProjectile
    {
        #region Attributes
        [Tooltip("Zeitpunkt der Scriptaktivierung")]
        [MinValue(0)]
        public float timeToMoveBack = 0;

        [HideInInspector]
        public float durationThenBackToSender = 0;

        [SerializeField]
        private float minDistance = 0.1f;

        private bool speedup = true;
        //private Vector2 tempVelocity;
        #endregion


        public override void Initialize()
        {
            base.Initialize();
            this.durationThenBackToSender = timeToMoveBack;
        }

        public override void Updating()
        {
            if (this.durationThenBackToSender > 0)
            {
                this.durationThenBackToSender -= (Time.deltaTime * this.skill.getTimeDistortion());
            }
            else
            {
                moveBackToSender();
            }
        }
        private void OnTriggerEnter2D(Collider2D hittedCharacter)
        {
            checkHit(hittedCharacter);
        }

        private void OnTriggerStay2D(Collider2D hittedCharacter)
        {
            //got Hit -> Back to Target
            checkHit(hittedCharacter);
        }

        public void checkHit(Collider2D hittedCharacter)
        {
            if (this.skill.sender != null
                && (
                    (!hittedCharacter.isTrigger
                     && hittedCharacter.GetComponent<Breakable>() == null
                     && hittedCharacter.GetComponent<Character>() != this.skill.sender)
                    || (hittedCharacter.GetComponent<Collectable>() != null)) //item stop
            )
            {
                if (this.GetComponent<SkillBoomerang>() != null) this.GetComponent<SkillBoomerang>().durationThenBackToSender = 0;
            }
        }

        #region Functions (private)
        private void moveBackToSender()
        {
            if (this.skill.sender != null)
            {
                //Bewege den Skill zurück zum Sender
                if (Vector3.Distance(this.skill.sender.GetShootingPosition(), this.transform.position) > this.minDistance)
                {
                    this.skill.SetDirection((this.skill.sender.GetShootingPosition() - (Vector2)this.transform.position));
                    this.setVelocity();
                }
                else
                {
                    this.skill.DeactivateIt();
                }
            }
        }
        #endregion
    }
}
