using System.Collections.Generic;
using UnityEngine;

namespace CrystalAlchemist
{
    public class Water : Terrain
    {
        [SerializeField]
        private GameObject effect;

        [SerializeField]
        private float maxDistance = 1f;

        private List<GameObject> effects = new List<GameObject>();

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.isTrigger) return;
            Step(collision);
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            if (collision.isTrigger) return;
            Step(collision);
        }

        private void Step(Collider2D collision)
        {
            this.effects.RemoveAll(item => item == null);

            Character character = collision.GetComponent<Character>();
            if (character == null) return;

            bool distance = UnityUtil.CheckDistances(character.GetGroundPosition(), this.maxDistance, this.effects);

            if (this.effect != null && distance)
                this.effects.Add(Instantiate(this.effect, character.GetGroundPosition(), Quaternion.identity));
        }
    }
}
