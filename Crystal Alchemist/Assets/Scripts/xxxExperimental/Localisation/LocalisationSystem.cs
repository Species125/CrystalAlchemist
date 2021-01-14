using System;
using System.IO;


namespace CrystalAlchemist
{
    public static class LocalisationSystem
    {
        public static bool isInit;

        public static LocalisationData german;
        public static LocalisationData english;

        public static void Init()
        {
            LoadFiles();
            isInit = true;
        }

        public static string GetLocalisedValue(string key, LocalisationFileType type)
        {
            if (!isInit) Init();

            switch (MasterManager.settings.language)
            {
                case Language.English: return english.GetLocalisedValue(key, type);
                case Language.German: return german.GetLocalisedValue(key, type);
            }

            return "";
        }

        public static void LoadFiles()
        {
            german = new LocalisationData(Language.German);
            english = new LocalisationData(Language.English);
        }

        private static void WriteLine(string path, string line)
        {
            if (File.Exists(path)) File.AppendAllText(path, line);
        }

        private static void WriteLine(string path, string key, string ger, string eng)
        {
            string line = string.Format("\"{0}\",\"{1}\",\"{2}\",", key, ger, eng) + Environment.NewLine;
            if (File.Exists(path)) File.AppendAllText(path, line);
        }
    }
}
