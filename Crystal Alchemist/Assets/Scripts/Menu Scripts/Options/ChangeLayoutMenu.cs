

using UnityEngine;

namespace CrystalAlchemist
{
    public class ChangeLayoutMenu : OptionsSwitch
    {
        private void OnEnable() => getLayout();

        private void getLayout()
        {
            if (MasterManager.settings.layoutType == InputDeviceType.keyboard)
                this.switchButtons(this.secondButton, this.firstButton);
            else
                this.switchButtons(this.firstButton, this.secondButton);
        }

        public void changeLayout(GameObject gameObject)
        {
            if (gameObject.name.ToLower() == "keyboard")
                MasterManager.settings.layoutType = InputDeviceType.keyboard;
            else
                MasterManager.settings.layoutType = InputDeviceType.gamepad;

            getLayout();
            SettingsEvents.current.DoLayoutChange();
        }
    }
}
