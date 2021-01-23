using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace CrystalAlchemist
{
    [CreateAssetMenu(menuName = "Game/CharacterCreation/Property Group")]
    public class CharacterCreatorPropertyGroup : ScriptableObject
    {
        [BoxGroup]
        [SerializeField]
        [TextArea]
        private string note;

        [BoxGroup]
        public bool canRemove = true;

        [BoxGroup]
        public List<CharacterCreatorProperty> properties = new List<CharacterCreatorProperty>();
    }
}
