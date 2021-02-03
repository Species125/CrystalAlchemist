using UnityEngine;

namespace CrystalAlchemist
{
    public class CharacterCreatorColor : CharacterCreatorButton
    {
        private Color color;

        private CharacterCreatorColorPaletteHandler handler;

        public void SetButton(Color color, CharacterCreatorColorPaletteHandler handler)
        {
            this.handler = handler;
            this.color = color;
            this.preview.color = this.color;
        }

        public override bool IsSelected()
        {
            if (this.handler.ContainsColor(this.color)) return true;

            return false;
        }

        public override void Click()
        {
            this.handler.UpdateColor(this.color);
        }
    }
}
