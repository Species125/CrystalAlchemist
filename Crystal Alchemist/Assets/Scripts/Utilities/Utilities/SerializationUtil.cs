using System;
using System.Collections.Generic;

using UnityEngine;

namespace CrystalAlchemist
{
    public static class SerializationUtil
    {
        public static string RaceToString(Race race)
        {
            return race.ToString();
        }

        public static Race StringToRace(string strRace)
        {
            if (Enum.TryParse(strRace, out Race race)) return race;
            return Race.human;
        }

        public static string[] ColorGroupsToArray(List<ColorGroupData> datas)
        {
            List<string> list = new List<string>();
            foreach (ColorGroupData data in datas)
            {
                string result = data.colorGroup.ToString()
                                + ":" + data.color.r
                                + ":" + data.color.g
                                + ":" + data.color.b
                                + ":" + data.color.a;
                list.Add(result);
            }
            return list.ToArray();
        }

        public static List<ColorGroupData> ArrayToColorGroups(string[] datas)
        {
            List<ColorGroupData> colorGroups = new List<ColorGroupData>();

            foreach (string data in datas)
            {
                string[] colorGroup = data.Split(':');

                string colorGroupName = colorGroup[0];
                float r = float.Parse(colorGroup[1]);
                float g = float.Parse(colorGroup[2]);
                float b = float.Parse(colorGroup[3]);
                float a = float.Parse(colorGroup[4]);

                Color color = new Color(r, g, b, a);
                if (Enum.TryParse(colorGroup[0], out ColorGroup group)) colorGroups.Add(new ColorGroupData(group, color));
            }

            return colorGroups;
        }

        public static string[] PlayerSpritesToArray(List<CharacterCreatorProperty> properties)
        {
            List<string> list = new List<string>();
            foreach (CharacterCreatorProperty property in properties) list.Add(property.path);            
            return list.ToArray();
        }

        public static List<CharacterCreatorProperty> ArrayToPlayerSprites(string[] paths)
        {
            List<CharacterCreatorProperty> parts = new List<CharacterCreatorProperty>();

            foreach (string path in paths)
            {
                CharacterCreatorProperty result = Resources.Load<CharacterCreatorProperty>(path);
                parts.Add(result);
            }
            return parts;
        }

        public static void SetPreset(CharacterPreset preset, string race, string[] colorGroupsArray, string[] characterPartsArray)
        {
            preset.setRace(StringToRace(race));

            List<ColorGroupData> colorGroups = ArrayToColorGroups(colorGroupsArray);
            preset.AddColorGroupRange(colorGroups);

            List<CharacterCreatorProperty> parts = ArrayToPlayerSprites(characterPartsArray);
            preset.AddProperty(parts);
        }

        public static void GetPreset(CharacterPreset preset, out string race, out string[] colorGroups, out string[] characterParts)
        {
            race = RaceToString(preset.getRace());
            colorGroups = ColorGroupsToArray(preset.GetColorGroupRange());
            characterParts = PlayerSpritesToArray(preset.GetProperties());
        }
    }
}
