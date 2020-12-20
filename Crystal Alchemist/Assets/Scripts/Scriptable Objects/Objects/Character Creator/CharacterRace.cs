using AssetIcons;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/CharacterCreation/Character Race")]
public class CharacterRace : ScriptableObject
{
    public Race race;

    public string raceName;

    [AssetIcon]
    public Sprite icon;
}
