using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace CrystalAlchemist
{
    [CreateAssetMenu(menuName = "Game/AI/AI Phase")]
    public class AIPhase : NetworkScriptableObject
    {
        [BoxGroup("Action Sequence")]
        [SerializeField]
        private List<AIAction> actions;

        [BoxGroup("Action Sequence")]
        [SerializeField]
        private bool loopActions;

        [BoxGroup("Events")]
        [SerializeField]
        private List<AIEvent> events;

        [BoxGroup("Debug")]
        [SerializeField]
        private AIAction currentAction;

        [BoxGroup("Debug")]
        [SerializeField]
        private AIAction currentDialog;

        [BoxGroup("Debug")]
        [SerializeField]
        private List<AIAction> eventActions = new List<AIAction>();

        [BoxGroup("Debug")]
        public int index;

        [BoxGroup("Debug")]
        public int eventIndex;

        [BoxGroup("Debug")]
        public int loops;

        public void SkipPhase()
        {
            foreach (AIEvent ev in this.events) ev.SkipPhase();
        }

        public void Initialize(AI npc)
        {
            foreach (AIEvent aiEvent in this.events) aiEvent.Initialize();
            ResetActions(npc);
        }

        public void Updating(AI npc)
        {
            if (this.currentAction != null && !this.currentAction.getActive())
            {
                if (this.currentAction.GetWait() > 0) this.currentAction = new AIAction(this.currentAction.GetWait(), npc);
                else this.currentAction = null;
            }
            if (this.currentDialog != null && !this.currentDialog.getActive())
            {
                if (this.currentDialog.GetWait() > 0) this.currentDialog = new AIAction(this.currentDialog.GetWait(), npc);
                else this.currentDialog = null;
            }

            foreach (AIEvent aiEvent in this.events)
            {
                aiEvent.Updating(npc);
                aiEvent.SetEventActions(npc, this.eventActions, this);
            }

            SetNextAction(npc);
            UpdatingAction(npc);
        }

        public int GetLoops()
        {
            return this.loops;
        }

        private void SetNextAction(AI npc)
        {
            if (this.eventActions.Count == 0)
            {
                if (this.index < this.actions.Count)
                {
                    if (this.currentDialog == null && this.actions[this.index].isDialog())
                    {
                        this.currentDialog = this.actions[this.index];
                        this.currentDialog.Initialize(npc);
                        this.index++;
                    }
                    else if (this.currentAction == null && !this.actions[this.index].isDialog())
                    {
                        this.currentAction = this.actions[this.index];
                        this.currentAction.Initialize(npc);
                        this.index++;
                    }
                }
                else
                {
                    if (this.currentAction == null
                        && this.currentDialog == null
                        && this.loopActions)
                    {
                        this.index = 0;
                        this.loops++;
                    }
                }
            }
            else
            {
                if (this.eventIndex < this.eventActions.Count)
                {
                    if (this.currentDialog == null && this.eventActions[this.eventIndex].isDialog())
                    {
                        this.currentDialog = this.eventActions[this.eventIndex];
                        this.currentDialog.Initialize(npc);
                        this.eventIndex++;
                    }
                    else if (this.currentAction == null && !this.eventActions[this.eventIndex].isDialog())
                    {
                        this.currentAction = this.eventActions[this.eventIndex];
                        this.currentAction.Initialize(npc);
                        this.eventIndex++;
                    }
                }
                else
                {
                    this.eventIndex = 0;
                    this.eventActions.Clear();
                }
            }
        }

        private void UpdatingAction(AI npc)
        {
            if (this.currentAction != null) this.currentAction.Updating(npc);
            if (this.currentDialog != null) this.currentDialog.Updating(npc);
        }

        public void ResetActions(AI npc)
        {
            if (this.currentAction != null) this.currentAction.Disable(npc);
            if (this.currentDialog != null) this.currentDialog.Disable(npc);

            this.currentAction = null;
            this.currentDialog = null;
        }
    }
}
