


using UnityEngine;
using UnityEngine.UI;

namespace CrystalAlchemist
{
    public class SkillMenuActiveSlots : MonoBehaviour
    {
        [SerializeField]
        private PlayerButtons buttons;
        [SerializeField]
        private Image skillImage;
        [SerializeField]
        private enumButton button;

        public Ability ability;

        private void Start() => setImage();

        private void setImage()
        {
            this.ability = this.buttons.GetAbilityFromButton(this.button);

            if (this.ability != null)
            {
                this.skillImage.gameObject.SetActive(true);
                if (ability.hasSkillBookInfo && this.ability.info != null) this.skillImage.sprite = this.ability.info.icon;
            }
            else
            {
                this.skillImage.gameObject.SetActive(false);
            }
        }

        public void AssignSkill()
        {
            Ability ability = MenuEvents.current.SetAbility();
            this.buttons.SetAbilityToButton(ability, this.button);
            MenuEvents.current.SelectAbility(null);

            setImage();
            MenuEvents.current.AssignAbility();
        }
    }
}
