using UnityEngine;

namespace CrystalAlchemist
{
    public static class CollisionUtil
    {
        public static bool checkDistanceTo(Vector2 from, Vector2 to, float startDistance, float distanceNeeded)
        {
            float distance = Vector3.Distance(from, to);
            if (distance > (startDistance + distanceNeeded)) return true;
            return false;
        }

        public static bool checkDistanceBetween(Vector2 from, Vector2 to, float min, float max)
        {
            float distance = Vector3.Distance(from, to);

            if (distance >= min && distance <= max) return true;

            return false;
        }

        public static float checkDistanceReduce(Character character, GameObject gameObject, float dead, float hit)
        {
            float distance = Vector3.Distance(character.GetGroundPosition(), gameObject.transform.position);

            float percentage = 100;
            if (distance > hit) percentage = 0;
            else if (distance > dead) percentage = 50;


            //float percentage = 100 - (100 / (saveDistance - deadDistance) * (distance - deadDistance));
            //percentage = Mathf.Round(percentage / 25) * 25;
            //if (percentage > 100) percentage = 100;
            //else if (percentage < 0) percentage = 0;

            return percentage;
        }

        public static bool checkBehindObstacle(Character character, GameObject gameObject)
        {
            float offset = 0.1f;
            Vector2 targetPosition = new Vector2(character.GetGroundPosition().x - (character.values.direction.x * offset),
                character.GetGroundPosition().y - (character.values.direction.y * offset));

            Vector2 start = gameObject.transform.position;
            Vector2 direction = (targetPosition - start).normalized;

            RaycastHit2D hit = Physics2D.Raycast(start, direction, 100f);

            if (hit && !hit.collider.isTrigger)
            {
                if (hit.collider.gameObject != character.gameObject) return true;
                else return false;
            }

            return true;
        }

        public static bool CheckCollision(Collider2D hittedCharacter, Skill skill)
        {
            return CheckCollision(hittedCharacter, skill, skill.sender);
        }

        public static bool CheckCollisionDead(Collider2D hittedCharacter, Skill skill)
        {
            return CheckCollisionDead(hittedCharacter, skill, skill.sender);
        }

        public static bool CheckCollision(Collider2D other, Skill skill, Character sender)
        {
            if (skill != null && skill.GetTriggerActive())
            {
                SkillTargetModule targetModule = skill.GetComponent<SkillTargetModule>();

                if (targetModule != null)
                {
                    Character hittedCharacter = null;
                    if (!other.isTrigger) hittedCharacter = other.GetComponent<Character>();

                    if (hittedCharacter != null &&
                        targetModule.affections.IsAffected(sender, other)) return true;

                    Skill hittedSkill = AbilityUtil.getSkillByCollision(other.gameObject);
                    return targetModule.affections.isSkillAffected(skill, hittedSkill);
                }
            }

            return false;
        }

        public static bool CheckCollisionDead(Collider2D other, Skill skill, Character sender)
        {
            if (skill != null && skill.GetTriggerActive())
            {
                SkillTargetModule targetModule = skill.GetComponent<SkillTargetModule>();

                if (targetModule != null)
                {
                    Character hittedCharacter = null;
                    if (!other.isTrigger) hittedCharacter = other.GetComponent<Character>();

                    if (hittedCharacter != null &&
                        targetModule.affections.IsDeadAffected(sender, other)) return true;

                    Skill hittedSkill = AbilityUtil.getSkillByCollision(other.gameObject);
                    return targetModule.affections.isSkillAffected(skill, hittedSkill);
                }
            }

            return false;
        }

        public static bool checkIfGameObjectIsViewed(Character character, GameObject target, int range)
        {
            Vector2 direction = character.values.direction;
            Vector2 temp = (character.transform.position - target.transform.position).normalized;

            float direction_angle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
            float temp_angle = Mathf.Atan2(temp.x, temp.y) * Mathf.Rad2Deg;
            float angle = Mathf.Abs(direction_angle - temp_angle);

            float min = 180 - range;
            float max = 180 + range;

            if (angle >= min && angle <= max) return true;
            return false;
        }

        public static bool checkIfGameObjectIsViewed(Character character, GameObject gameObject)
        {
            return checkIfGameObjectIsViewed(character, gameObject, 1f);
        }

        public static bool checkIfGameObjectIsViewed(Character character, GameObject gameObject, float distance)
        {
            float width = 0.2f;
            float offset = 0.1f;

            Vector2 position = new Vector2(character.GetGroundPosition().x - (character.values.direction.x * offset),
                character.GetGroundPosition().y - (character.values.direction.y * offset));

            RaycastHit2D[] hit = Physics2D.CircleCastAll(position, width, character.values.direction, distance);

            foreach (RaycastHit2D hitted in hit)
            {
                if (hitted != false
                    && !hitted.collider.isTrigger
                    && ((hitted.collider.transform.gameObject == gameObject)
                        || (hitted.collider.transform.parent != null && hitted.collider.transform.parent.gameObject == gameObject))) return true;
            }

            return false;
        }
    }
}
