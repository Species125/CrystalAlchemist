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

    private void Awake()
    {
        this.template.gameObject.SetActive(false);

        for (int i = 0; i < races.Count; i++)
        {
            CharacterCreatorRaceButton newButton = Instantiate(template, this.content.transform);
            newButton.SetButton(races[i], this);
            newButton.name = "Item " + i + ":" + races[i].raceName;

            newButton.gameObject.SetActive(true);
            if (i == 0)
            {
                newButton.GetComponent<ButtonExtension>().SetAsFirst();
                newButton.GetComponent<ButtonExtension>().ReSelect();
            }

            this.buttons.Add(newButton);
        }
    }

    public bool IsRace(Race race)
    {
        if (race == this.mainMenu.creatorPreset.getRace()) return true;
        return false;
    }

    public void UpdateRace(Race race)
    {
        this.mainMenu.creatorPreset.setRace(race);
        this.mainMenu.updateGear();
    }
}
