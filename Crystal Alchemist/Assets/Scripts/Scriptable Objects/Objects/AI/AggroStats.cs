using Sirenix.OdinInspector;
using UnityEngine;

namespace CrystalAlchemist
{
    [CreateAssetMenu(menuName = "Game/AI/Aggro Stats")]
    public class AggroStats : ScriptableObject
    {
        [BoxGroup("Aggro Attributes")]
        public bool firstHitMaxAggro = true;

        [BoxGroup("Aggro Attributes")]
        [Range(0, 100)]
        public float aggroIncreaseFactor = 25;

        [BoxGroup("Aggro Attributes")]
        [Range(0, 100)]
        public float aggroOnHitIncreaseFactor = 25;

        [BoxGroup("Aggro Attributes")]
        [Range(-100, 0)]
        public float aggroDecreaseFactor = -25;

        [BoxGroup("Aggro Attributes")]
        [Range(0, 100)]
        public float aggroNeededToTarget = 100;

        [BoxGroup("Aggro Attributes")]
        public float targetChangeDelay = 0f;

        [BoxGroup("Aggro Object Attributes")]
        [Required]
        public Affections affections;
    }
}
