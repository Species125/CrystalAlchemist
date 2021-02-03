
using TMPro;
using UnityEngine;

namespace CrystalAlchemist
{
    public class SkillGetDuration : MonoBehaviour
    {
        [SerializeField]
        private TextMeshPro textField;

        [SerializeField]
        private Skill skill;

        private void FixedUpdate() => this.textField.text = FormatUtil.setDurationToString(skill.GetDurationLeft());
    }
}
