using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace CrystalAlchemist
{
    [CreateAssetMenu(menuName = "Game/Player/Save Game")]
    public class PlayerSaveGame : ScriptableObject
    {
        [BoxGroup("Time")]
        public FloatValue timePlayed;
        [BoxGroup("Time")]
        public TimeValue time;

        [BoxGroup("Player")]
        public StringValue characterName;
        [BoxGroup("Player")]
        public CharacterPreset playerPreset;
        [BoxGroup("Player")]
        public CharacterValues playerValue;
        [BoxGroup("Player")]
        public PlayerInventory inventory;
        [BoxGroup("Player")]
        public PlayerButtons buttons;
        [BoxGroup("Player")]
        public PlayerSkillset skillSet;
        [BoxGroup("Player")]
        public PlayerAttributes attributes;
        [BoxGroup("Player")]
        public PlayerGameProgress progress;

        [BoxGroup("Teleport")]
        public PlayerTeleportList teleportList;

        [BoxGroup("Stats")]
        public CharacterStats stats;

        [BoxGroup("Stats")]
        [SerializeField]
        private CharacterPreset defaultPreset;

        [BoxGroup("Misc")]
        [SerializeField]
        private StringValue currentScene;

        [BoxGroup("Misc")]
        [SerializeField]
        private StringValue teleportPath;

        [BoxGroup("Misc")]
        [SerializeField]
        private MenuDialogBoxInfo dialogBoxInfo;

        [BoxGroup("Misc")]
        [ReadOnly]
        [SerializeField]
        private string slotName;

        public void Clear(Action callback, string slotName)
        {
            Clear();
            this.slotName = slotName;
            callback?.Invoke();
        }

        private void Clear()
        {
            this.playerPreset.Clear();
            this.time.Clear();
            this.timePlayed.SetValue(0f);
            this.characterName.SetValue("Hero");
            this.playerValue.Clear(this.stats);
            this.inventory.Clear();
            this.buttons.Clear();
            this.skillSet.Clear();
            this.attributes.Clear();
            this.progress.Clear();
            this.currentScene.SetValue("");
            this.teleportPath.SetValue("");
            this.dialogBoxInfo.Clear();
            this.teleportList.Clear();

            GameUtil.SetPreset(this.defaultPreset, this.playerPreset);

            Debug.Log("Savegame cleared");

            UnityUtil.ThrowException("Attributes not empty", this, this.attributes.pointsSpent > 0);
            UnityUtil.ThrowException("Progress not empty", this, this.progress.GetAmount() > 0);
        }

        public void SetCharacterName(string characterName) => this.characterName.SetValue(characterName);

        public string GetCharacterName() { return this.characterName.GetValue(); }
        
        public void SaveGame() => SaveSystem.Save(this, this.slotName);         
    }
}
