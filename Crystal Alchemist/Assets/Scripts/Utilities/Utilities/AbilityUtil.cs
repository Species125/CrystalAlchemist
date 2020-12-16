using UnityEngine;

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
}
