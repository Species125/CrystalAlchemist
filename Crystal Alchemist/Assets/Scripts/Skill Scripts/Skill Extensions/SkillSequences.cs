using System.Collections.Generic;


using UnityEngine;

namespace CrystalAlchemist
{
    public class SkillSequences : SkillExtension
    {
        [SerializeField]
        private List<BossMechanic> bossMechanics = new List<BossMechanic>();

        public override void Initialize()
        {
            BossMechanic bossMechanic = this.bossMechanics[Random.Range(0, this.bossMechanics.Count)];
            NetworkEvents.current.InstantiateBossSequence(bossMechanic, this.skill.sender, this.skill.target);
        }
    }
}
