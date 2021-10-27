using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace CrystalAlchemist
{
    public enum RequirementType
    {
        single,
        all
    }

    [System.Serializable]
    public class AIEvent
    {
        [SerializeField]
        private List<AITrigger> triggers = new List<AITrigger>();

        [SerializeField]
        private List<AIAction> actions = new List<AIAction>();

        [SerializeField]
        private bool repeatEvent = false;

        [SerializeField]
        private bool interruptCurrentAction = false;

        [SerializeField]
        [ShowIf("repeatEvent")]
        private float eventCooldown = 0;

        [SerializeField]
        private RequirementType requirementsNeeded = RequirementType.single;

        private bool eventActive = true;
        private float timeLeft = 0;

        [SerializeField]
        [MinValue(0.1)]
        private float delay = 0.1f; //to prevent triggering bevor initial action

        public void Initialize()
        {
            this.eventActive = true;
            this.timeLeft = 0;

            foreach (AITrigger trigger in this.triggers)
            {
                trigger.Initialize();
            }
        }

        public void Updating(AI npc)
        {
            if (this.eventActive)
            {
                foreach (AITrigger trigger in this.triggers) trigger.Updating();
            }

            if (!this.eventActive && this.repeatEvent) updateTimer();
            if (this.delay > 0) this.delay -= Time.deltaTime;
        }

        private void updateTimer()
        {
            if (this.timeLeft > 0) this.timeLeft -= Time.deltaTime;
            else { this.timeLeft = 0; this.eventActive = true; }
        }

        public bool IsEventTriggered(AI npc, int loops)
        {
            if ((this.delay <= 0 && this.eventActive && this.IsTriggered(npc, loops)))
            {
                this.eventActive = false;
                if (this.repeatEvent) this.timeLeft = this.eventCooldown;

                return true;
            }

            return false;
        }

        private bool IsTriggered(AI npc, int loops)
        {
            int triggerCount = 0;

            foreach (AITrigger trigger in this.triggers)
            {
                if (trigger.IsTriggered(npc,loops)) triggerCount++;
            }

            if ((this.requirementsNeeded == RequirementType.all && triggerCount == this.triggers.Count)
                || (this.requirementsNeeded == RequirementType.single && triggerCount > 0)) return true;

            return false;
        }

        public List<AIAction> GetActions()
        {
            return this.actions;
        }
    }
}