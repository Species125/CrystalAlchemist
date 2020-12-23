public class CharacterCreatorGear : CharacterCreatorButton
{
    public CharacterCreatorPartProperty property;

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
        return this.handler.GetComponent<CharacterCreatorGearHandler>().FindGear(this.property);
    }

    public override void Click()
    {
        this.handler.GetComponent<CharacterCreatorGearHandler>().UpdateGear(this.property);
        base.Click();
    }
}
