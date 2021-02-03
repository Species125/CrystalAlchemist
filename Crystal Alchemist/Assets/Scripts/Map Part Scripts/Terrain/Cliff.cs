using System.Collections.Generic;
using UnityEngine;

namespace CrystalAlchemist
{
    public class Cliff : Terrain
    {
        private List<Character> characters = new List<Character>();
        private Vector2 direction;

        [SerializeField]
        private GameObject center;

        [SerializeField]
        private float speed = 32;

        [SerializeField]
        private Collider2D collider;

        private void FixedUpdate()
        {
            foreach (Character character in this.characters)
            {
                if (character.myRigidbody != null)
                {
                    this.direction = character.myRigidbody.position - (Vector2)this.center.transform.position;

                    character.myRigidbody.AddForce(this.direction.normalized * this.speed * character.GetSpeedFactor() * character.values.timeDistortion, ForceMode2D.Force);
                }
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (this.collider != null) this.collider.enabled = false;

            Character character = collision.GetComponent<Character>();
            if (character != null && !this.characters.Contains(character))
            {
                this.characters.Add(character);
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (this.collider != null) this.collider.enabled = true;

            Character character = collision.GetComponent<Character>();
            if (character != null && this.characters.Contains(character))
            {
                this.characters.Remove(character);
                character.myRigidbody.velocity = Vector2.zero;
            }
        }
    }
}
