
using UnityEngine;

namespace CrystalAlchemist
{
    public enum InputDeviceType
    {
        keyboard,
        mouse,
        gamepad
    }


    [CreateAssetMenu(menuName = "Game/Settings/Device Info")]
    public class InputDeviceInfo : ScriptableObject
    {
        public string button;

        public InputDeviceType type;

        public void SetDevice(string type, string button)
        {
            this.button = button;
            if (type.Contains("Keyboard")) this.type = InputDeviceType.keyboard;
            else if (type.Contains("Mouse")) this.type = InputDeviceType.mouse;
            else this.type = InputDeviceType.gamepad;

            UpdateLayout();
            GameEvents.current.DoDeviceChanged();
        }

        private void UpdateLayout()
        {
            if (this.type == InputDeviceType.gamepad
                && MasterManager.settings.layoutType != InputDeviceType.gamepad)
            {
                MasterManager.settings.layoutType = InputDeviceType.gamepad;
                SettingsEvents.current.DoLayoutChange();
            }
            else if (this.type != InputDeviceType.gamepad
                     && MasterManager.settings.layoutType == InputDeviceType.gamepad)
            {
                MasterManager.settings.layoutType = InputDeviceType.keyboard;
                SettingsEvents.current.DoLayoutChange();
            }
        }
    }
}