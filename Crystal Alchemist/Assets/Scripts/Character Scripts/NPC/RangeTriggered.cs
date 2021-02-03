

using Sirenix.OdinInspector;
using UnityEngine;

namespace CrystalAlchemist
{
    public class RangeTriggered : MonoBehaviour
    {
        [Required]
        [SerializeField]
        private Affections affections;

        [Required]
        [SerializeField]
        private AI npc;

        private void Update()
        {
            RotationUtil.rotateCollider(this.npc, this.gameObject);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (this.affections.IsAffected(this.npc, collision)) GameEvents.current.DoRangeTrigger(this.npc, true);
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            if (this.affections.IsAffected(this.npc, collision)) GameEvents.current.DoRangeTrigger(this.npc, true);
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (this.affections.IsAffected(this.npc, collision)) GameEvents.current.DoRangeTrigger(this.npc, false);
        }
    }
}
