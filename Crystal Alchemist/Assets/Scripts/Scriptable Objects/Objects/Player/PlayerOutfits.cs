using System.Collections.Generic;
using UnityEngine;

namespace CrystalAlchemist
{
    [CreateAssetMenu(menuName = "Game/Player/Player Outfits")]
    public class PlayerOutfits : ScriptableObject
    {
        [SerializeField]
        private List<CharacterCreatorPropertyGroup> outfits = new List<CharacterCreatorPropertyGroup>();        

        public void Clear()
        {
            foreach (CharacterCreatorPropertyGroup group in this.outfits) group.Clear();
        }

        public void AddGear(CharacterCreatorProperty property)
        {
            foreach (CharacterCreatorPropertyGroup group in this.outfits)
            {
                if (group.type.Contains(property.type)) group.AddOutfit(property);
            }
        }

        public bool GearExists(CharacterCreatorProperty property)
        {
            foreach (CharacterCreatorPropertyGroup group in this.outfits)
            {
                if (group.type.Contains(property.type) && group.PropertyExists(property)) return true;
            }

            return false;
        }

        public List<string[]> GetOutfits()
        {
            List<string[]> skillset = new List<string[]>();
            foreach (CharacterCreatorPropertyGroup group in this.outfits)
            {
                skillset.AddRange(group.GetOutfits());
            }
            return skillset;
        }
    }
}
