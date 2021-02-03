using System.Collections.Generic;

namespace CrystalAlchemist
{
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
        public string[] colorGroups;
        public string[] characterParts;
        public List<string[]> progress = new List<string[]>();

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

            SetInventory(saveGame.inventory);
            SetPreset(saveGame.playerPreset);

            this.abilities = saveGame.buttons.saveButtonConfig();
            this.timePlayed = saveGame.timePlayed.GetValue();
            this.characterName = saveGame.GetCharacterName();

            SetStartTeleport(saveGame.teleportList.GetLatestTeleport());
            SetLastTeleport(saveGame.teleportList.GetReturnTeleport());
            SetTeleportList(saveGame.teleportList);
            SetProgress(saveGame.progress);
        }

        private void SetInventory(PlayerInventory inventory)
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

        private void SetPreset(CharacterPreset preset)
        {
            SerializationUtil.GetPreset(preset, out this.race, out this.colorGroups, out this.characterParts);
        }

        private void SetProgress(PlayerGameProgress progress)
        {
            this.progress = progress.GetProgressRaw();
        }

        private void SetTeleportList(PlayerTeleportList list)
        {
            this.teleportPoints.Clear();

            foreach (TeleportStats stat in list.GetStats()) this.teleportPoints.Add(stat.teleportName);
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
}