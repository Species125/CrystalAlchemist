using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace CrystalAlchemist
{
    [System.Serializable]
    public class OutfitData
    {
        public bool isDefault = false;
        public CharacterCreatorProperty property;

        public OutfitData(CharacterCreatorProperty property, bool isDefault = false)
        {
            this.isDefault = isDefault;
            this.property = property;
        }
    }

    [CreateAssetMenu(menuName = "Game/CharacterCreation/Property Group")]
    public class CharacterCreatorPropertyGroup : ScriptableObject
    {
        [BoxGroup]
        [SerializeField]
        [TextArea]
        private string note;

        [BoxGroup]
        public List<BodyPart> type = new List<BodyPart>();

        [BoxGroup]
        public bool canRemove = true;

        [BoxGroup]
        public List<OutfitData> outfits = new List<OutfitData>();

        public void Clear()
        {
            this.outfits.RemoveAll(items => !items.isDefault);
        }

        public void AddOutfit(CharacterCreatorProperty property)
        {
            if (!PropertyExists(property)) this.outfits.Add(new OutfitData(property, false));
        }

        public List<CharacterCreatorProperty> GetProperties()
        {
            List<CharacterCreatorProperty> result = new List<CharacterCreatorProperty>();
            foreach (OutfitData data in this.outfits) result.Add(data.property);
            return result;
        }

        public bool PropertyExists(CharacterCreatorProperty property)
        {
            foreach (OutfitData data in this.outfits)
            {
                if (property.name == data.property.name) return true;
            }

            return false;
        }

        public List<string[]> GetOutfits()
        {
            List<string[]> result = new List<string[]>();

            foreach (OutfitData data in this.outfits)
            {
                if (!data.isDefault) result.Add(new string[] { data.property.type.ToString(), data.property.name });
            }

            return result;
        }
    }
}
