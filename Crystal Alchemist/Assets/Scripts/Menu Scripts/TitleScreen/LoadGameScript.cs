using UnityEngine;

namespace CrystalAlchemist
{
    public class LoadGameScript : MonoBehaviour
    {
        [SerializeField]
        private PlayerSaveGame saveGame;

        public void LoadGame(SaveSlot slot)
        {
            if (slot != null && slot.data != null)
            {
                //Cursor.visible = false;
                LoadSystem.loadPlayerData(this.saveGame, slot.data, AfterLoad); //load from data into savegame         
            }
        }

        private void AfterLoad()
        {
            GameEvents.current.DoChangeScene(this.saveGame.teleportList.GetLatestTeleport().scene);
        }
    }
}
