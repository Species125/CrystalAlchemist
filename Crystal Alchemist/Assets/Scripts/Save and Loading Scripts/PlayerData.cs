﻿using System.Collections.Generic;

[System.Serializable]
public class PlayerData
{
    public float health;
    public float mana;

    public int maxHealth;
    public int maxMana;
    public int healthRegen;
    public int manaRegen;
    public int buffplus;
    public int debuffminus;

    public List<string> keyItems = new List<string>();
    public List<string[]> inventoryItems = new List<string[]>();
    public List<string[]> abilities = new List<string[]>();

    public string characterName;
    public string race;
    public List<string[]> colorGroups = new List<string[]>();
    public List<string[]> characterParts = new List<string[]>();
    public List<string> progress = new List<string>();

    public List<string> teleportPoints = new List<string>();
    public string startTeleport;
    public string lastTeleport;

    public float timePlayed;

    public PlayerData(PlayerSaveGame saveGame)
    {
        this.health = saveGame.playerValue.life;
        this.mana = saveGame.playerValue.mana;

        this.maxHealth = saveGame.attributes.health;
        this.maxMana = saveGame.attributes.mana;
        this.healthRegen = saveGame.attributes.healthRegen;
        this.manaRegen = saveGame.attributes.manaRegen;
        this.buffplus = saveGame.attributes.buffPlus;
        this.debuffminus = saveGame.playerValue.debuffMinus;

        setInventory(saveGame.inventory);
        setPreset(saveGame.playerPreset);

        this.abilities = saveGame.buttons.saveButtonConfig();
        this.timePlayed = saveGame.timePlayed.GetValue();
        this.characterName = saveGame.GetCharacterName();

        SetStartTeleport(saveGame.teleportList.nextTeleport);
        SetLastTeleport(saveGame.teleportList.lastTeleport);
        SetTeleportList(saveGame.teleportList);
        SetProgress(saveGame.progress);
    }

    private void setInventory(PlayerInventory inventory)
    {
        this.keyItems.Clear();
        this.inventoryItems.Clear();

        foreach (ItemGroup item in inventory.inventoryItems)
        {
            string[] temp = new string[2];
            temp[0] = item.name;
            temp[1] = item.GetAmount() + "";
            this.inventoryItems.Add(temp);
        }

        foreach (ItemStats item in inventory.keyItems)
        {
            string temp = item.name;
            this.keyItems.Add(temp);
        }
    }

    private void setPreset(CharacterPreset preset)
    {     
        this.race = preset.getRace().ToString();

        this.colorGroups.Clear();
        this.characterParts.Clear();

        foreach(ColorGroupData data in preset.GetColorGroupRange())
        {
            string[] temp = new string[5];
            temp[0] = data.colorGroup.ToString();
            temp[1] = data.color.r + "";
            temp[2] = data.color.g + "";
            temp[3] = data.color.b + "";
            temp[4] = data.color.a + "";
            this.colorGroups.Add(temp);
        }

        foreach (CharacterPartData data in preset.GetCharacterPartDataRange())
        {
            string[] temp = new string[2];
            temp[0] = data.parentName;
            temp[1] = data.name;
            this.characterParts.Add(temp);
        }
    }

    private void SetProgress(PlayerGameProgress progress)
    {
        this.progress.AddRange(progress.GetPermanent());
    }

    private void SetTeleportList(PlayerTeleportList list)
    {
        this.teleportPoints.Clear();

        foreach(TeleportStats stat in list.GetStats()) this.teleportPoints.Add(stat.teleportName);        
    }

    private void SetStartTeleport(TeleportStats stats)
    {
        if (stats == null) return;
        this.startTeleport = stats.teleportName;
    }

    private void SetLastTeleport(TeleportStats stats)
    {
        if (stats == null) return;
        this.lastTeleport = stats.teleportName;
    }

    public string GetStartTeleportName()
    {
        TeleportStats stats = MasterManager.GetTeleportStats(this.startTeleport);
        if (stats != null) return stats.GetTeleportName();
        return "???";
    }
}

[System.Serializable]
public class GameOptions
{
    public float musicVolume;
    public float soundVolume;

    public string layout;
    public string language;

    public bool useHealthBar;
    public bool useManaBar;

    public int cameraDistance;
    public float uiSize = 1f;

    public GameOptions()
    {
        this.musicVolume = MasterManager.settings.backgroundMusicVolume;
        this.soundVolume = MasterManager.settings.soundEffectVolume;

        this.layout = MasterManager.settings.layoutType.ToString().ToLower();
        this.language = MasterManager.settings.language.ToString();

        this.useHealthBar = MasterManager.settings.healthBar;
        this.useManaBar = MasterManager.settings.manaBar;

        this.cameraDistance = MasterManager.settings.cameraDistance;
        this.uiSize = MasterManager.settings.UISize;
    }
}