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
        private PhotonView bedPosition;

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
            return (base.PlayerCanInteract() 
                && (playerID <= 0 || playerID == NetworkUtil.GetID(this.player)));
        }

        public override void OnExit(Collider2D characterCollisionBox)
        {
            if (this.player != null && !this.player.values.IsAlive()) return;
            base.OnExit(characterCollisionBox);
        }

        public override void DoOnSubmit()
        {
            if (!this.isSleeping) SleepingStart();
            else WakeUpStart();
        }

        private void SleepingStart()
        {
            PhotonNetwork.CurrentRoom.IsOpen = false;
            SetBedInUse(NetworkUtil.GetID(this.player));

            GameEvents.current.DoChangeState(CharacterState.respawning);
            this.isSleeping = true;
            this.position = this.player.transform.position;

            this.player.SetTransform(this.bedPosition.transform.position, this.bedPosition.ViewID);

            SetBlanket(true);
            this.translationID = this.wakeUpActionID;

            MusicEvents.current.PauseMusic(this.fadeOut);
            MusicEvents.current.PlayMusicOnce(this.music, 0, 0);

            GameEvents.current.DoSleep(() => SleepingEnd());
        }

        private void SleepingEnd()
        {
            GameEvents.current.DoChangeState(CharacterState.sleeping);
            GameEvents.current.DoTimeChange(this.newValue);
        }

        private void WakeUpStart()
        {
            GameEvents.current.DoChangeState(CharacterState.respawning);
            this.player.myRigidbody.velocity = Vector2.zero;
            this.translationID = this.oldID;

            GameEvents.current.DoTimeReset();
            GameEvents.current.DoWakeUp(this.position, () => WakeUpEnd());
        }

        private void WakeUpEnd()
        {
            SetBlanket(false);
            this.isSleeping = false;
            MusicEvents.current.RestartMusic(this.fadeIn);
            SetBedInUse(0);

            PhotonNetwork.CurrentRoom.IsOpen = true;
            GameEvents.current.DoChangeState(CharacterState.idle);
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
