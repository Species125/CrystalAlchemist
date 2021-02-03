using UnityEngine;

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
            GameEvents.current.DoChangeScene(this.saveGame.teleportList.GetLatestTeleport().scene);
        }
    }
}
