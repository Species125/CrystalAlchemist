using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace CrystalAlchemist
{
    public class SkillSlot : MonoBehaviour
    {
        [BoxGroup("Debug")]
        [ReadOnly]
        public Image image;

        [BoxGroup("Debug")]
        [ReadOnly]
        public Ability ability;

        [BoxGroup("Debug")]
        [ReadOnly]
        public int ID;

        public int Initialize(int page)
        {
            this.ID = (page * 10) + (this.gameObject.transform.GetSiblingIndex() + 1);
            return this.ID;
        }

        public void SetSkill(Ability ability)
        {
            if (ability == null || !ability.hasSkillBookInfo || ability.info == null) this.image.enabled = false;
            else
            {
                this.ability = ability;
                this.image.enabled = true;
                this.image.sprite = ability.info.icon;
                this.image.color = new Color(1f, 1f, 1f, 1f);
            }
        }

        public void SelectAbility()
        {
            MenuEvents.current.SelectAbility(this.ability);
        }
    }
}
