
using TMPro;
using UnityEngine;

namespace CrystalAlchemist
{
    public class CharacterCreatorName : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI textField;

        [SerializeField]
        private StringValue playerName;

        private void OnEnable()
        {
            this.textField.text = playerName.GetValue();
        }
    }
}
