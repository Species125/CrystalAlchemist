using UnityEngine;
using System.Collections.Generic;
using Photon.Pun;

public static class AbilityUtil
{
    public static Ability InstantiateAbility(Ability ability)
    {
        Ability newAbility = Object.Instantiate(ability);
        newAbility.Initialize();
        newAbility.name = ability.name;
        newAbility.SetSender(ability.GetSender());

        return newAbility;
    }

    public static Ability InstantiateAbility(Ability ability, Character sender)
    {
        Ability newAbility = Object.Instantiate(ability);
        newAbility.Initialize();
        newAbility.name = ability.name;
        newAbility.SetSender(sender);
        return newAbility;
    }

    public static void instantiateSequence(BossMechanic sequence, Character sender, Character target)
    {
        BossMechanic newSequence = Object.Instantiate(sequence);
        newSequence.name = sequence.name;
        newSequence.Initialize(sender, target);
    }    

    public static Skill getSkillByCollision(GameObject collision)
    {
        return collision.GetComponentInParent<Skill>();
    }

    public static void SetEffectOnHit(Skill skill, Vector2 position)
    {
        foreach (SkillEffectModule modules in skill.GetComponents<SkillEffectModule>()) modules.OnHit(position);
    }






    public static Skill InstantiateSkill(Ability ability, Character target)
    {
        //Single Target
        return InstantiateSkill(ability, target, 1);
    }

    public static Skill InstantiateSkill(Ability ability, Character target, float reduce)
    {
        //Single Target
        return InstantiateSkill(ability, target, ability.GetSender().transform.position, reduce);
    }

    public static Skill InstantiateSkill(Ability ability, Vector2 position, Character sender)
    {
        //Laser and Projectile Impact
        return InstantiateSkill(ability, null, position, sender, Quaternion.identity);
    }

    public static Skill InstantiateSkill(Ability ability, Character target, Vector2 position, Character sender, Quaternion rotation)
    {
        //Boss Sequence
        Skill result = InstantiateSkill(ability, target, position, 1, true, rotation);
        result.sender = sender;
        return result;
    }

    public static Skill InstantiateSkill(Ability ability, Character target, Vector2 position, float reduce)
    {
        return InstantiateSkill(ability, target, position, reduce, false, Quaternion.identity);
    }

    public static Skill InstantiateSkill(Ability ability, Character target, Vector2 position, float reduce, bool standAlone, Quaternion rotation)
    {
        if (ability.skill == null) return null;

        Character sender = ability.GetSender();

        Skill activeSkill = Object.Instantiate(ability.skill, position, rotation);
        //Skill activeSkill = temp.GetComponent<Skill>();
        //GameObject temp = PhotonNetwork.Instantiate(ability.skill.path, position, rotation);

        activeSkill.name = ability.skill.name;
        activeSkill.Initialize(ability.positionOffset, ability.lockDirection, ability.isRapidFire, ability.timeDistortion, ability.attachToSender);
        activeSkill.SetMaxDuration(ability.hasMaxDuration, ability.maxDuration);
        activeSkill.SetStandAlone(standAlone);
        activeSkill.SetDelay(ability.hasDelay, ability.delay);

        if (target != null) activeSkill.target = target;

        ReduceCostAndDamage(ability, activeSkill, reduce, ability.shareDamage);

        if (sender != null)
        {
            if (ability.attachToSender) activeSkill.transform.parent = sender.activeSkillParent.transform;
            activeSkill.sender = sender;
            sender.values.activeSkills.Add(activeSkill);
        }

        return activeSkill;
    }



    public static void ReduceCostAndDamage(Ability ability, Skill activeSkill, float reduce, bool shareDamage)
    {
        SkillTargetModule targetModule = activeSkill.GetComponent<SkillTargetModule>();
        SkillSenderModule sendermodule = activeSkill.GetComponent<SkillSenderModule>();

        if (targetModule != null && shareDamage)
        {
            List<CharacterResource> temp = new List<CharacterResource>();

            for (int i = 0; i < targetModule.affectedResources.Count; i++)
            {
                CharacterResource elem = targetModule.affectedResources[i];
                elem.amount /= reduce;
                temp.Add(elem);
            }

            targetModule.affectedResources = temp;
        }

        if (sendermodule != null) sendermodule.costs.amount /= reduce;
    }
}
