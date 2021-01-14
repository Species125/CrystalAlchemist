using UnityEngine;

namespace CrystalAlchemist
{
    public class TargetingCollider : MonoBehaviour
    {
        [SerializeField]
        private TargetingSystem system;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            system.addTarget(collision);
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            system.addTarget(collision);
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            system.removeTarget(collision);
        }
    }
}
