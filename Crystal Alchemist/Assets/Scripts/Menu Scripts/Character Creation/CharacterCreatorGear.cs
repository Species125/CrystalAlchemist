

namespace CrystalAlchemist
{
    public class CharacterCreatorGear : CharacterCreatorButton
    {
        public CharacterCreatorPartProperty property;

        private CharacterCreatorGearHandler handler;

        public void SetButton(CharacterCreatorPartProperty property, CharacterCreatorGearHandler handler)
        {
            this.handler = handler;
            this.property = property;

            this.gameObject.name = property.name;
            this.preview.enabled = true;
            this.preview.sprite = property.GetSprite(true);
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
