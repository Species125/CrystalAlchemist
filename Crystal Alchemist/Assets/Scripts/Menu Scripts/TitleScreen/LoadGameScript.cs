


using UnityEngine;
using UnityEngine.SceneManagement;

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
            SceneManager.LoadSceneAsync(this.saveGame.teleportList.GetNextTeleport().scene);
        }
    }
}
