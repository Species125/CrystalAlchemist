using System.Collections.Generic;
using UnityEngine;

public class CharacterCreatorRaceHandler : CharacterCreatorButtonHandler
{
    [SerializeField]
    private CharacterCreatorRaceButton template;

    [SerializeField]
    private GameObject content;

    [SerializeField]
    private List<CharacterRace> races = new List<CharacterRace>();

    private Race currentRace;

    private void Awake()
    {
        for (int i = 0; i < races.Count; i++)
        {
            CharacterRace race = races[i];
            CreateButton(race, i);

            if (IsRace(race.race)) this.currentRace = race.race;
        }

        Destroy(this.template.gameObject);
    }

    private void CreateButton(CharacterRace race, int i)
    {
        CharacterCreatorRaceButton newButton = Instantiate(template, this.content.transform);
        newButton.SetButton(race, this);
        newButton.name = races[i].raceName;

        newButton.gameObject.SetActive(true);
        SetFirst(newButton, i);

        this.buttons.Add(newButton);
    }

    private bool IsRace(Race race)
    {
        if (race == this.mainMenu.creatorPreset.getRace()) return true;
        return false;
    }

    public bool ContainsRace(Race race)
    {
        return this.currentRace == race;
    }

    public void UpdateRace(Race race)
    {
        this.mainMenu.creatorPreset.setRace(race);
        this.mainMenu.updateGear();
        this.UpdatePreview();
    }
}
