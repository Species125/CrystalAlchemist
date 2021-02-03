using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace CrystalAlchemist
{
    public class SaveGameMenu : MonoBehaviour
    {
        [BoxGroup("Save Menu")]
        [Required]
        [SerializeField]
        private List<SaveSlot> slots = new List<SaveSlot>();

        [BoxGroup("Save Menu")]
        [Required]
        [SerializeField]
        private SavePointInfo savePointInfo;

        [BoxGroup("Save Menu")]
        [SerializeField]
        private PlayerSaveGame saveGame;

        //Called from Dialogbox
        public void UpdateSaves()
        {
            foreach (SaveSlot slot in this.slots) slot.getData();
        }

        public void SaveGame(SaveSlot slot)
        {
            this.saveGame.teleportList.SetNextTeleport(this.savePointInfo.stats); //set as new Spawnpoint
            SaveSystem.Save(this.saveGame, slot.gameObject.name); //saves savegame to data

            UpdateSaves();
        }
    }
}


