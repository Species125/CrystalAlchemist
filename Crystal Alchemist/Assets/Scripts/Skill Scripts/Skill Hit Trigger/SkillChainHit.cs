using Sirenix.OdinInspector;
using UnityEngine;

namespace CrystalAlchemist
{
    public class SkillChainHit : SkillHitTrigger
    {
        public enum ChainType
        {
            min,
            between
        }

        [BoxGroup("Mechanics")]
        [SerializeField]
        private ChainType type;

        [SerializeField]
        [ShowIf("type", ChainType.between)]
        [BoxGroup("Mechanics")]
        private Vector2 rangeNeeded;

        [SerializeField]
        [ShowIf("type", ChainType.min)]
        [BoxGroup("Mechanics")]
        private float distanceNeeded = 0f;

        [ShowIf("type", ChainType.min)]
        [BoxGroup("Mechanics")]
        [SerializeField]
        private bool canBreak = false;

        [ShowIf("type", ChainType.min)]
        [BoxGroup("Mechanics")]
        [SerializeField]
        private bool useStartDistance = false;

        [BoxGroup("Mechanics")]
        [SerializeField]
        private bool changeColor = false;

        [BoxGroup("Mechanics")]
        [ShowIf("changeColor", true)]
        [SerializeField]
        [ColorUsage(true, true)]
        private Color rightColor;

        [BoxGroup("Mechanics")]
        [ShowIf("changeColor", true)]
        [SerializeField]
        [ColorUsage(true, true)]
        private Color wrongColor;

        [BoxGroup("Objects")]
        [HideLabel]
        [SerializeField]
        private IndicatorObject indicator;

        private float startDistance;

        private void OnDrawGizmos()
        {
            if (this.type == ChainType.min) DrawMin();
            else if (this.type == ChainType.between) DrawBetween();
        }

        private void DrawMin()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(this.transform.position, this.distanceNeeded);
        }

        private void DrawBetween()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(this.transform.position, this.rangeNeeded.y);
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(this.transform.position, this.rangeNeeded.x);
        }

        public override void Initialize()
        {
            base.Initialize();
            if (this.useStartDistance) this.startDistance = Vector2.Distance(this.skill.target.GetGroundPosition(), this.skill.sender.GetGroundPosition());
        }

        private void FixedUpdate()
        {
            if (this.indicator != null) this.indicator.UpdateCastingIndicator(this.skill.sender, this.skill.target);

            if (this.canBreak && this.type == ChainType.min && this.hasRightDistance()) DeactivateIt();

            if (this.changeColor && this.indicator != null)
            {
                if (this.hasRightDistance()) this.indicator.ChangeIndicator(this.skill.target, this.rightColor);
                else this.indicator.ChangeIndicator(this.skill.target, this.wrongColor);
            }
        }

        public void HitIt()
        {
            if (!hasRightDistance()) this.skill.hitIt(this.skill.target);
        }

        public void DeactivateIt()
        {
            if (this.indicator != null) this.indicator.ClearIndicator();
            this.skill.DeactivateIt();
        }

        private bool hasRightDistance()
        {
            if (this.skill.target != null)
            {
                if (this.type == ChainType.min)
                    return CollisionUtil.checkDistanceTo(this.skill.target.GetGroundPosition(), this.skill.sender.GetGroundPosition(), startDistance, this.distanceNeeded);
                else if (this.type == ChainType.between)
                    return CollisionUtil.checkDistanceBetween(this.skill.target.GetGroundPosition(), this.skill.sender.GetGroundPosition(), this.rangeNeeded.x, this.rangeNeeded.y);
            }

            return false;
        }
    }
}
