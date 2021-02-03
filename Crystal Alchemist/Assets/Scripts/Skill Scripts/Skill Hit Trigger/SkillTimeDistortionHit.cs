

using Sirenix.OdinInspector;
using UnityEngine;

namespace CrystalAlchemist
{
    public class SkillTimeDistortionHit : SkillHitTrigger
    {
        [BoxGroup]
        [SerializeField]
        private float skillTimeValue = -99;

        private void SetSkillDistortion(GameObject gameObject)
        {
            Skill skill = AbilityUtil.getSkillByCollision(gameObject);
            if (skill == null || (skill != null && skill.getTimeDistortion() == this.skillTimeValue)) return;

            skill.updateTimeDistortion(this.skillTimeValue);
            Invert(gameObject, true);
        }

        private void RemoveSkillDistortion(GameObject gameObject)
        {
            Skill skill = AbilityUtil.getSkillByCollision(gameObject);
            if (skill == null) return;

            skill.updateTimeDistortion(0);
            Invert(gameObject, false);
        }

        private void Invert(GameObject hittedCharacter, bool value)
        {
            CustomRenderer customRenderer = hittedCharacter.GetComponentInChildren<CustomRenderer>();
            if (customRenderer != null) customRenderer.InvertColors(value);
        }

        private void OnTriggerEnter2D(Collider2D collision) => SetSkillDistortion(collision.gameObject);

        private void OnTriggerExit2D(Collider2D collision) => RemoveSkillDistortion(collision.gameObject);
    }
}
