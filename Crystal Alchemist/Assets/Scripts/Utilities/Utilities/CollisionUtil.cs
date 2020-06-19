﻿using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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

    public static bool checkAffections(Character sender, bool affectOther, bool affectSame, bool affectNeutral, Collider2D hittedCharacter)
    {
        Character target = hittedCharacter.GetComponent<Character>();

        if (!hittedCharacter.isTrigger
            && target != null
            && target.values.currentState != CharacterState.dead
            && target.values.currentState != CharacterState.respawning)
        {
            return checkMatrix(sender, target, affectOther, affectSame, affectNeutral);
        }

        return false;
    }

    private static bool checkMatrix(Character sender, Character target, bool other, bool same, bool neutral)
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
            if (sender.values.characterType == target.values.characterType) return true;
        }

        if (neutral)
        {
            if (target.values.characterType == CharacterType.Object) return true;
        }

        return false;
    }

    public static List<Character> getAllAffectedCharacters(Skill skill)
    {
        List<Character> targets = new List<Character>();
        SkillTargetModule targetModule = skill.GetComponent<SkillTargetModule>();

        if (targetModule != null)
        {
            List<Character> found = MonoBehaviour.FindObjectsOfType<Character>().ToList();

            foreach(Character character in found)
            {
                if (checkMatrix(skill.sender, character, targetModule.affectOther, targetModule.affectSame, targetModule.affectNeutral))
                    targets.Add(character);
            }
            
        }

        return targets;
    }

    public static bool checkCollision(Collider2D hittedCharacter, Skill skill)
    {
        return checkCollision(hittedCharacter, skill, skill.sender);
    }

    public static bool checkCollision(Collider2D other, Skill skill, Character sender)
    {
        if (skill != null && skill.GetTriggerActive())
        {
            SkillTargetModule targetModule = skill.GetComponent<SkillTargetModule>();

            if (targetModule != null)
            {
                Character hittedCharacter = null;
                if (!other.isTrigger) hittedCharacter = other.GetComponent<Character>();

                if (hittedCharacter != null)
                {
                    if (targetModule.affectSelf && hittedCharacter == sender) return true;
                    if (checkAffections(sender, targetModule.affectOther, targetModule.affectSame, targetModule.affectNeutral, other)) return true;
                }

                Skill hittedSkill = AbilityUtil.getSkillByCollision(other.gameObject);

                if (hittedSkill != null)
                {
                    if (targetModule.affectSkills && hittedSkill != skill) return true;
                }
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
