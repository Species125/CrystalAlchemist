using UnityEngine;

namespace CrystalAlchemist
{
    public class SaveGameManager : MonoBehaviour
    {
        [SerializeField]
        private PlayerSaveGame saveGame;

        [SerializeField]
        [Tooltip("Saving in minutes")]
        private float saveInterval = 60;

        [SerializeField]
        [Tooltip("Saving in minutes")]
        private float saveDelay = 0.1f;

        private bool saveIt = false;

        private void Awake()
        {
            GameEvents.current.OnSaveGame += SaveGame;
            GameEvents.current.OnKeyItem += HasKeyItemAlready;
        }

        private void Start()
        {
            GameEvents.current.DoSaveGame(false);
            InvokeRepeating("Updating", this.saveDelay, this.saveInterval);
        }

        private void Updating()
        {
            if (this.saveIt) SaveDirectly();
        }

        private void SaveDirectly()
        {
            this.saveGame.SaveGame();
            this.saveIt = false;
            Debug.Log("Saved");
        }

        private void OnDestroy()
        {
            GameEvents.current.OnSaveGame -= SaveGame;
            GameEvents.current.OnKeyItem -= HasKeyItemAlready;
        }

        private void SaveGame(bool value)
        {
            if (value) this.saveIt = true;
            else SaveDirectly();
        }

        private bool HasKeyItemAlready(string name)
        {
            if (this.saveGame.inventory == null) return false;
            foreach (ItemStats elem in this.saveGame.inventory.keyItems) if (elem != null && name == elem.name) return true;
            return false;
        }
    }
}
