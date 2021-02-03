using Sirenix.OdinInspector;
using UnityEngine;

namespace CrystalAlchemist
{
    public class RewardArea : MonoBehaviour
    {
        [InfoBox("Maybe set bouncing to true for items", InfoMessageType.Info)]
        [SerializeField]
        private bool collectAll;

        [SerializeField]
        [MinValue(1)]
        [HideIf("collectAll")]
        private int maxAmount = 1;

        private int counter;

        private void Start() => GameEvents.current.OnCollect += DestroyItems;

        private void OnDestroy() => GameEvents.current.OnCollect -= DestroyItems;

        private void DestroyItems(ItemStats stats)
        {
            if (this.collectAll) return;
            counter++;
            if (counter >= this.maxAmount) this.gameObject.SetActive(false);
        }
    }
}
