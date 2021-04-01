using UnityEngine;

namespace CrystalAlchemist 
{
    public class Spielwiese : MonoBehaviour
    {
        public PlayerSaveGame saveGame;

        private void Awake()
        {
            saveGame.Clear(null, "Slot 0");
        }
    }
}
