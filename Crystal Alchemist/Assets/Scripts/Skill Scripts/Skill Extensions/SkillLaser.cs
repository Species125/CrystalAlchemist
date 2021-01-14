using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace CrystalAlchemist
{
    public class SkillLaser : SkillExtension
    {
        #region Attributes

        [InfoBox("Kein Hit-Script notwendig, da kein Collider verwendet wird")]
        [SerializeField]
        [MinValue(0)]
        private float distance = 0;

        [SerializeField]
        private SpriteRenderer laserSprite;

        [SerializeField]
        private bool targetRequired = false;

        private List<GameObject> hitPoints = new List<GameObject>();

        private Vector2 position;

        #endregion


        #region Unity Functions

        public override void Initialize() => this.laserSprite.enabled = false;

        public override void Updating() => drawLine();

        private void OnDestroy() => this.hitPoints.Clear();

        #endregion

        #region Functions (private)
        private void drawLine()
        {
            Collider2D hitted = null;
            Vector2 hitPoint = Vector2.zero;

            if (this.skill.standAlone)
            {
                this.position = this.transform.position;
                this.skill.SetDirection(RotationUtil.DegreeToVector2(this.transform.rotation.eulerAngles.z));
            }
            else
            {
                this.skill.SetDirection(this.skill.sender.values.direction);
                this.skill.SetVectors();

                this.position = this.transform.position;
            }

            if (targetRequired && this.skill.target == null) LineRenderUtil.Renderempty(this.laserSprite);
            else LineRenderUtil.RenderLine(this.skill.sender, this.skill.target, this.skill.GetDirection(), this.distance, this.laserSprite, this.position, out hitted, out hitPoint);

            if (hitted != null && this.skill.GetTriggerActive())
            {
                if (CollisionUtil.checkCollision(hitted, this.skill)) this.skill.hitIt(hitted);
                AbilityUtil.SetEffectOnHit(this.skill, hitPoint);
            }
        }
        #endregion
    }
}
