


using UnityEngine;

namespace CrystalAlchemist
{
    public class ChangeStatusBarMenu : OptionsSwitch
    {
        [SerializeField]
        private CostType resourceType;

        private void OnEnable() => getLayout();

        private void getLayout()
        {
            if (this.resourceType == CostType.life) setLayout(MasterManager.settings.healthBar);
            else if (this.resourceType == CostType.mana) setLayout(MasterManager.settings.manaBar);
        }

        private void setLayout(bool value)
        {
            if (value) this.switchButtons(this.secondButton, this.firstButton);
            else this.switchButtons(this.firstButton, this.secondButton);
        }

        public void changeLayout(bool useBar)
        {
            if (this.resourceType == CostType.life) MasterManager.settings.healthBar = useBar;
            else if (this.resourceType == CostType.mana) MasterManager.settings.manaBar = useBar;

            getLayout();
            SettingsEvents.current.DoHUDChange();
        }
    }
}
