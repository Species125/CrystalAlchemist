

using Sirenix.OdinInspector;
using UnityEngine;

namespace CrystalAlchemist
{
    public class SkillSenderModule : SkillModule
    {
        [BoxGroup("Sender Attribute")]
        [HideLabel]
        public Costs costs;

        [BoxGroup("Sender Attribute")]
        [Tooltip("Intervall während der Dauer des Skills Leben oder Mana verändert werden.")]
        [MinValue(0)]
        public float intervallSender = 0;

        [BoxGroup("Sender Attribute")]
        [Tooltip("Bewegungsgeschwindigkeit während eines Casts")]
        [Range(-100, 0)]
        public int speedDuringCasting = 0;

        [BoxGroup("Sender Attribute")]
        [Tooltip("Bewegungsgeschwindigkeit während des Angriffs")]
        [Range(-100, 0)]
        public int speedDuringDuration = 0;

        [BoxGroup("Sender Attribute")]
        [Tooltip("Soll die Geschwindigkeit auch die Animation beeinflussen?")]
        public bool affectAnimation = true;

        [Space(10)]
        [BoxGroup("Sender Attribute")]
        [Tooltip("True = nach vorne, False = Knockback")]
        [SerializeField]
        private bool forward = false;

        [BoxGroup("Sender Attribute")]
        [MinValue(0)]
        [Tooltip("Stärke des Knockbacks")]
        [SerializeField]
        private float selfThrust = 0;

        [BoxGroup("Sender Attribute")]
        [MinValue(0)]
        [Tooltip("Dauer des Knockbacks")]
        [HideIf("selfThrust", 0f)]
        [SerializeField]
        private float selfThrustTime = 0;

        private float elapsed;


        public override void Initialize()
        {
            if (this.skill.sender != null)
            {
                if (this.skill.sender.values.currentState != CharacterState.dead
                    && this.skill.sender.values.currentState != CharacterState.respawning)
                {
                    updateResourceSender();
                    setSelfTrust();

                    this.elapsed = this.intervallSender;
                }

                if (this.speedDuringDuration != 0) this.skill.sender.updateSpeed(this.speedDuringDuration, this.affectAnimation);
            }
        }

        public override void Updating()
        {
            if (this.intervallSender > 0)
            {
                if (this.elapsed > 0) this.elapsed -= (Time.deltaTime * this.skill.getTimeDistortion());
                else
                {
                    if (this.skill.sender != null)
                    {
                        if (!this.skill.sender.HasEnoughCurrency(this.costs)) this.skill.DeactivateIt();
                        else
                        {
                            this.elapsed = this.intervallSender;
                            this.updateResourceSender();
                        }
                    }
                }
            }
        }

        private void OnDestroy()
        {
            if (this.skill.sender != null)
            {
                //if (this.stateType != StateType.none) this.skill.sender.values.currentState = CharacterState.idle;
                if (this.speedDuringDuration != 0) this.skill.sender.updateSpeed(0);
            }
        }

        private void updateResourceSender()
        {
            if (this.skill.sender != null) this.skill.sender.reduceResource(this.costs);
        }

        private void setSelfTrust()
        {
            if (this.selfThrust > 0)
            {
                int trustdirection = -1; //knockback
                if (forward) trustdirection = 1; //dash

                this.skill.sender.KnockBack(selfThrustTime, selfThrust, (this.skill.GetDirection() * trustdirection));
            }
        }
    }
}
