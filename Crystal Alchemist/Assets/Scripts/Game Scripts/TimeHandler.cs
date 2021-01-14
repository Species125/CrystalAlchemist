

using Photon.Pun;
using UnityEngine;

namespace CrystalAlchemist
{
    public class TimeHandler : MonoBehaviour, IPunObservable
    {
        //TODO: RPC instead of Observable

        [SerializeField]
        private TimeValue timeValue;

        private void Start()
        {
            this.timeValue.Reset();
        }

        private void FixedUpdate()
        {
            if (!NetworkUtil.IsMaster()) return;
            this.timeValue.setTime(Time.fixedDeltaTime);
        }


        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                stream.SendNext(this.timeValue.hour);
                stream.SendNext(this.timeValue.minute);
            }
            else
            {
                this.timeValue.hour = (int)stream.ReceiveNext();
                this.timeValue.minute = (int)stream.ReceiveNext();
            }
        }
    }
}
