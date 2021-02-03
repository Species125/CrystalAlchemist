using System.Collections.Generic;


using UnityEngine;

namespace CrystalAlchemist
{
    [System.Serializable]
    public struct TutorialProperty
    {
        [SerializeField]
        private string ID;
        public Sprite firstImage;
        public Sprite secondImage;

        public string GetText()
        {
            return FormatUtil.GetLocalisedText(ID, LocalisationFileType.tutorials);
        }
    }

    [CreateAssetMenu(menuName = "Game/Menu/TutorialInfo")]
    public class TutorialInfo : ScriptableObject
    {
        [SerializeField]
        private string titleID;

        public List<TutorialProperty> properties = new List<TutorialProperty>();

        public string GetTitle()
        {
            return FormatUtil.GetLocalisedText(this.titleID, LocalisationFileType.tutorials);
        }
    }
}