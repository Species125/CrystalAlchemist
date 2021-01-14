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

        public static string[] CharacterPartsToArray(List<CharacterPartData> datas)
        {
            List<string> list = new List<string>();
            foreach (CharacterPartData data in datas)
            {
                string result = data.parentName + ":" + data.name;
                list.Add(result);
            }
            return list.ToArray();
        }

        public static List<CharacterPartData> ArrayToCharacterPartData(string[] datas)
        {
            List<CharacterPartData> parts = new List<CharacterPartData>();

            foreach (string data in datas)
            {
                string[] characterPart = data.Split(':');
                string parentName = characterPart[0];
                string name = characterPart[1];

                parts.Add(new CharacterPartData(parentName, name));
            }
            return parts;
        }

        public static void SetPreset(CharacterPreset preset, string race, string[] colorGroupsArray, string[] characterPartsArray)
        {
            preset.setRace(StringToRace(race));

            List<ColorGroupData> colorGroups = ArrayToColorGroups(colorGroupsArray);
            preset.AddColorGroupRange(colorGroups);

            List<CharacterPartData> parts = ArrayToCharacterPartData(characterPartsArray);
            preset.AddCharacterPartDataRange(parts);
        }

        public static void GetPreset(CharacterPreset preset, out string race, out string[] colorGroups, out string[] characterParts)
        {
            race = RaceToString(preset.getRace());
            colorGroups = ColorGroupsToArray(preset.GetColorGroupRange());
            characterParts = CharacterPartsToArray(preset.GetCharacterPartDataRange());
        }
    }
}
