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

        public void PlayMusic(JukeboxButton button)
        {
            StopMusic();
            RPCPlay(button.GetTheme());
        }

        public void Pause() => RPCPause();

        public void StopMusic() => RPCStop();        

        private void RPCPlay(MusicTheme theme)
        {
            object[] datas = new object[] { theme.path, this.fadeIn};
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
            object[] datas = new object[] { this.fadeOut };
            RaiseEventOptions options = NetworkUtil.TargetAll();

            PhotonNetwork.RaiseEvent(NetworkUtil.JUKEBOX_STOP, datas, options, SendOptions.SendUnreliable);
        }
    }
}
