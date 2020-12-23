using UnityEngine;
using UnityEngine.UI;

public class CharacterCreatorColor : CharacterCreatorButton
{
    private Color color;

    public void SetButton(Color color, CharacterCreatorColorPaletteHandler handler)
    {
        this.handler = handler;
        this.color = color;
        this.preview.color = this.color;
    }

    public override bool IsSelected()
    {
        CharacterCreatorColorPaletteHandler colorHandler = this.handler.GetComponent<CharacterCreatorColorPaletteHandler>();
        if (colorHandler.HasColor(this.color)) return true;

        return false;
    }

    public override void Click()
    {
        //TODO: Swap
        this.handler.GetComponent<CharacterCreatorColorPaletteHandler>().UpdateColor(this.color);
        base.Click();
    }
}
