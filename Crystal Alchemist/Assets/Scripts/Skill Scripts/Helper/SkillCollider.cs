using UnityEngine;

namespace CrystalAlchemist
{
    public class SkillCollider : MonoBehaviour
    {
        public Skill skill;

        public virtual Vector2 GetPosition()
        {
            return this.transform.position;
        }
    }
}
