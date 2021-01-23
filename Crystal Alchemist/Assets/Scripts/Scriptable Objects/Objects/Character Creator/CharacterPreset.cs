using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace CrystalAlchemist
{
    public enum Race
    {
        human,
        elf,
        goblin,
        catgirl,
        pony,
        unicorn,
        drahn,
        lamia,
        machina
    }

    public enum ColorGroup
    {
        hairstyle,
        eyes,
        skin,
        scales,
        faceGear,
        headGear,
        underwear,
        upperGear,
        lowerGear,
        backGear,
        none
    }

    [System.Serializable]
    public class ColorGroupData
    {
        public ColorGroup colorGroup;
        public Color color = Color.white;

        public ColorGroupData(ColorGroup colorGroup, Color color)
        {
            this.colorGroup = colorGroup;
            this.color = color;
        }
    }

    [CreateAssetMenu(menuName = "Game/CharacterCreation/Character Preset")]
    public class CharacterPreset : ScriptableObject
    {
        [SerializeField]
        private bool readOnly = false;

        [SerializeField]
        private Race race;

        [SerializeField]
        private List<ColorGroupData> colorGroups = new List<ColorGroupData>();

        [SerializeField]
        private List<CharacterCreatorProperty> properties = new List<CharacterCreatorProperty>();

        [Button]
        public void SetPreset(CharacterPreset preset) => GameUtil.SetPreset(preset, this);
       

        public void Clear() => this.properties.RemoveAll(item => item == null);

        public Race getRace()
        {
            return this.race;
        }

        public void setRace(Race race)
        {
            if (!this.readOnly) this.race = race;
        }

        public CharacterCreatorProperty GetProperty(BodyPart type)
        {
            this.properties.RemoveAll(item => item == null);

            foreach (CharacterCreatorProperty property in this.properties)
            {
                if (property.type == type) return property;
            }
            return null;
        }


        #region CharacterPartData
        
        public List<CharacterCreatorProperty> GetProperties()
        {
            return this.properties;
        }

        public bool ContainsProperty(CharacterCreatorProperty property)
        {
            return this.properties.Contains(property);
        }

        public void AddProperty(List<CharacterCreatorProperty> properties)
        {
            this.properties.RemoveAll(item => item == null);

            if (this.readOnly) return;

            this.properties.Clear();
            foreach (CharacterCreatorProperty property in properties) AddProperty(property);
        }

        public void AddProperty(CharacterCreatorProperty property)
        {
            if (this.readOnly) return;

            CharacterCreatorProperty propertyOfSameType = GetPropertyOfSameType(property);
            this.properties.Remove(propertyOfSameType);

            this.properties.Add(property);
        }

        public void RemoveProperty(CharacterCreatorProperty property)
        {
            if (this.readOnly) return;

            if (!property) this.properties.Remove(property);
        }

        public CharacterCreatorProperty GetPropertyOfSameType(CharacterCreatorProperty property)
        {
            if (property == null) return null;
            this.properties.RemoveAll(item => item == null);

            foreach (CharacterCreatorProperty prop in this.properties)
            {
                if (property.type == prop.type) return prop;
            }

            return null;
        }

        #endregion


        #region ColorGroups

        public ColorGroupData GetColorGroupData(ColorGroup colorGroup)
        {
            foreach (ColorGroupData colorGroupData in this.colorGroups)
            {
                if (colorGroupData.colorGroup == colorGroup) return colorGroupData;
            }
            return null;
        }

        public List<ColorGroupData> GetColorGroupRange()
        {
            return this.colorGroups;
        }


        public void AddColorGroup(ColorGroup colorGroup, Color color)
        {
            if (!this.readOnly)
            {
                ColorGroupData colorGroupData = this.GetColorGroupData(colorGroup);
                this.colorGroups.Remove(colorGroupData);

                ColorGroupData newGroup = new ColorGroupData(colorGroup, color);
                this.colorGroups.Add(newGroup);
            }
        }

        public void AddColorGroup(ColorGroupData data)
        {
            AddColorGroup(data.colorGroup, data.color);
        }

        public void AddColorGroupRange(List<ColorGroupData> groups)
        {
            if (!this.readOnly)
            {
                this.colorGroups.Clear();

                foreach (ColorGroupData group in groups)
                {
                    AddColorGroup(group);
                }
            }
        }

        public void RemoveColorGroup(ColorGroup colorGroup)
        {
            if (!this.readOnly)
            {
                ColorGroupData colorGroupData = this.GetColorGroupData(colorGroup);
                if (colorGroupData != null) this.colorGroups.Remove(colorGroupData);
            }
        }

        public List<Color> getColors(List<ColorTable> tables)
        {
            List<Color> colors = new List<Color>();

            foreach (ColorTable table in tables)
            {
                foreach (ColorGroupData data in this.colorGroups)
                {
                    if (data.colorGroup == table.colorGroup) colors.Add(data.color);
                }
            }
            return colors;
        }


        public Color getColor(ColorGroup colorGroup)
        {
            foreach (ColorGroupData data in this.colorGroups)
            {
                if (data.colorGroup == colorGroup) return data.color;
            }
            return Color.white;
        }

        #endregion
    }
}