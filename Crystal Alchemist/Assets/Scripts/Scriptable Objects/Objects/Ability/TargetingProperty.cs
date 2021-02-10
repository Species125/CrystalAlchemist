using Sirenix.OdinInspector;
using UnityEngine;

namespace CrystalAlchemist
{
    public enum TargetingMode
    {
        manual,
        auto
    }

    public enum RangeType
    {
        none,
        circle,
        view
    }

    [CreateAssetMenu(menuName = "Game/Ability/Targeting Property")]
    public class TargetingProperty : ScriptableObject
    {
        [BoxGroup("Zielerfassung")]
        [Tooltip("Manual = Spieler kann Ziele auswählen, Auto = Sofort ohne Bestätigung")]
        public TargetingMode targetingMode = TargetingMode.manual;

        [BoxGroup("Zielerfassung")]
        public int maxAmountOfTargets = 1;

        [BoxGroup("Zielerfassung")]
        public bool hasMaxDuration = true;

        [ShowIf("hasMaxDuration")]
        [BoxGroup("Zielerfassung")]
        public float maxDuration = 20f;

        [BoxGroup("Zielerfassung")]
        [Tooltip("In welchen Intervallen die Ziele getroffen werden sollen")]
        [Range(0, 10)]
        public float multiHitDelay = 0;

        [BoxGroup("Zielerfassung")]
        [Tooltip("Soll die Reichweite bei der Zielerfassung angezeigt werden")]
        public bool showRange = false;

        [BoxGroup("Zielerfassung")]
        public RangeType rangeType = RangeType.circle;

        [BoxGroup("Zielerfassung")]
        [HideIf("rangeType", RangeType.none)]
        public float range = 6f;

        [BoxGroup("Zielerfassung")]
        public SkillAffections affections;
    }
}