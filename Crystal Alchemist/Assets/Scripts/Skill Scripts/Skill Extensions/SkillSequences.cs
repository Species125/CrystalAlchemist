using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillSequences : SkillExtension
{
    [SerializeField]
    private List<BossMechanic> bossMechanics = new List<BossMechanic>();

    public override void Initialize()
    {
        BossMechanic bossMechanic = this.bossMechanics[Random.Range(0, this.bossMechanics.Count)];
        AbilityUtil.instantiateSequence(bossMechanic, this.skill.sender, this.skill.target);
    }
}
