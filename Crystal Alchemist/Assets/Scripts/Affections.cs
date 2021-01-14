using Sirenix.OdinInspector;
using UnityEngine;

namespace CrystalAlchemist
{
    [CreateAssetMenu(menuName = "Game/Affections/Character Affection")]
    public class Affections : ScriptableObject
    {
        [BoxGroup("Wirkungsbereich")]
        [Tooltip("Anderer Charaktertyp")]
        [SerializeField]
        protected bool other = false;

        [BoxGroup("Wirkungsbereich")]
        [Tooltip("Gleicher Charaktertyp")]
        [SerializeField]
        protected bool same = false;

        [BoxGroup("Wirkungsbereich")]
        [Tooltip("wirkt auf Objekte")]
        [SerializeField]
        protected bool neutral = false;

        public bool IsAffected(Character sender, Collider2D hittedCharacter)
        {
            Character target = hittedCharacter.GetComponent<Character>();

            if (!hittedCharacter.isTrigger
                && target != null
                && target.values.currentState != CharacterState.dead
                && target.values.currentState != CharacterState.respawning)
            {
                return IsAffected(sender, target);
            }

            return false;
        }

        protected virtual bool IsAffected(Character sender, Character target)
        {
            return checkMatrix(sender, target, other, same, neutral, false);
        }

        protected bool checkMatrix(Character sender, Character target, bool other, bool same, bool neutral, bool self)
        {
            if (other)
            {
                if (sender == null) return true;
                if (sender.values.characterType == CharacterType.Friend && target.values.characterType == CharacterType.Enemy) return true;
                if (sender.values.characterType == CharacterType.Enemy && target.values.characterType == CharacterType.Friend) return true;
            }

            if (same)
            {
                if (sender == null) return true;
                if (sender.values.characterType == target.values.characterType && sender != target) return true;
            }

            if (neutral)
            {
                if (target.values.characterType == CharacterType.Object) return true;
            }

            if (self && sender == target) return true;

            return false;
        }
    }
}


