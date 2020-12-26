using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System;

public class SaveSystem
{
    public static void Save(PlayerSaveGame saveGame, string slot)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/" + slot + "." + MasterManager.globalValues.saveGameFiletype;
        FileStream stream = new FileStream(path, FileMode.Create);

        PlayerData data = new PlayerData(saveGame);

        formatter.Serialize(stream, data);
        stream.Close();
        stream.Dispose();
    }

    public static void SaveOptions()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/options."+MasterManager.globalValues.saveGameFiletype;
        FileStream stream = new FileStream(path, FileMode.Create);

        GameOptions data = new GameOptions();

        formatter.Serialize(stream, data);
        stream.Close();
        stream.Dispose();
    }

    public static PlayerData loadPlayer(string name)
    {
        if (name != null)
        {
            string path = Application.persistentDataPath + "/" + name + "." + MasterManager.globalValues.saveGameFiletype;

            if (File.Exists(path))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                FileStream stream = new FileStream(path, FileMode.Open);

                if (stream.Length <= 0) return null;

                PlayerData data = formatter.Deserialize(stream) as PlayerData;

                stream.Close();
                stream.Dispose();

                return data;
            }
        }

        return null;
    }

    public static void loadOptions()
    {
        string path = Application.persistentDataPath + "/options." + MasterManager.globalValues.saveGameFiletype;

        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            GameOptions data = formatter.Deserialize(stream) as GameOptions;

            stream.Close();
            stream.Dispose();

            MasterManager.settings.backgroundMusicVolume = data.musicVolume;
            MasterManager.settings.soundEffectVolume = data.soundVolume;

            if (Enum.TryParse(data.language, out Language language)) MasterManager.settings.language = language;
            if (Enum.TryParse(data.layout, out InputDeviceType layoutType)) MasterManager.settings.layoutType = layoutType;

            MasterManager.settings.healthBar = data.useHealthBar;
            MasterManager.settings.manaBar = data.useManaBar;
            MasterManager.settings.cameraDistance = data.cameraDistance;
            MasterManager.settings.UISize = data.uiSize;
            if (MasterManager.settings.UISize < 0.5f) MasterManager.settings.UISize = 0.5f;
        }
    }
}


