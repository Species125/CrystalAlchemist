

namespace CrystalAlchemist
{
    public class CharacterCreatorGear : CharacterCreatorButton
    {
        public CharacterCreatorProperty property;

        private CharacterCreatorGearHandler handler;

        public void SetButton(CharacterCreatorProperty property, CharacterCreatorGearHandler handler)
        {
            this.handler = handler;
            this.property = property;

            this.gameObject.name = property.name;
            this.preview.enabled = true;
            this.preview.sprite = property.GetSprite();
        }

        public override bool IsSelected()
        {
            return this.handler.ContainsGear(this.property);
        }

        public override void Click()
        {
            this.handler.UpdateGear(this.property);
        }
    }
}
