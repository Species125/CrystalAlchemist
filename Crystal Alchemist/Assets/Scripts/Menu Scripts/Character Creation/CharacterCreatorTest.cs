
using Sirenix.OdinInspector;
using UnityEngine;

namespace CrystalAlchemist
{
    public class CharacterCreatorTest : MonoBehaviour
    {
        [SerializeField]
        private SimpleSignal signal;

        [Button]
        private void UpdateCharacter()
        {
            this.signal.Raise();
        }
    }
}
