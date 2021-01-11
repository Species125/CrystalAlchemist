using Sirenix.OdinInspector;
using UnityEngine;

public class SkillSpread : SkillExtension
{
    [Required]
    [SerializeField]
    private Ability ability;

    [Required]
    [SerializeField]
    private Transform spawnPoint;

    [SerializeField]
    private float delay;

    private float elapsed;
    private int index;

    public override void Updating()
    {
        if (this.elapsed <= 0)
        {
            Transform child = this.spawnPoint.GetChild(this.index);
            AbilityUtil.InstantiateSkill(this.ability, this.skill.target, child.position, this.skill.sender, child.rotation);
            this.elapsed = this.delay;
            this.index++;
        }
        else this.elapsed -= Time.deltaTime * this.skill.getTimeDistortion();

        if (this.index >= this.spawnPoint.childCount) this.skill.DeactivateIt();
    }
}
