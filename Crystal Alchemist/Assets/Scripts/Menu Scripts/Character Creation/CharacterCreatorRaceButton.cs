using TMPro;
using UnityEngine.UI;
using UnityEngine;

public class CharacterCreatorRaceButton : CharacterCreatorButton
{
    [SerializeField]
    private TMP_Text textField;

    [SerializeField]
    private Image image;

    private Race race;

    public void SetRace(CharacterRace race)
    {
        this.textField.text = race.raceName;
        this.image.sprite = race.icon;
        this.race = race.race;
    }

    public override void Click()
    {
        this.mainMenu.creatorPreset.setRace(this.race);
        this.mainMenu.updateGear();
        base.Click();
    }
}
