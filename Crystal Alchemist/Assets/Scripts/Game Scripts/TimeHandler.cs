using Photon.Pun;
using UnityEngine;
using ExitGames.Client.Photon;
using Photon.Realtime;

namespace CrystalAlchemist
{
    public class TimeHandler : MonoBehaviourPunCallbacks
    {
        [SerializeField]
        private TimeValue timeValue;

        private void Start()
        {
            this.timeValue.Reset();
        }

        public override void OnEnable()
        {
            base.OnEnable();
            PhotonNetwork.NetworkingClient.EventReceived += NetworkingEvent;
            GameEvents.current.OnTimeChange += RaiseTimeChangedEvent;
            GameEvents.current.OnTimeReset += RaiseTimeResetEvent;
        }

        public override void OnDisable()
        {
            base.OnDisable();
            PhotonNetwork.NetworkingClient.EventReceived -= NetworkingEvent;
            GameEvents.current.OnTimeChange -= RaiseTimeChangedEvent;
            GameEvents.current.OnTimeReset -= RaiseTimeResetEvent;
        }

        private void NetworkingEvent(EventData obj)
        {
            if (obj.Code == NetworkUtil.TIME_STARTUP)
            {
                object[] datas = (object[])obj.CustomData;
                int hour = (int)datas[0];
                int minute = (int)datas[1];

                this.timeValue.SetTime(hour, minute);
            }
            else if (obj.Code == NetworkUtil.TIME_CHANGED)
            {
                object[] datas = (object[])obj.CustomData;
                float factor = (float)datas[0];

                this.timeValue.SetFactor(factor);
            }
            else if (obj.Code == NetworkUtil.TIME_RESET)
            {
                this.timeValue.Reset();
            }
        }

        private void RaiseTimeStartEvent()
        {
            if (!NetworkUtil.IsMaster()) return;

            object[] datas = new object[] { this.timeValue.GetHour(), this.timeValue.GetMinute() };
            RaiseEventOptions options = NetworkUtil.TargetAll();

            PhotonNetwork.RaiseEvent(NetworkUtil.TIME_STARTUP, datas, options, SendOptions.SendUnreliable);
        }

        private void RaiseTimeResetEvent()
        {
            if (!NetworkUtil.IsMaster()) return;

            object[] datas = new object[0];
            RaiseEventOptions options = NetworkUtil.TargetAll();

            PhotonNetwork.RaiseEvent(NetworkUtil.TIME_RESET, datas, options, SendOptions.SendUnreliable);
        }

        private void RaiseTimeChangedEvent(float factor)
        {
            if (!NetworkUtil.IsMaster()) return;

            object[] datas = new object[] { factor };
            RaiseEventOptions options = NetworkUtil.TargetAll();

            PhotonNetwork.RaiseEvent(NetworkUtil.TIME_CHANGED, datas, options, SendOptions.SendUnreliable);
        }

        private void FixedUpdate() => this.timeValue.SetTime(Time.fixedDeltaTime);        

        public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer) => RaiseTimeStartEvent();
     
    }
}
