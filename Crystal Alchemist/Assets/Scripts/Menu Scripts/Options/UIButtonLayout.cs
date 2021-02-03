using System.Collections.Generic;


using UnityEngine;

namespace CrystalAlchemist
{
    public class UIButtonLayout : MonoBehaviour
    {
        [SerializeField]
        private List<GameObject> gamepadUI;

        [SerializeField]
        private List<GameObject> keyboardUI;

        [SerializeField]
        private bool updateOnRuntime = true;

        private void setActive(bool value, List<GameObject> gameObjects)
        {
            foreach (GameObject obje in gameObjects) obje.SetActive(value);
        }

        private void Start()
        {
            UpdateLayout();
            if (this.updateOnRuntime) SettingsEvents.current.OnLayoutChanged += UpdateLayout;
        }

        //private void OnEnable() => UpdateLayout();

        private void OnDestroy()
        {
            if (this.updateOnRuntime) SettingsEvents.current.OnLayoutChanged -= UpdateLayout;
        }

        private void UpdateLayout()
        {
            if (MasterManager.settings.layoutType == InputDeviceType.keyboard)
            {
                setActive(false, this.gamepadUI);
                setActive(true, this.keyboardUI);
            }
            else
            {
                setActive(true, this.gamepadUI);
                setActive(false, this.keyboardUI);
            }
        }
    }
}
