using UnityEngine;

public class CharacterCreatorSubMenuChild : MonoBehaviour
{
    [SerializeField]
    private Race excludedRace;

    [SerializeField]
    private bool enableColor = true;

    public bool isEnabledByRace(Race race)
    {
        return race != excludedRace;
    }

    public bool isEnabledByGear()
    {
        return this.enableColor;
    }
}
