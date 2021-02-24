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
        public int aggroIncreaseFactor = 25;

        [BoxGroup("Aggro Attributes")]
        [Range(0, 100)]
        public int aggroOnHitIncreaseFactor = 25;

        [BoxGroup("Aggro Attributes")]
        [Range(-100, 0)]
        public int aggroDecreaseFactor = -25;

        [BoxGroup("Aggro Attributes")]
        public float targetChangeDelay = 0f;

        [BoxGroup("Aggro Object Attributes")]
        [Required]
        public Affections affections;
    }
}
