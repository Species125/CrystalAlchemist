using TMPro;
using UnityEngine;

public class CharacterCreatorRaceButton : CharacterCreatorButton
{
    [SerializeField]
    private TMP_Text textField;

    private Race race;

    public override bool IsSelected()
    {
        return this.handler.GetComponent<CharacterCreatorRaceHandler>().IsRace(this.race);
    }

    public void SetButton(CharacterRace race, CharacterCreatorRaceHandler handler)
    {
        this.handler = handler;
        this.textField.text = race.raceName;
        this.preview.sprite = race.icon;
        this.race = race.race;
    }

    public override void Click()
    {
        this.handler.GetComponent<CharacterCreatorRaceHandler>().UpdateRace(this.race);
        base.Click();
    }
}
