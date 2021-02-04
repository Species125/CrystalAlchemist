using Sirenix.OdinInspector;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CrystalAlchemist
{
    public class SaveSlot : MonoBehaviour
    {
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
        private GameObject factions;

        [Required]
        [BoxGroup("Easy Access")]
        [SerializeField]
        private TextMeshProUGUI race;

        [Required]
        [BoxGroup("Easy Access")]
        [SerializeField]
        private TextMeshProUGUI slotname;

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
        [BoxGroup("Easy Access")]
        [SerializeField]
        private bool isForLoading = false;

        public PlayerData data;
        private Selectable button;

        private void Awake()
        {
            this.button = this.GetComponent<Selectable>();
            if(this.isForLoading) this.button.interactable = false;
        }

        private void OnEnable()
        {
            this.slotname.text = this.gameObject.name;
            getData();
        }

        public void getData()
        {
            this.newGame.SetActive(false);
            this.loadGame.SetActive(false);

            foreach (Transform child in this.factions.transform)
            {
                child.gameObject.SetActive(false);
            }

            this.data = SaveSystem.loadPlayer(this.gameObject.name);

            if (this.data != null)
            {
                if (this.isForLoading) this.button.interactable = true;

                string name = data.characterName;
                string race = data.race;
                float timePlayed = data.timePlayed;
                string ort = data.GetStartTeleportName();
                float maxLife = 1 + (2 * data.maxHealth);
                float maxMana = 1 + (2 * data.maxMana);

                this.loadGame.SetActive(true);

                setLifeMana((int)maxLife, this.health);
                setLifeMana((int)maxMana, this.mana);
                this.characterName.text = name;

                this.race.text = race;

                foreach (Transform child in this.factions.transform)
                {
                    if (child.name == race)
                    {
                        child.gameObject.SetActive(true);
                        break;
                    }
                }

                this.characterLocation.text = ort;

                TimeSpan span = TimeSpan.FromSeconds(timePlayed);
                this.timePlayed.text = span.ToString(@"hh\:mm\:ss");
            }
            else
            {
                if (this.isForLoading) this.button.interactable = false;
                this.newGame.SetActive(true);
            }
        }

        private void setLifeMana(int value, GameObject temp)
        {
            for (int i = 0; i < temp.transform.childCount; i++)
            {
                temp.transform.GetChild(i).gameObject.SetActive(false);
                if (i < value) temp.transform.GetChild(i).gameObject.SetActive(true);
            }
        }
    }
}
