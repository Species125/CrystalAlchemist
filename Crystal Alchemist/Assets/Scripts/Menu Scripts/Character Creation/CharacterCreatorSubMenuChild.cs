using System.Collections.Generic;
using UnityEngine;

public class CharacterCreatorSubMenuChild : MonoBehaviour
{
    [SerializeField]
    private RaceRestriction restriction = RaceRestriction.exclude;

    [SerializeField]
    private List<Race> races = new List<Race>();

    public bool isEnabledByRace(Race race)
    {
        if (this.restriction == RaceRestriction.exclude && !this.races.Contains(race)) return true;
        else if (this.restriction == RaceRestriction.include && this.races.Contains(race)) return true;
        return false;
    }
}
