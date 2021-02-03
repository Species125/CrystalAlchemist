



using UnityEngine;

namespace CrystalAlchemist
{
    public class ChangeLanguageMenu : OptionsSwitch
    {
        private void OnEnable() => getFlag();

        private void getFlag()
        {
            if (MasterManager.settings.language == Language.English) this.switchButtons(this.secondButton, this.firstButton);
            else this.switchButtons(this.firstButton, this.secondButton);
        }

        public void changeLanguage(GameObject gameObject)
        {
            changeLanguageWithoutSave(gameObject);
            SaveSystem.SaveOptions();
        }

        public void changeLanguageWithoutSave(GameObject gameObject)
        {
            if (gameObject.name.ToUpper() == "GER") MasterManager.settings.language = Language.German;
            else MasterManager.settings.language = Language.English;

            getFlag();
            SettingsEvents.current.DoLanguageChange();
        }
    }
}
