using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace CrystalAlchemist
{
    [RequireComponent(typeof(TMP_InputField))]
    public class InputFieldExtension : MonoBehaviour, ISelectHandler
    {
        private TMP_InputField inputField;

        [SerializeField]
        private ButtonExtension button;

        [SerializeField]
        private UnityEvent OnSelected;

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

        public void OnSelect(BaseEventData eventData)
        {
            this.OnSelected?.Invoke();
        }
    }
}
