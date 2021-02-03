using AssetIcons;



using Sirenix.OdinInspector;
using UnityEngine;

namespace CrystalAlchemist
{
    [CreateAssetMenu(menuName = "Game/Ability/Skill Book Info")]
    public class SkillBookInfo : ScriptableObject
    {
        [BoxGroup("Pflichtfelder")]
        [Tooltip("Beschreibung des Skills")]
        public SkillType category = SkillType.magical;

        [BoxGroup("Pflichtfelder")]
        [Tooltip("Sortierung")]
        public int orderIndex = 10;

        [BoxGroup("Sound und Icons")]
        [Tooltip("Icon für den Spieler")]
        [AssetIcon]
        public Sprite icon;

        public string getDescription()
        {
            return FormatUtil.GetLocalisedText(this.name + "_Description", LocalisationFileType.skills);
        }
    }
}
