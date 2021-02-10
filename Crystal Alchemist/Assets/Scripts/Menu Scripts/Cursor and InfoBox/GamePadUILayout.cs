using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CrystalAlchemist
{
    public class GamePadUILayout : MonoBehaviour
    {
        [SerializeField]
        private Sprite xbox;

        [SerializeField]
        private Sprite ps4;

        [SerializeField]
        private SpriteRenderer spriteRenderer;

        [SerializeField]
        private List<Image> images = new List<Image>();

        private void Start()
        {
            GameEvents.current.OnDeviceChanged += UpdateInputUI;
            UpdateInputUI();
        }

        private void OnDestroy() => GameEvents.current.OnDeviceChanged -= UpdateInputUI;

        private void UpdateInputUI()
        {
            InputDeviceInfo info = MasterManager.inputDeviceInfo;
            
            if (info.gamepadType == GamePadType.ps4) UpdateIcons(this.ps4);
            else UpdateIcons(this.xbox);            
        }

        private void UpdateIcons(Sprite sprite)
        {
            foreach(Image image in this.images)
            {
                image.sprite = sprite;
            }

            if (this.spriteRenderer) this.spriteRenderer.sprite = sprite;
        }
    }
}
