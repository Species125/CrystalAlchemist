using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/CharacterCreation/Property Group")]
public class CharacterCreatorPropertyGroup : ScriptableObject
{   
    public List<CharacterCreatorPartProperty> properties = new List<CharacterCreatorPartProperty>();
}
