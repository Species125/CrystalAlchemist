

using TMPro;
using UnityEngine;

namespace CrystalAlchemist
{
    public class CastBar : MonoBehaviour
    {
        [SerializeField]
        private SpriteFillBar charging;
        [SerializeField]
        private SpriteRenderer full;
        [SerializeField]
        private TextMeshPro skillName;
        [SerializeField]
        private TextMeshPro percentage;
        [SerializeField]
        private SpriteRenderer icon;
        [SerializeField]

        private float offset = 1f;
        private Ability ability;

        public void setCastBar(Character character, Ability ability)
        {
            this.transform.parent = character.transform;
            this.transform.position = new Vector2(this.transform.position.x, this.transform.position.y + this.offset);
            this.ability = ability;
            this.skillName.text = this.ability.GetName();
            this.icon.sprite = ability.GetSprite();
        }

        private void Update()
        {
            float percent = ability.holdTimer / this.ability.castTime;
            this.charging.fillAmount(percent);

            string text = (int)(percent * 100) + "%";
            if (percent * 100 >= 100) text = "BEREIT!";

            this.percentage.text = text;

            if (ability.holdTimer >= this.ability.castTime) this.full.enabled = true;
            else this.full.enabled = false;
        }

        public void destroyIt()
        {
            Destroy(this.gameObject, 0.1f);
        }
    }
}
