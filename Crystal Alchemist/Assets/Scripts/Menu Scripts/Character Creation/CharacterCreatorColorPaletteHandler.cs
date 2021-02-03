
using Sirenix.OdinInspector;
using UnityEngine;

namespace CrystalAlchemist
{
    public class CharacterCreatorColorPaletteHandler : CharacterCreatorButtonHandler
    {
        [Required]
        [SerializeField]
        private CharacterCreatorColorPalette palette;

        [Required]
        [SerializeField]
        private CharacterCreatorColor template;

        public ColorGroup colorGroup;

        private Color currentColor;
        private Color defaultColor = new Color(0, 0, 0, 0);

        private void Awake()
        {
            for (int i = 0; i < this.palette.colors.Count; i++)
            {
                Color color = this.palette.colors[i];
                CreateButton(color, i);

                if (HasColor(color)) SetCurrentColor(color);
            }

            Destroy(this.template.gameObject);
        }

        private void CreateButton(Color color, int i)
        {
            CharacterCreatorColor colorOption = Instantiate(this.template, this.transform);
            this.SetFirst(colorOption, i);

            colorOption.gameObject.SetActive(true);
            colorOption.SetButton(color, this);

            this.buttons.Add(colorOption);
        }

        private bool HasColor(Color color)
        {
            ColorGroupData data = this.mainMenu.playerPreset.GetColorGroupData(this.colorGroup);
            if (data != null && data.color == color) return true;
            return false;
        }

        public bool ContainsColor(Color color)
        {
            return this.currentColor == color;
        }

        private void SetCurrentColor(Color color)
        {
            if (this.currentColor == color) this.currentColor = this.defaultColor;
            else this.currentColor = color;
        }

        public void UpdateColor(Color color)
        {
            SetCurrentColor(color);

            if (this.currentColor == this.defaultColor)
            {
                if (this.palette.canRemove) this.mainMenu.playerPreset.RemoveColorGroup(this.colorGroup);
                else this.mainMenu.playerPreset.AddColorGroup(this.colorGroup, this.palette.defaultColor);
            }
            else this.mainMenu.playerPreset.AddColorGroup(this.colorGroup, color);

            this.UpdatePreview();
        }
    }
}
