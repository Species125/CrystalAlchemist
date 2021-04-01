using Sirenix.OdinInspector;
using UnityEngine;

namespace CrystalAlchemist
{
    public class RewardArea : MonoBehaviour
    {
        [DetailedInfoBox("For hiding items when collected amount of x", "Set bouncing true for item", InfoMessageType.Info)]
        [SerializeField]
        private bool collectAll;

        [SerializeField]
        [MinValue(1)]
        [HideIf("collectAll")]
        private int maxAmount = 1;

        private int counter;

        private void Start() => GameEvents.current.OnCollect += DestroyItems;

        private void OnDestroy() => GameEvents.current.OnCollect -= DestroyItems;

        private void DestroyItems(ItemDrop drop, int amount)
        {
            if (this.collectAll) return;
            counter++;
            if (counter >= this.maxAmount) this.gameObject.SetActive(false);
        }
    }
}
