using TMPro;
using UnityEngine;

namespace CrystalAlchemist
{
    [RequireComponent(typeof(TMP_InputField))]
    public class InputFieldExtension : MonoBehaviour
    {
        private TMP_InputField inputField;

        [SerializeField]
        private ButtonExtension button;

        private void Start()
        {
            this.inputField = this.GetComponent<TMP_InputField>();
            GameEvents.current.OnDeviceChanged += OnDeviceChanged;
        }

        private void OnDestroy()
        {
            GameEvents.current.OnDeviceChanged -= OnDeviceChanged;
        }

        private void OnDeviceChanged()
        {
            if (MasterManager.inputDeviceInfo.type == InputDeviceType.gamepad && this.inputField.interactable)
            {
                if (button != null) button.Select();
                this.inputField.interactable = false;
            }
            else if (MasterManager.inputDeviceInfo.type != InputDeviceType.gamepad && !this.inputField.interactable)
            {
                this.inputField.interactable = true;
            }
        }
    }
}
