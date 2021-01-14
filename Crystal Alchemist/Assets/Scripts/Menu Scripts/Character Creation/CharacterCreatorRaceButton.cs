
using TMPro;
using UnityEngine;

namespace CrystalAlchemist
{
    public class CharacterCreatorRaceButton : CharacterCreatorButton
    {
        [SerializeField]
        private TMP_Text textField;

        private Race race;

        private CharacterCreatorRaceHandler handler;

        public override bool IsSelected()
        {
            return this.handler.ContainsRace(this.race);
        }

        public void SetButton(CharacterRace race, CharacterCreatorRaceHandler handler)
        {
            this.handler = handler;
            this.textField.text = race.raceName;
            this.preview.sprite = race.icon;
            this.race = race.race;
        }

        public override void Click()
        {
            this.handler.UpdateRace(this.race);
        }
    }
}
