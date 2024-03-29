using Sirenix.OdinInspector;
using UnityEngine;
using Photon.Pun;
using UnityEngine.Playables;

namespace CrystalAlchemist
{
    [RequireComponent(typeof(PlayableDirector))]
    public class PlayCutscene : NetworkBehaviour 
    {
        [SerializeField]
        private PlayableDirector director;

        private void Awake()
        {
            if(!this.director) this.director = GetComponent<PlayableDirector>();
        }


        //public void Play() => Invoke("PlayIt", 0.1f);

        public void PlayOnAllClients()
        {
            Play();
            if (this.photonView == null) Debug.LogError("Missing Photonview on Cutscene " + this.gameObject.name);
            this.photonView.RPC("RpcPlay", RpcTarget.Others);
        }

        [PunRPC]
        protected void RpcPlay() => Play();

        [ButtonGroup]
        private void Play() => this.director.Play();
    }
}
