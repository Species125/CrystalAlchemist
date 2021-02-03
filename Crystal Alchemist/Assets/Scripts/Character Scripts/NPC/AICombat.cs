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

        [BoxGroup("AI")]
        [SerializeField]
        [ShowIf("startPhase")]
        [Tooltip("False, wenn Animator Event verwendet wird")]
        private bool startImmediately = true;

        [BoxGroup("Debug")]
        [ReadOnly]
        [SerializeField]
        private AIPhase activePhase;
        //private bool isActive;
        private AI npc;

        private bool isInit = true;
        #endregion

        public override void Initialize()
        {
            base.Initialize();

            this.npc = this.character.GetComponent<AI>();
            StartPhase();
            isInit = false;
        }

        public void SkipPhase() => this.activePhase.SkipPhase();        

        private void OnEnable()
        {
            if (!this.isInit && this.startPhase != null && this.startImmediately) StartPhase();
        }

        public override void Updating()
        {
            base.Updating();

            if (this.activePhase != null && !this.character.values.isCharacterStunned())
            {
                this.activePhase.Updating(this.npc);
                //if (NetworkUtil.IsMaster()) this.activePhase.SetNextIndex();
            }
        }

        private void OnDisable()
        {
            if (!this.isInit) EndPhase();
        }

        private void OnDestroy()
        {
            if (!this.isInit) EndPhase();
        }


        private void DestroyActivePhase()
        {   
            if (this.activePhase != null)
            {
                this.activePhase.ResetActions(this.npc);
                Destroy(this.activePhase);
            }
        }

        public override List<Character> GetTargetsFromTargeting()
        {
            List<Character> result = new List<Character>();
            foreach (KeyValuePair<int, float[]> aggro in this.npc.aggroList) result.Add(NetworkUtil.GetCharacter(aggro.Key));
            return result;
        }

        public override void ShowTargetingSystem(Ability ability)
        {

        }

        public void StartPhase() => StartPhase(this.startPhase);

        public void StartPhase(AIPhase phase)
        {
            if (!NetworkUtil.IsMaster()) return;

            string path = "";
            if(phase != null) path = phase.path;

            this.character.photonView.RPC("RpcStartPhase", RpcTarget.All, path);
        }

        [PunRPC]
        protected void RpcStartPhase(string path, PhotonMessageInfo info)
        {
            if (path.Replace(" ","").Length <= 1) return;
            AIPhase phase = Resources.Load<AIPhase>(path);

            if (phase != null)
            {
                //this.isActive = true;
                DestroyActivePhase();
                this.activePhase = Instantiate(phase);
                this.activePhase.Initialize(this.npc);
            }
        }

        public void EndPhase()
        {
            if (!NetworkUtil.IsMaster()) return;
            this.character.photonView.RPC("RpcEndPhase", RpcTarget.All);
        }

        [PunRPC]
        protected void RpcEndPhase(PhotonMessageInfo info)
        {
            //this.isActive = false;
            DestroyActivePhase();
        }        
    }
}

