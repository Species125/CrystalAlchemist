using UnityEngine;
using Sirenix.OdinInspector;
using System.Collections.Generic;

[RequireComponent(typeof(AI))]
public class AICombat : CharacterCombat
{
    #region Attributes

    [BoxGroup("AI")]
    [SerializeField]
    private AIPhase startPhase;

    [BoxGroup("AI")]
    [SerializeField]
    [ShowIf("startPhase")]
    [Tooltip("False, wenn Animator Event verwendet wird")]
    private bool startImmediately = true;

    private AIPhase activePhase;
    private bool isActive;
    private AI npc;
    #endregion

    public override void Initialize()
    {
        base.Initialize();
        this.npc = this.character.GetComponent<AI>();
    }

    public void SkipPhase()
    {
        this.activePhase.SkipPhase();
    }

    private void OnEnable()
    {
        if (this.startPhase != null && this.startImmediately) StartPhase();
    }

    public void StartPhase() => StartPhase(this.startPhase);

    public override void Updating()
    {
        base.Updating();

        if (this.activePhase != null && !this.character.values.isCharacterStunned())
            this.activePhase.Updating(this.npc);
    }

    private void OnDisable() => EndPhase();    

    private void OnDestroy() => EndPhase();    

    public void EndPhase()
    {
        this.isActive = false;
        DestroyActivePhase();        
    }

    private void DestroyActivePhase()
    {
        if (this.activePhase != null)
        {
            this.activePhase.ResetActions(this.npc);
            Destroy(this.activePhase);
        }
    }

    public void StartPhase(AIPhase phase)
    {
        if (phase != null)
        {
            this.isActive = true;

            DestroyActivePhase();
            this.activePhase = Instantiate(phase);
            this.activePhase.Initialize(this.npc);
        }
    }

    public override List<Character> GetTargetsFromTargeting()
    {
        List<Character> result = new List<Character>();
        foreach(KeyValuePair<Character, float[]> aggro in this.npc.aggroList) result.Add(aggro.Key);
        return result;
    }

    public override void ShowTargetingSystem(Ability ability)
    {
        
    }
}

