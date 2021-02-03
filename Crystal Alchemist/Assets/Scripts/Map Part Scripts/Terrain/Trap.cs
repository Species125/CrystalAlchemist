using UnityEngine;
using UnityEngine.Events;

namespace CrystalAlchemist
{
    public class Trap : Terrain
    {
        [SerializeField]
        private UnityEvent events;

        [SerializeField]
        private bool repeatEvents;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            this.events?.Invoke();
            if (!this.repeatEvents) this.enabled = false;
        }
    }
}
