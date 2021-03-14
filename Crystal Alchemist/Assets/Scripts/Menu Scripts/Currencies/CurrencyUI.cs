using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CrystalAlchemist
{
    public class CurrencyUI : MonoBehaviour
    {
        [SerializeField]
        private GameObject child;

        [SerializeField]
        List<CurrencySlot> slots = new List<CurrencySlot>();

        [SerializeField]
        private CharacterValues values;

        [SerializeField]
        private float delay = 5f;

        private void Start()
        {
            GameEvents.current.OnCurrencyChanged += UpdateCurrency;
            UpdateCurrency(true);
        }

        private void OnDestroy()
        {
            GameEvents.current.OnCurrencyChanged -= UpdateCurrency;
        }

        private IEnumerator hideCo()
        {
            yield return new WaitForSeconds(this.delay);
            child.SetActive(false);
        }

        private void UpdateCurrency(bool show)
        {
            if (!this.gameObject.activeInHierarchy || !this.enabled) return;
            if (show) ShowUI();
            else child.SetActive(false);
        }

        private void ShowUI()
        {
            StopAllCoroutines();
            child.SetActive(true);
            foreach (CurrencySlot slot in this.slots) slot.UpdateCurrency();
            if (values.currentState != CharacterState.interact) StartCoroutine(hideCo());
        }

    }
}
