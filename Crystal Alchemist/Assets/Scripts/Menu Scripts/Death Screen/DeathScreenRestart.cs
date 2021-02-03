using Sirenix.OdinInspector;
using UnityEngine;

namespace CrystalAlchemist
{
    public class DeathScreenRestart : MonoBehaviour
    {
        [Required]
        [SerializeField]
        private PlayerTeleportList playerTeleport;

        [Required]
        [SerializeField]
        private StringValue path;

        private TeleportStats stats;

        private void Start()
        {
            this.stats = Resources.Load<TeleportStats>(this.path.GetValue());
            Invoke("Restart", 1f);
        }

        private void Restart()
        {
            CancelInvoke();
            this.playerTeleport.SetNextTeleport(this.stats);
            this.playerTeleport.SetAnimation(true, true);
            GameEvents.current.DoTeleport();
        }
    }
}
