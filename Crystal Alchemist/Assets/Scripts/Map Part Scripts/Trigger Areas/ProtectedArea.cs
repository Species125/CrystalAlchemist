using System.Collections.Generic;
using UnityEngine;

namespace CrystalAlchemist
{
    public class ProtectedArea : MonoBehaviour
    {
        [SerializeField]
        private List<AI> protectingNPCs = new List<AI>();

        [SerializeField]
        [Range(0, 120)]
        private float aggroIncreaseFactor = 25;

        [SerializeField]
        [Range(-120, 0)]
        private float aggroDecreaseFactor = -25f;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            setAggro(collision, false);
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            setAggro(collision, true);
        }

        private void setAggro(Collider2D collision, bool decrease)
        {
            this.protectingNPCs.RemoveAll(x => x == null);
            Character character = collision.GetComponent<Character>();

            if (character != null)
            {
                foreach (AI enemy in this.protectingNPCs)
                {
                    if (enemy != null && enemy.gameObject.activeInHierarchy)
                    {
                        if (!decrease) GameEvents.current.DoAggroIncrease(enemy, character, this.aggroIncreaseFactor);
                        else GameEvents.current.DoAggroDecrease(enemy, character, this.aggroDecreaseFactor);
                    }
                }
            }
        }
    }
}