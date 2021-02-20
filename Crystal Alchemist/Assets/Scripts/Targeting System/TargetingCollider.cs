using UnityEngine;

namespace CrystalAlchemist
{
    public class TargetingCollider : MonoBehaviour
    {
        [SerializeField]
        private TargetingSystem system;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            system.AddTarget(collision);
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            system.RemoveTarget(collision);
        }
    }
}
