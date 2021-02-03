using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

namespace CrystalAlchemist
{
    public enum Language
    {
        German,
        English
    }

    public enum LocalisationFileType
    {
        skills,
        items,
        characters,
        dialogs,
        menues,
        statuseffects,
        objects,
        minigames,
        maps,
        tutorials,
        patchnotes
    }

    public class LocalisationData
    {
        public Dictionary<string, string> skills;
        public Dictionary<string, string> items;
        public Dictionary<string, string> characters;
        public Dictionary<string, string> dialogs;
        public Dictionary<string, string> menues;
        public Dictionary<string, string> maps;
        public Dictionary<string, string> statuseffects;
        public Dictionary<string, string> objects;
        public Dictionary<string, string> minigames;
        public Dictionary<string, string> tutorials;
        public Dictionary<string, string> patchNotes;

        private char lineSeperator = '\n';
        private char surround = '"';
        private string[] fieldSeperator = { "\",\"" };
        public Language language;

        public LocalisationData(Language language)
        {
            this.language = language;
            string attributeID = GetLanguageID();

            skills = GetDictionaryValues(attributeID, LocalisationFileType.skills);
            items = GetDictionaryValues(attributeID, LocalisationFileType.items);
            characters = GetDictionaryValues(attributeID, LocalisationFileType.characters);
            dialogs = GetDictionaryValues(attributeID, LocalisationFileType.dialogs);
            menues = GetDictionaryValues(attributeID, LocalisationFileType.menues);
            maps = GetDictionaryValues(attributeID, LocalisationFileType.maps);
            statuseffects = GetDictionaryValues(attributeID, LocalisationFileType.statuseffects);
            objects = GetDictionaryValues(attributeID, LocalisationFileType.objects);
            minigames = GetDictionaryValues(attributeID, LocalisationFileType.minigames);
            tutorials = GetDictionaryValues(attributeID, LocalisationFileType.tutorials);
            patchNotes = GetDictionaryValues(attributeID, LocalisationFileType.patchnotes);
        }

        public string GetLocalisedValue(string key, LocalisationFileType type)
        {
            string value = key;

            switch (type)
            {
                case LocalisationFileType.skills: this.skills.TryGetValue(key, out value); break;
                case LocalisationFileType.items: this.items.TryGetValue(key, out value); break;
                case LocalisationFileType.dialogs: this.dialogs.TryGetValue(key, out value); break;
                case LocalisationFileType.characters: this.characters.TryGetValue(key, out value); break;
                case LocalisationFileType.menues: this.menues.TryGetValue(key, out value); break;
                case LocalisationFileType.maps: this.maps.TryGetValue(key, out value); break;
                case LocalisationFileType.statuseffects: this.statuseffects.TryGetValue(key, out value); break;
                case LocalisationFileType.objects: this.objects.TryGetValue(key, out value); break;
                case LocalisationFileType.minigames: this.minigames.TryGetValue(key, out value); break;
                case LocalisationFileType.tutorials: this.tutorials.TryGetValue(key, out value); break;
                case LocalisationFileType.patchnotes: this.patchNotes.TryGetValue(key, out value); break;
            }

            return value;
        }

        private string GetLanguageID()
        {
            if (this.language == Language.English) return "en";
            return "de";
        }

        private Dictionary<string, string> GetDictionaryValues(string attributeID, LocalisationFileType type)
        {
            TextAsset file = Resources.Load<TextAsset>("Data/Localisation/" + type.ToString());
            Dictionary<string, string> dictionary = new Dictionary<string, string>();

            string[] lines = file.text.Split(lineSeperator);

            int attributeIndex = 1;
            string[] headers = lines[0].Split(fieldSeperator, StringSplitOptions.None);

            for (int i = 0; i < headers.Length; i++)
            {
                if (headers[i].Contains(attributeID))
                {
                    attributeIndex = i;
                    break;
                }
            }

            Regex CSVParser = new Regex(",(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))");

            for (int i = 1; i < lines.Length; i++)
            {
                string line = lines[i];
                string[] fields = CSVParser.Split(line);

                for (int f = 0; f < fields.Length; f++)
                {
                    fields[f] = fields[f].TrimStart(' ', surround);
                    fields[f] = fields[f].TrimEnd(surround);
                }

                if (fields.Length > attributeIndex)
                {
                    var key = fields[0];

                    if (dictionary.ContainsKey(key)) { continue; }
                    var value = fields[attributeIndex].Replace("<br>", Environment.NewLine);
                    dictionary.Add(key, value);
                }
            }

            return dictionary;
        }
    }
}