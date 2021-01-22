using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace CrystalAlchemist {
    public class PlayerListUIChild : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI nameField;

        [SerializeField]
        private Image healthbar;

        [SerializeField]
        private Image manabar;

        [SerializeField]
        private Image dead;

        [SerializeField]
        private CharacterValues values;

        public int ID;

        public void SetChild(int ID)
        {
            Character character = NetworkUtil.GetCharacter(ID);
            this.values = character.values;
            this.ID = ID;
            this.nameField.text = character.GetCharacterName();
            UpdateData(this.ID);
        }

        private void OnEnable() => GameEvents.current.OnLifeManaUpdate += UpdateData;
        
        private void OnDisable() => GameEvents.current.OnLifeManaUpdate -= UpdateData;

        private void UpdateData(int ID)
        {
            if (ID != this.ID) return;            
            this.healthbar.fillAmount = (float)(this.values.life / this.values.maxLife);
            this.manabar.fillAmount = (float)(this.values.mana / this.values.maxMana);
            this.nameField.color = this.values.effectColor;

            if (this.values.life <= 0) this.dead.gameObject.SetActive(true);
            else this.dead.gameObject.SetActive(false);
        }
    }
}
