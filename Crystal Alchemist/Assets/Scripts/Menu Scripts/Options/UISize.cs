

using UnityEngine;

namespace CrystalAlchemist
{
    public class UISize : MonoBehaviour
    {
        private void Start() => SettingsEvents.current.OnUISizeChanged += UpdateLayout;

        private void OnEnable() => UpdateLayout();

        private void OnDestroy() => SettingsEvents.current.OnUISizeChanged -= UpdateLayout;

        private void UpdateLayout() => this.transform.localScale = new Vector3(MasterManager.settings.UISize, MasterManager.settings.UISize, 1);
    }
}
