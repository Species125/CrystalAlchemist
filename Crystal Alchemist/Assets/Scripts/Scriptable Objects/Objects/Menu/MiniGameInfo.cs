



using UnityEngine;

namespace CrystalAlchemist
{
    [CreateAssetMenu(menuName = "Game/Menu/MiniGame Info")]
    public class MiniGameInfo : ScriptableObject
    {
        public MiniGameMatches matches;
        public MiniGameRound miniGameUI;
        public string miniGameName;

        public string GetName()
        {
            return FormatUtil.GetLocalisedText(this.miniGameName + "_Name", LocalisationFileType.minigames);
        }

        public string GetDescription()
        {
            return FormatUtil.GetLocalisedText(this.miniGameName + "_Description", LocalisationFileType.minigames);
        }
    }
}
