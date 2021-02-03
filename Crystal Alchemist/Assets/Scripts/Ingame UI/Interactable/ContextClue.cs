using UnityEngine;

namespace CrystalAlchemist
{
    public class ContextClue : MonoBehaviour
    {
        private void OnEnable() => GameEvents.current.DoCurrencyChange(true);

        private void OnDisable() => GameEvents.current.DoCurrencyChange(false);
    }
}
