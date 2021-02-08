using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
using UnityEngine;

namespace CrystalAlchemist
{
    public class JukeboxMenu : MenuBehaviour
    {
        [SerializeField]
        private float fadeIn = 2f;

        [SerializeField]
        private float fadeOut;

        public override void Start()
        {
            base.Start();
            PhotonNetwork.NetworkingClient.EventReceived += NetworkingEvent;
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            PhotonNetwork.NetworkingClient.EventReceived += NetworkingEvent;
        }

        public void PlayMusic(JukeboxButton button)
        {
            StopMusic();
            RPCPlay(button.GetTheme());
        }

        public void Pause() => RPCPause();

        public void StopMusic() => RPCStop();

        private void NetworkingEvent(EventData obj)
        {
            if (obj.Code == NetworkUtil.JUKEBOX_PLAY)
            {
                object[] datas = (object[])obj.CustomData;
                string path = (string)datas[0];

                MusicTheme theme = Resources.Load<MusicTheme>(path);
                MusicEvents.current.PlayMusic(theme, this.fadeIn);
                Debug.Log("Change Theme to " + theme.name);
            }
            else if (obj.Code == NetworkUtil.JUKEBOX_PAUSE)
            {
                MusicEvents.current.TogglePause();
                Debug.Log("Paused theme");
            }
            else if (obj.Code == NetworkUtil.JUKEBOX_STOP)
            {
                MusicEvents.current.StopMusic(this.fadeOut);
                Debug.Log("Stopped theme");
            }
        }

        private void RPCPlay(MusicTheme theme)
        {
            object[] datas = new object[] { theme.path };
            RaiseEventOptions options = NetworkUtil.TargetAll();

            PhotonNetwork.RaiseEvent(NetworkUtil.JUKEBOX_PLAY, datas, options, SendOptions.SendUnreliable);
        }

        private void RPCPause()
        {
            object[] datas = new object[0];
            RaiseEventOptions options = NetworkUtil.TargetAll();

            PhotonNetwork.RaiseEvent(NetworkUtil.JUKEBOX_PAUSE, datas, options, SendOptions.SendUnreliable);
        }

        private void RPCStop()
        {
            object[] datas = new object[0]; 
            RaiseEventOptions options = NetworkUtil.TargetAll();

            PhotonNetwork.RaiseEvent(NetworkUtil.JUKEBOX_STOP, datas, options, SendOptions.SendUnreliable);
        }
    }
}
