

using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace CrystalAlchemist
{
    [RequireComponent(typeof(CircleCollider2D))]
    public class SkillImpactHit : SkillHitTrigger
    {
        public enum aoeType
        {
            hide,
            range,
            view
        }

        public enum lookType
        {
            lookAway,
            lookAtIt
        }

        [BoxGroup("Mechanics")]
        [SerializeField]
        private aoeType type;

        [BoxGroup("Mechanics")]
        [ShowIf("type", aoeType.view)]
        [SerializeField]
        private lookType look;

        [BoxGroup("Mechanics")]
        [ShowIf("type", aoeType.view)]
        [SerializeField]
        [Range(1, 180)]
        [Tooltip("Sichtfeld in Grad. 90 = 45 in jede Richtung")]
        private int sightAngle = 90;

        [BoxGroup("Mechanics")]
        [ShowIf("type", aoeType.range)]
        [SerializeField]
        [Range(0, 20)]
        private float deadZone;

        [BoxGroup("Mechanics")]
        [ShowIf("type", aoeType.range)]
        [SerializeField]
        [Range(0, 20)]
        private float hitZone;

        [BoxGroup("Mechanics")]
        [SerializeField]
        [Range(0, 20)]
        private float radius;


        private void OnValidate()
        {
            if (this.type == aoeType.range)
            {
                if (hitZone < deadZone) hitZone = deadZone;
                if (radius < hitZone) radius = hitZone;
            }

            this.GetComponent<CircleCollider2D>().radius = this.radius;
        }

        private void OnDrawGizmos()
        {
            if (this.type == aoeType.range) DrawRange();
            else if (this.type == aoeType.view) DrawView();
            else if (this.type == aoeType.hide) DrawHide();
        }

        private void DrawHide()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(this.transform.position, this.radius);
        }

        private void DrawView()
        {
            if (this.look == lookType.lookAway)
            {
                Gizmos.color = new Color(1f, 0.25f, 1);
                Gizmos.DrawWireSphere(this.transform.position, this.radius);
                Gizmos.DrawIcon(this.transform.position, "Skills/Look Away", false, Color.white);
            }
            else
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawWireSphere(this.transform.position, this.radius);
                Gizmos.DrawIcon(this.transform.position, "Skills/Look At It", false, Color.white);
            }
        }

        private void DrawRange()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(this.transform.position, this.radius);
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(this.transform.position, this.hitZone);
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(this.transform.position, this.deadZone);

#if UNITY_EDITOR
            GUI.contentColor = Color.red;
            Handles.Label(this.transform.position + new Vector3(this.deadZone + 0.25f, 3), "DEAD (100%)");

            GUI.contentColor = Color.yellow;
            Handles.Label(this.transform.position + new Vector3(this.hitZone + 0.25f, 2), "HIT (50%)");

            GUI.contentColor = Color.green;
            Handles.Label(this.transform.position + new Vector3(this.radius + 0.25f, 1), "SAFE (0%)");
#endif
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            hitTargetCollision(collision);
        }

        private void hitTargetCollision(Collider2D hittedCharacter)
        {
            if (CollisionUtil.CheckCollision(hittedCharacter, this.skill))
            {
                if (this.type == aoeType.hide)
                {
                    //hide AOE
                    bool isHiding = CollisionUtil.checkBehindObstacle(hittedCharacter.GetComponent<Character>(), this.gameObject);
                    if (!isHiding) this.skill.hitIt(hittedCharacter);
                }
                else if (this.type == aoeType.range)
                {
                    //range AOE
                    float percentage = CollisionUtil.checkDistanceReduce(hittedCharacter.GetComponent<Character>(), this.gameObject, this.deadZone, this.hitZone);
                    this.skill.hitIt(hittedCharacter, percentage);
                }
                else if (this.type == aoeType.view)
                {
                    //look /away AOE
                    bool isLookingAt = CollisionUtil.checkIfGameObjectIsViewed(hittedCharacter.GetComponent<Character>(), this.gameObject, this.sightAngle);
                    if ((this.look == lookType.lookAway && isLookingAt) || (this.look == lookType.lookAtIt && !isLookingAt)) this.skill.hitIt(hittedCharacter);
                }
                else
                {
                    //normal AOE
                    this.skill.hitIt(hittedCharacter);
                }
            }
        }
    }
}
