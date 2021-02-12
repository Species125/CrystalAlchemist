using UnityEngine;
using UnityEngine.UI;

namespace CrystalAlchemist
{
    public class KeybindingMenuGamepad : MonoBehaviour
    {
        [SerializeField]
        private Sprite xbox;

        [SerializeField]
        private Sprite ps4;

        [SerializeField]
        private Image icon;


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
            if (MasterManager.inputDeviceInfo.gamepadType == GamePadType.ps4) this.icon.sprite = this.ps4;
            else this.icon.sprite = this.xbox;
        }
    }
}
