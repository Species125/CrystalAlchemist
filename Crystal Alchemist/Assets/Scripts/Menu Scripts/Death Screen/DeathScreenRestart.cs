using Sirenix.OdinInspector;
using System.Collections;
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
            StartCoroutine(DelayCo());
        }

        private IEnumerator DelayCo()
        {
            this.stats = Resources.Load<TeleportStats>(this.path.GetValue());
            this.playerTeleport.SetNextTeleport(this.stats, true, true);

            yield return new WaitForSeconds(2f);

            GameEvents.current.DoTeleport();
        }
    }
}
