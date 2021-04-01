using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace CrystalAlchemist
{
    public class ProtectedArea : MonoBehaviour
    {
        [DetailedInfoBox("For hiding items when collected amount of x", "Set bouncing true for item", InfoMessageType.Info)]
        [SerializeField]
        private List<AI> protectingNPCs = new List<AI>();

        [SerializeField]
        [Range(0, 100)]
        private int aggroIncreaseFactor = 25;

        [SerializeField]
        [Range(-100, 0)]
        private int aggroDecreaseFactor = -25;

        private void OnTriggerEnter2D(Collider2D collision) => SetAggro(collision, false);

        private void OnTriggerExit2D(Collider2D collision) => SetAggro(collision, true);        

        private void SetAggro(Collider2D collision, bool decrease)
        {
            if (collision.isTrigger) return;

            this.protectingNPCs.RemoveAll(x => x == null);
            Character character = collision.GetComponent<Character>();

            if (character != null)
            {
                foreach (AI enemy in this.protectingNPCs)
                {
                    if (enemy != null && enemy.gameObject.activeInHierarchy)
                    {
                        if (!decrease) enemy._IncreaseAggro(character, this.aggroIncreaseFactor);
                        else enemy._DecreaseAggro(character, this.aggroDecreaseFactor);
                    }
                }
            }
        }
    }
}