using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCreatorRace : MonoBehaviour
{
    [SerializeField]
    private CharacterCreatorRaceButton template;

    [SerializeField]
    private GameObject content;

    [SerializeField]
    private List<CharacterRace> races = new List<CharacterRace>();

    private void Start()
    {
        this.template.gameObject.SetActive(false);

        for (int i = 0; i < races.Count; i++)
        {
            CharacterCreatorRaceButton newButton = Instantiate(template, this.content.transform);
            newButton.SetRace(races[i]);
            newButton.name = "Item " + i + ":" + races[i].raceName;

            newButton.gameObject.SetActive(true);
            if (i == 0)
            {
                newButton.GetComponent<ButtonExtension>().SetAsFirst();
                newButton.GetComponent<ButtonExtension>().ReSelect();
            }
        }
    }
}
