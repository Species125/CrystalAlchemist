using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

public class SkillEffectModule : SkillModule
{
    public enum Mode
    {
        OnStart,
        OnUpdate,
        OnHit
    }

    [SerializeField]
    private Object effect;

    [SerializeField]
    private Mode mode;

    [SerializeField]
    private bool hasMaxDistance = false;

    [ShowIf("hasMaxDistance")]
    [SerializeField]
    private float maxDistanceBetween = 1f;

    [ShowIf("mode", Mode.OnUpdate)]
    [SerializeField]
    private float delay;

    [SerializeField]
    private float autoDestroyAfter;

    private List<GameObject> hitPoints = new List<GameObject>();
    private float ghostDelay;

    public override void Initialize()
    {
        base.Initialize();
        if (this.mode == Mode.OnStart) SetImpactEffect(this.transform.position);
    }

    public override void Updating()
    {
        base.Updating();
        OnUpdate();
    }

    private void OnUpdate()
    {
        if (this.mode != Mode.OnUpdate) return;
        if (ghostDelay > 0) this.ghostDelay -= (Time.deltaTime * this.skill.getTimeDistortion());
        else
        {
            SetImpactEffect(this.skill.transform.position);
            this.ghostDelay = this.delay;
        }
    }

    public void OnHit(Vector2 position)
    {
        if (this.mode == Mode.OnHit) SetImpactEffect(position);
    }

    private void SetImpactEffect(Vector2 position)
    {
        if (this.effect != null)
        {
            this.hitPoints.RemoveAll(item => item == null);

            bool impactPossible = true;
            if (this.hasMaxDistance) impactPossible = UnityUtil.CheckDistances(position, this.maxDistanceBetween, this.hitPoints);

            if (impactPossible)
            {
                if (this.effect.GetType() == typeof(GameObject))
                {
                    GameObject gameObject = Instantiate(this.effect, this.transform.position, Quaternion.identity) as GameObject;
                    AddToList(gameObject);
                }
                if (this.effect.GetType() == typeof(Ability))
                {
                    Ability ability = Instantiate(this.effect) as Ability;
                    //Skill skill = ability.InstantiateSkill
                    Skill skill = AbilityUtil.InstantiateSkill(ability, position, this.skill.sender);
                    skill.transform.SetParent(this.transform);
                    Destroy(ability);
                    AddToList(skill.gameObject);
                }
            }
        }
    }

    private void AddToList(GameObject gameObject)
    {
        hitPoints.Add(gameObject);
        if(this.autoDestroyAfter > 0) Destroy(gameObject, this.autoDestroyAfter);
    }
}
