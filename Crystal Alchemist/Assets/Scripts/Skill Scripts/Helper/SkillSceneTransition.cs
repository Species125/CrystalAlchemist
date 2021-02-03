using Photon.Pun;
using UnityEngine;
using TMPro;

namespace CrystalAlchemist
{
    [RequireComponent(typeof(SceneTransition))]
    public class SkillSceneTransition : NetworkBehaviour, IPunInstantiateMagicCallback
    {
        [SerializeField]
        private float duration = 30f;

        [SerializeField]
        private float interval = 1f;

        [SerializeField]
        private TextMeshPro textField;

        private SceneTransition transition;
        private float countDown;

        private void Awake()
        {
            this.transition = this.GetComponent<SceneTransition>();
        }

        public void StartCountDown()
        {            
            this.countDown = this.duration;
            InvokeRepeating("Updating", 0, this.interval);
        }

        public void OnPhotonInstantiate(PhotonMessageInfo info)
        {
            object[] instantiationData = info.photonView.InstantiationData;
            string path = (string)instantiationData[0];

            TeleportStats stats = Resources.Load<TeleportStats>(path);
            this.transition.SetTeleport(stats);
        }

        private void Updating()
        {
            this.textField.text = FormatUtil.setDurationToString(this.countDown);
            if (this.countDown > 0) this.countDown -= this.interval;
            else Destroy(this.gameObject);
        }
    }
}
