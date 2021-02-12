using UnityEngine;

namespace CrystalAlchemist
{
    public class KeybindingMenu : MonoBehaviour
    {
        [SerializeField]
        private GameObject gamepad;

        [SerializeField]
        private GameObject keyboard;

        private void Start()
        {
            UpdateLayout();
            GameEvents.current.OnDeviceChanged += UpdateLayout;
        }

        private void OnEnable()
        {
            UpdateLayout();
        }

        private void OnDestroy()
        {
            GameEvents.current.OnDeviceChanged -= UpdateLayout;
        }

        private void UpdateLayout()
        {
            this.gamepad.SetActive(false);
            this.keyboard.SetActive(false);

            if (MasterManager.inputDeviceInfo.type == InputDeviceType.gamepad) this.gamepad.SetActive(true);
            else this.keyboard.SetActive(true);
        }
    }
}
