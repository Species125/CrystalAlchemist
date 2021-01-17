using Sirenix.OdinInspector;
using UnityEngine;

namespace CrystalAlchemist
{
    public enum AITriggerType
    {
        time,
        life,
        range,
        aggro,
        aggroLost,
        loops
    }

    [System.Serializable]
    public class AITrigger
    {
        [SerializeField]
        private AITriggerType type;

        [ShowIf("type", AITriggerType.time)]
        [SerializeField]
        private float time;

        [ShowIf("type", AITriggerType.life)]
        [SerializeField]
        private float life;

        [ShowIf("type", AITriggerType.loops)]
        [SerializeField]
        private int maxLoops;

        private bool timesUp = false;
        private float elapsed = 0;

        public void Initialize()
        {
            startTimer();
        }

        public void Updating()
        {
            if (this.time > 0)
            {
                if (this.elapsed > 0) this.elapsed -= Time.deltaTime;
                else { this.elapsed = 0; this.timesUp = true; }
            }
        }

        public bool isTriggered(AI npc, AIPhase phase)
        {
            if (this.type == AITriggerType.aggro) return checkAggro(npc);
            else if (this.type == AITriggerType.aggroLost) return checkAggroLost(npc);
            else if (this.type == AITriggerType.life) return checkLife(npc);
            else if (this.type == AITriggerType.range) return checkRange(npc);
            else if (this.type == AITriggerType.time) return checkTime();
            else if (this.type == AITriggerType.loops) return checkLoops(phase.GetLoops());
            return false;
        }

        private bool checkLoops(int amount)
        {
            if (amount >= this.maxLoops) return true;
            return false;
        }

        private bool checkLife(AI npc)
        {
            return (npc.values.life <= (npc.values.maxLife * this.life / 100));
        }

        private bool checkRange(AI npc)
        {
            return npc.rangeTriggered;
        }

        private bool checkAggro(AI npc)
        {
            return (npc.targetID > 0);
        }

        private bool checkAggroLost(AI npc)
        {
            return (npc.targetID <= 0);
        }

        private bool checkTime()
        {
            return this.timesUp;
        }


        private void startTimer()
        {
            this.elapsed = this.time;
            this.timesUp = false;
        }
    }
}