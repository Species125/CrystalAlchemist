using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace CrystalAlchemist
{
    [RequireComponent(typeof(AI))]
    public class AICombat : CharacterCombat
    {
        #region Attributes

        [BoxGroup("AI")]
        [SerializeField]
        [InfoBox("Muss der Photonview hinzugefügt werden")]
        private AIPhase startPhase;

        [SerializeField]
        private bool startImmediately = true;

        [BoxGroup("Debug")]
        [ReadOnly]
        [SerializeField]
        private AIPhase activePhase;

        private AI npc;

        [BoxGroup("Debug")]
        [ReadOnly]
        [SerializeField]
        private AIAction currentAction;

        [BoxGroup("Debug")]
        [ReadOnly]
        [SerializeField]
        private AIAction currentDialog;

        [BoxGroup("Debug")]
        [SerializeField]
        [ReadOnly]
        private List<int> eventList = new List<int>();

        [BoxGroup("Debug")]
        [SerializeField]
        [ReadOnly]
        private List<AIAction> eventActions = new List<AIAction>();

        [BoxGroup("Debug")]
        [ReadOnly]
        [SerializeField]
        private int index;

        [BoxGroup("Debug")]
        [ReadOnly]
        [SerializeField]
        private int eventIndex;

        [BoxGroup("Debug")]
        [ReadOnly]
        [SerializeField]
        private int loops;

        private bool isInit = true;
        #endregion

        [Button]
        public void StartPhase() => StartPhase(this.startPhase);


        [Button]
        private void NextPhase()
        {
            foreach (AIAction action in this.activePhase.actions)
            {
                if (action.GetActionType() == AIAction.AIActionType.startPhase)
                {
                    StartPhase(action.GetPhase());
                    break;
                }
            }
        }



        #region startphase

        public override void Initialize()
        {
            base.Initialize();

            this.npc = this.character.GetComponent<AI>();
            if (this.startImmediately) StartPhase();
            isInit = false;
        }

        private void OnEnable()
        {
            if (!this.isInit && this.startPhase != null && this.startImmediately) StartPhase();
        }

        public void StartPhase(AIPhase phase)
        {
            if (!NetworkUtil.IsMaster()) return;

            string path = "";
            if (phase != null) path = phase.path;

            this.character.photonView.RPC("RpcStartPhase", RpcTarget.All, path);
        }

        [PunRPC]
        protected void RpcStartPhase(string path, PhotonMessageInfo info)
        {
            if (path.Replace(" ", "").Length <= 1) return;
            AIPhase phase = Resources.Load<AIPhase>(path);

            SetPhase(phase);
        }

        private void SetPhase(AIPhase phase)
        {
            if (phase == null) return;

            DestroyActivePhase();
            this.activePhase = Instantiate(phase);
            this.activePhase.Initialize(); //Initialize Events
        }

        #endregion


        #region Update

        public override void Updating()
        {
            base.Updating();

            if (!this.activePhase) return;

            UpdatingActions(); //all clients!
            UpdateingEvent();

            if (!NetworkUtil.IsMaster()
                || this.character.values.isCharacterStunned()
               ) return; //Master will set new Actions!

            GetWaitAfterAction();
            GetEvent();
            GetNextAction();
        }


        private void UpdatingActions()
        {
            if (this.currentAction != null) 
                this.currentAction.Updating(this.npc);

            if (this.currentDialog != null) 
                this.currentDialog.Updating(this.npc);
        }

        private void UpdateingEvent()
        {
            for (int i = 0; i < this.activePhase.events.Count; i++)
            {
                AIEvent aiEvent = this.activePhase.events[i];
                aiEvent.Updating(this.npc);
            }
        }

        private void GetWaitAfterAction()
        {
            if (this.currentAction != null && !this.currentAction.getActive())
            {
                if (this.currentAction.GetWait() > 0) AddWait(new AIAction(this.currentAction.GetWait(), this.npc), false); //wait
                else AddWait(null, false); //clear
            }
            if (this.currentDialog != null && !this.currentDialog.getActive())
            {
                if (this.currentDialog.GetWait() > 0) AddWait(new AIAction(this.currentDialog.GetWait(), this.npc), true); //wait
                else AddWait(null, true); //clear
            }
        }

        private void GetEvent()
        {
            for (int i = 0; i < this.activePhase.events.Count; i++)
            {
                AIEvent aiEvent = this.activePhase.events[i];

                if (aiEvent.IsEventTriggered(this.npc, this.loops) 
                    && !this.eventList.Contains(i)) this.eventList.Add(i);                
            }

            if (this.eventList.Count > 0) this.character.photonView.RPC("RpcNextEvent", RpcTarget.All, this.eventList);
        }

        [PunRPC]
        protected void RpcNextEvent(List<int> events, PhotonMessageInfo info)
        {
            CancelAction();

            foreach (int _event in events)
            {
                this.eventActions.AddRange(this.activePhase.events[_event].GetActions());
            }

            this.eventList.Clear();
        }



        private void GetNextAction()
        {
            if (this.eventActions.Count > 0) //EVENT!
            {
                if (this.eventIndex < this.eventActions.Count)
                {
                    if (this.currentDialog == null && this.eventActions[this.eventIndex].isDialog())
                    {
                        this.currentDialog = this.eventActions[this.eventIndex];
                        this.eventIndex++;
                        this.currentDialog.Initialize(this.npc);
                    }
                    else if (this.currentAction == null && !this.eventActions[this.eventIndex].isDialog())
                    {
                        this.currentAction = this.eventActions[this.eventIndex];
                        this.eventIndex++;
                        this.currentAction.Initialize(this.npc);
                    }
                }
                else
                {
                    this.eventActions.Clear();
                    this.eventIndex = 0;
                }
            }
            else //no active Event, do normal action
            {
                if (this.index < this.activePhase.actions.Count)
                {
                    if ((this.currentDialog == null && this.activePhase.actions[this.index].isDialog()) //Is Dialog?
                      || (this.currentAction == null && !this.activePhase.actions[this.index].isDialog())) //or is Action?
                    {
                        this.character.photonView.RPC("RpcNextAction", RpcTarget.All, this.index);
                        this.index++;
                    }
                }
                else
                {
                    //Repeat phase from start
                    if (this.currentAction == null
                        && this.currentDialog == null
                        && this.activePhase.loopActions)
                    {
                        this.index = 0;
                        this.loops++;
                    }
                }
            }
        }

        [PunRPC]
        protected void RpcNextAction(int index, PhotonMessageInfo info)
        {
            AIAction action = this.activePhase.actions[index];

            if (action.isDialog())
            {
                this.currentDialog = action;
                this.currentDialog.Initialize(this.npc);
            }
            else
            {
                this.currentAction = action;
                this.currentAction.Initialize(this.npc);
            }

            this.index = index;
        }

        /// <summary>
        /// Set Wait to Master and clear Actions on Clients
        /// </summary>
        private void AddWait(AIAction action, bool isDialog)
        {
            if (isDialog) this.currentDialog = action;
            else this.currentAction = action;

            this.character.photonView.RPC("RpcWaitAction", RpcTarget.Others, isDialog);
        }

        [PunRPC]
        protected void RpcWaitAction(bool isDialog, PhotonMessageInfo info)
        {
            if (isDialog) this.currentDialog = null;
            else this.currentAction = null;
        }

        #endregion


        #region Targeting

        public override List<Character> GetTargetsFromTargeting()
        {
            return this.npc.GetTargets();
        }

        public override void ShowTargetingSystem(Ability ability)
        {

        }

        #endregion



        #region End Phase

        private void OnDisable()
        {
            if (!this.isInit) EndPhase();
        }

        private void OnDestroy()
        {
            if (!this.isInit) EndPhase();
        }

        [Button]
        public void EndPhase()
        {
            if (!NetworkUtil.IsMaster()) return;
            this.character.photonView.RPC("RpcEndPhase", RpcTarget.All);
        }

        [PunRPC]
        protected void RpcEndPhase(PhotonMessageInfo info)
        {
            DestroyActivePhase();
        }

        private void DestroyActivePhase()
        {
            if (this.activePhase != null) ClearPhase();
        }

        public void ClearPhase()
        {
            CancelAction();

            this.loops = 0;
            this.index = 0;

            this.eventList.Clear();
            this.eventIndex = 0;
            this.eventActions.Clear();

            Destroy(this.activePhase);
        }

        private void CancelAction()
        {
            if (this.currentAction != null) this.currentAction.Disable(this.npc);
            if (this.currentDialog != null) this.currentDialog.Disable(this.npc);

            this.currentAction = null;
            this.currentDialog = null;
        }

        #endregion
    }
}

