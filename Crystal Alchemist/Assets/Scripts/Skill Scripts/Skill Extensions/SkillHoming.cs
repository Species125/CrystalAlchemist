
using Sirenix.OdinInspector;
using UnityEngine;

namespace CrystalAlchemist
{
    public class SkillHoming : SkillProjectile
    {
        [BoxGroup("Homing-spezifische Attribute")]
        public float offSetTime;

        [BoxGroup("Homing-spezifische Attribute")]
        public float offSetStrength;

        private void Update()
        {
            moveToTarget();
        }

        private void moveToTarget()
        {
            if (this.skill.target != null)
            {
                this.stopVelocity();
                if (Vector3.Distance(this.skill.target.transform.position, this.transform.position) > 0.25f)
                {
                    //Ermittle Position des Ziels
                    Vector2 targetPosition = this.skill.target.GetGroundPosition();

                    //offSetTime und offSetStrength lassen den Skill nicht direkt, sondern in einer Kurve fliegen
                    if (this.offSetTime >= 0)
                    {
                        this.offSetTime -= Time.deltaTime;

                        if (Mathf.Abs(this.skill.target.transform.position.x) > Mathf.Abs(this.skill.target.transform.position.y))
                        {
                            targetPosition = new Vector2(targetPosition.x, targetPosition.y + offSetStrength);
                        }
                        else
                        {
                            targetPosition = new Vector2(targetPosition.x + offSetStrength, targetPosition.y);
                        }

                        this.offSetStrength -= (this.offSetStrength / this.offSetTime);
                    }

                    this.skill.SetDirection(targetPosition - (Vector2)this.transform.position);
                    this.setVelocity();
                }
                else
                {
                    //Starte End-Animation wenn der Skill sein Ziel erreicht hat
                    AnimatorUtil.SetAnimatorParameter(this.skill.animator, "Explode", true);
                }
            }
            else
            {
                //Zerstöre Skill, wenn das Ziel nicht mehr vorhanden ist. 
                //TODO: Weiter fliegen lassen?
                this.skill.DeactivateIt();
            }
        }
    }
}
