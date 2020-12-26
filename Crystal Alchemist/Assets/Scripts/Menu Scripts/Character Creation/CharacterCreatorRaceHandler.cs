using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class CharacterCreatorRaceHandler : CharacterCreatorButtonHandler
{
    [SerializeField]
    private CharacterCreatorPropertyGroup raceGroup;

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
        if (race == this.mainMenu.playerPreset.getRace()) return true;
        return false;
    }

    public bool ContainsRace(Race race)
    {
        return this.currentRace == race;
    }

    private void UpdateRace()
    {
        List<CharacterCreatorGear> gearButtons = new List<CharacterCreatorGear>();
        UnityUtil.GetChildObjects<CharacterCreatorGear>(this.transform, gearButtons);

        foreach (CharacterCreatorPartProperty part in this.raceGroup.properties)
        {
            CharacterPartData data = this.mainMenu.playerPreset.GetCharacterPartData(part.parentName, part.partName);
            bool enableIt = part.enableIt(this.mainMenu.playerPreset.getRace(), data);

            if (enableIt) this.mainMenu.playerPreset.AddCharacterPartData(part.parentName, part.partName);
            else this.mainMenu.playerPreset.RemoveCharacterPartData(part.parentName, part.partName);

            //check if colorgroup exists
        }
    }

    public void UpdateRace(Race race)
    {
        this.mainMenu.playerPreset.setRace(race);
        this.UpdateRace();
        this.UpdatePreview();
    }
}
