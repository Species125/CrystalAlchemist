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

    [System.Serializable]
    public class CharacterPartData
    {
        public string parentName;
        public string name;

        public CharacterPartData(string parent, string name)
        {
            this.parentName = parent;
            this.name = name;
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
        private List<CharacterPartData> characterParts = new List<CharacterPartData>();


        [Button]
        public void SetPreset(CharacterPreset preset) => GameUtil.setPreset(preset, this);


        public Race getRace()
        {
            return this.race;
        }

        public void setRace(Race race)
        {
            if (!this.readOnly) this.race = race;
        }


        #region CharacterPartData

        public CharacterPartData GetCharacterPartData(CharacterCreatorPartProperty property)
        {
            return GetCharacterPartData(property.parentName, property.partName);
        }

        public CharacterPartData GetCharacterPartData(string parentName, string name)
        {
            foreach (CharacterPartData data in this.characterParts)
            {
                if (((data.parentName.ToUpper() == parentName.ToUpper())
                     && (name == null || data.name.ToUpper() == name.ToUpper()))) return data;
            }
            return null;
        }

        public CharacterPartData GetCharacterPartData(string parentName)
        {
            foreach (CharacterPartData characterPartData in this.characterParts)
            {
                if (characterPartData.parentName.ToUpper() == parentName.ToUpper()) return characterPartData;
            }
            return null;
        }

        public List<CharacterPartData> GetCharacterPartDataRange()
        {
            return this.characterParts;
        }


        public void AddCharacterPartData(CharacterPartData data)
        {
            AddCharacterPartData(data.parentName, data.name);
        }

        public void AddCharacterPartData(string parentName, string name)
        {
            if (!this.readOnly)
            {
                CharacterPartData characterPartData = this.GetCharacterPartData(parentName);
                this.characterParts.Remove(characterPartData);

                CharacterPartData newGroup = new CharacterPartData(parentName, name);
                this.characterParts.Add(newGroup);
            }
        }

        public void AddCharacterPartDataRange(List<CharacterPartData> groups)
        {
            if (!this.readOnly)
            {
                this.characterParts.Clear();

                foreach (CharacterPartData group in groups)
                {
                    AddCharacterPartData(group);
                }
            }
        }


        public void RemoveCharacterPartData(string parentName, string name)
        {
            if (!this.readOnly)
            {
                CharacterPartData characterPartData = this.GetCharacterPartData(parentName, name);
                if (characterPartData != null) this.characterParts.Remove(characterPartData);
            }
        }

        public void RemoveCharacterPartData(string parentName)
        {
            if (!this.readOnly)
            {
                CharacterPartData characterPartData = this.GetCharacterPartData(parentName);
                if (characterPartData != null) this.characterParts.Remove(characterPartData);
            }
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