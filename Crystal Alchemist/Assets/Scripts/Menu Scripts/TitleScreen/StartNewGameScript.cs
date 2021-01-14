

using UnityEngine;
using UnityEngine.SceneManagement;

namespace CrystalAlchemist
{
    public class StartNewGameScript : MonoBehaviour
    {
        [SerializeField]
        private PlayerSaveGame saveGame;

        [SerializeField]
        private TeleportStats startTeleport;

        public void StartNewGame()
        {
            //Cursor.visible = false;
            this.saveGame.teleportList.SetNextTeleport(this.startTeleport);
            SceneManager.LoadSceneAsync(this.saveGame.teleportList.GetNextTeleport().scene);
        }
    }
}
