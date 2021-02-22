using UnityEngine;

namespace CrystalAlchemist
{
    public class DirectionRing : MonoBehaviour
    {
        [SerializeField]
        private Player player;

        [SerializeField]
        private SpriteRenderer spriteRenderer;

        private void Awake()
        {
            this.spriteRenderer.enabled = false;
            if (!player.isLocalPlayer) return;
        }

        private void OnDestroy()
        {
            if (!player.isLocalPlayer) return;
        }

        private void LateUpdate()
        {
            float angle = (Mathf.Atan2(this.player.values.direction.y, this.player.values.direction.x) * Mathf.Rad2Deg) + 90;
            Vector3 rotation = new Vector3(0, 0, angle);

            this.transform.rotation = Quaternion.Euler(rotation);

            spriteRenderer.enabled = player.values.CanMove();
        }
    }
}
