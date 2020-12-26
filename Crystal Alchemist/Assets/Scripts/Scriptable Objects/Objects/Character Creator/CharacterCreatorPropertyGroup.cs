using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(menuName = "Game/CharacterCreation/Property Group")]
public class CharacterCreatorPropertyGroup : ScriptableObject
{
    [BoxGroup]
    public bool canRemove = true;

    [BoxGroup]
    public List<CharacterCreatorPartProperty> properties = new List<CharacterCreatorPartProperty>();
}
