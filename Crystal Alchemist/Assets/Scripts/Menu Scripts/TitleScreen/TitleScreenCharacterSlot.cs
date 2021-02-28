using Sirenix.OdinInspector;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CrystalAlchemist
{
    public class TitleScreenCharacterSlot : MonoBehaviour
    {
        [Required]
        [SerializeField]
        [BoxGroup("Main")]
        private string slotName;

        [Required]
        [SerializeField]
        [BoxGroup("Main")]
        private GameObject newGame;

        [Required]
        [SerializeField]
        [BoxGroup("Main")]
        private GameObject loadGame;

        [Required]
        [BoxGroup("Easy Access")]
        [SerializeField]
        private TextMeshProUGUI characterName;

        [Required]
        [BoxGroup("Easy Access")]
        [SerializeField]
        private TextMeshProUGUI race;

        [Required]
        [BoxGroup("Easy Access")]
        [SerializeField]
        private TextMeshProUGUI slotNameField;

        [Required]
        [BoxGroup("Easy Access")]
        [SerializeField]
        private TextMeshProUGUI characterLocation;

        [Required]
        [BoxGroup("Easy Access")]
        [SerializeField]
        private TextMeshProUGUI timePlayed;

        [Required]
        [BoxGroup("Easy Access")]
        [SerializeField]
        private GameObject health;

        [Required]
        [BoxGroup("Easy Access")]
        [SerializeField]
        private GameObject mana;

        [Required]
        [BoxGroup("UI")]
        [SerializeField]
        private Selectable deleteButton;        

        [Required]
        [BoxGroup("New Game")]
        [SerializeField]
        private PlayerSaveGame saveGame;

        [Required]
        [BoxGroup("New Game")]
        [SerializeField]
        private TimeValue timeValue;

        [Required]
        [BoxGroup("New Game")]
        [SerializeField]
        private TeleportStats startTeleport;

        [Required]
        [BoxGroup("Delete")]
        [SerializeField]
        private Image image;

        [Required]
        [BoxGroup("Delete")]
        [SerializeField]
        private ButtonExtension button;

        [BoxGroup("Debug")]
        [SerializeField]
        [ReadOnly]
        private PlayerData data;

        private float percentage = 0f;

        private void OnEnable()
        {
            this.percentage = 0f;

            this.slotNameField.text = this.slotName;
            GetData();
        }

        private void GetData()
        {
            this.newGame.SetActive(false);
            this.loadGame.SetActive(false);
            this.deleteButton.interactable = true;

            this.data = SaveSystem.LoadPlayer(this.slotName);

            if (this.data != null)
            {
                this.loadGame.SetActive(true);

                float maxLife = 1 + (2 * data.maxHealth);
                float maxMana = 1 + (2 * data.maxMana);
                SetValues((int)maxLife, this.health);
                SetValues((int)maxMana, this.mana);

                this.characterName.text = data.characterName;
                this.race.text = data.race;
                this.characterLocation.text = data.GetStartTeleportName();

                TimeSpan span = TimeSpan.FromSeconds(data.timePlayed);
                this.timePlayed.text = span.ToString(@"hh\:mm\:ss");
            }
            else
            {
                this.deleteButton.interactable = false;
                this.newGame.SetActive(true);
            }
        }

        private void SetValues(int value, GameObject temp)
        {
            for (int i = 0; i < temp.transform.childCount; i++)
            {
                temp.transform.GetChild(i).gameObject.SetActive(false);
                if (i < value) temp.transform.GetChild(i).gameObject.SetActive(true);
            }
        }

        private void FixedUpdate()
        {
            if (this.percentage > 0)
            {
                this.percentage -= (Time.fixedDeltaTime/10);
                this.image.fillAmount = this.percentage;
            }
            else if (this.percentage < 0)
            {
                this.percentage = 0;
                this.image.fillAmount = this.percentage;
            }            
        }

        public void OnClick()
        {
            if (this.data == null) StartNewGame();
            else LoadGame();
        }

        public void OnDelete()
        {
            this.percentage += 0.25f;
            if (this.percentage >= 1)
            {
                DeleteCharacter();
                this.percentage = 0;
                this.image.fillAmount = this.percentage;
            }
        }

        private void StartNewGame()
        {
            PrepareSaveGame(()=>
            {
                this.saveGame.teleportList.SetNextTeleport(this.startTeleport, true, true);
                AfterLoad();
            });            
        }

        private void LoadGame()
        {
            PrepareSaveGame(()=> LoadSystem.loadPlayerData(this.saveGame, this.data, AfterLoad));                   
        }

        private void PrepareSaveGame(Action action)
        {
            this.timeValue.Clear();
            this.saveGame.Clear(action, this.slotName);
        }

        private void AfterLoad()
        {
            GameEvents.current.DoChangeScene(this.saveGame.teleportList.GetLatestTeleport().scene);
        }

        private void DeleteCharacter()
        {
            SaveSystem.DeletePlayerData(this.slotName);
            GetData();
            this.button.Select();
        }
    }
}