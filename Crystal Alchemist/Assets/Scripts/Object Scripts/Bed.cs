using Sirenix.OdinInspector;
using UnityEngine;
using Photon.Pun;

namespace CrystalAlchemist
{
    public class Bed : Interactable
    {
        public enum BedState
        {
            awake,
            sleeping,
            wakingup
        }

        [BoxGroup("Bett")]
        [SerializeField]
        private float newValue;

        [BoxGroup("Bett")]
        [SerializeField]
        private float offset = 0.35f;

        [BoxGroup("Bett")]
        [SerializeField]
        private GameObject blanket;

        [BoxGroup("Bett")]
        [SerializeField]
        private string wakeUpActionID;

        [BoxGroup("Bett")]
        [SerializeField]
        private AudioClip music;

        [BoxGroup("Bett")]
        [SerializeField]
        private float fadeIn = 1;

        [BoxGroup("Bett")]
        [SerializeField]
        private float fadeOut = 1;

        private Vector2 position;
        private bool isSleeping;
        private string oldID;
        private int playerID;

        public override void Start()
        {
            base.Start();
            SetBlanket(false);
            this.oldID = this.translationID;
        }

        public override bool PlayerCanInteract()
        {
            return (base.PlayerCanInteract() && (playerID <= 0 || playerID == this.player.photonView.ViewID));
        }

        public override void DoOnSubmit()
        {
            if (!this.isSleeping) //sleep
            {
                SetBedInUse(this.player.photonView.ViewID);

                this.player.values.currentState = CharacterState.sleeping;
                this.isSleeping = true;
                this.position = this.player.transform.position;
                Vector2 position = new Vector2(this.transform.position.x, this.transform.position.y + offset);

                SetBlanket(true);                
                this.translationID = this.wakeUpActionID;

                MusicEvents.current.PauseMusic(this.fadeOut);
                MusicEvents.current.PlayMusicOnce(this.music, 0, 0);
                GameEvents.current.DoSleep(position, () => StartSleeping());
            }
            else //wake up
            {
                this.player.myRigidbody.velocity = Vector2.zero;                
                this.translationID = this.oldID;

                GameEvents.current.DoTimeReset();
                GameEvents.current.DoWakeUp(this.position, () => PlayerAwake());
            }
        }

        private void StartSleeping()
        {
            GameEvents.current.DoTimeChange(this.newValue);            
        }

        private void PlayerAwake()
        {
            SetBlanket(false);
            this.isSleeping = false;
            MusicEvents.current.RestartMusic(this.fadeIn);
            SetBedInUse(0);
        }        

        private void SetBedInUse(int ID) => this.photonView.RPC("RpcBedInUse", RpcTarget.All, ID);        

        [PunRPC]
        protected void RpcBedInUse(int ID) => this.playerID = ID;

        private void SetBlanket(bool value)
        {
            if (!PhotonNetwork.IsConnected) return;
            this.photonView.RPC("RpcSetBlanket", RpcTarget.All, value);
        }

        [PunRPC]
        protected void RpcSetBlanket(bool value) => this.blanket.SetActive(value);
    }
}
