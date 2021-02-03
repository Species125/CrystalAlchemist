
using Sirenix.OdinInspector;
using UnityEngine;

namespace CrystalAlchemist
{
    [CreateAssetMenu(menuName = "Game/AI/Movement Stats")]
    public class MovementStats : ScriptableObject
    {
        [BoxGroup("Character Movement")]
        public MovementPriority movementPriority = MovementPriority.target;

        [BoxGroup("Character Movement")]
        public float targetRadius = 0.1f;

        [BoxGroup("Character Movement")]
        public float partnerRadius = 0.1f;

        [BoxGroup("Character Movement")]
        public float delay = 0;



        [BoxGroup("Movement Attributes")]
        public bool backToStart = true;

        [ShowIf("backToStart")]
        [BoxGroup("Movement Attributes")]
        public float returnDelay = 3f;



        [BoxGroup("Patrol")]
        public bool isPatrol = false;

        [ShowIf("isPatrol")]
        [BoxGroup("Patrol")]
        public float patrolDelay = 3f;



        [ShowIf("isPatrol")]
        [BoxGroup("Patrol")]
        public float followPathPrecision = 0.01f;

        [ShowIf("isPatrol")]
        [BoxGroup("Patrol")]
        public bool followPathInCircle = true;

        [BoxGroup("Pathfinding")]
        public float accuracy = 0.25f;
    }
}
