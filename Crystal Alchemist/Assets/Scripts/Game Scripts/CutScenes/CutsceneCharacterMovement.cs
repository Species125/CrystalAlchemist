using DG.Tweening;
using UnityEngine;

namespace CrystalAlchemist
{
    public class CutsceneCharacterMovement : MonoBehaviour
    {
        [SerializeField]
        private Vector2 position;

        [SerializeField]
        private float duration;

        private Rigidbody2D myRigidbody;

        private void Start()
        {
            Player player = NetworkUtil.GetLocalPlayer();
            if (player == null) return;
            this.myRigidbody = player.gameObject.GetComponent<Rigidbody2D>();
        }

        public void Play()
        {
            this.myRigidbody?.DOMove(position, this.duration);
        }
    }
}
