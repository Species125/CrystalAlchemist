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
            GameEvents.current.OnKeyItemExists += HasKeyItemAlready;
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
            GameEvents.current.OnKeyItemExists -= HasKeyItemAlready;
        }

        private void SaveGame(bool value)
        {
            if (value) this.saveIt = true;
            else SaveDirectly();
        }

        private bool HasKeyItemAlready(ScriptableObject scriptableObject)
        {
            if (scriptableObject.GetType() == typeof(InventoryItem))
            {
                if (this.saveGame.inventory == null) return false;

                InventoryItem inventoryItem = (InventoryItem)scriptableObject;
                return this.saveGame.inventory.KeyItemExists(inventoryItem);
            }
            else if(scriptableObject.GetType() == typeof(Ability))
            {
                if (this.saveGame.skillSet == null) return false;

                Ability ability = (Ability)scriptableObject;
                return this.saveGame.skillSet.Exists(ability);
            }
            else if (scriptableObject.GetType() == typeof(CharacterCreatorProperty))
            {
                if (this.saveGame.outfits == null) return false;

                CharacterCreatorProperty property = (CharacterCreatorProperty)scriptableObject;
                return this.saveGame.outfits.GearExists(property);
            }

            return false;
        }
    }
}
