using System;
using System.Collections.Generic;


using UnityEngine;

namespace CrystalAlchemist
{
    [System.Serializable]
    public struct PatchNote
    {
        [SerializeField]
        private string version;
        [SerializeField]
        private string date;

        public string GetText()
        {
            string text = string.Format("Version {0} [{1}]", version, date);
            for (int i = 1; i <= 25; i++)
            {
                string details = FormatUtil.GetLocalisedText((version + i), LocalisationFileType.patchnotes);
                if (details != null && details.Length > 1) text += Environment.NewLine + "- " + details;
            }
            return text;
        }
    }

    [CreateAssetMenu(menuName = "Game/Menu/Patch Notes")]
    public class PatchNoteInfo : ScriptableObject
    {
        [SerializeField]
        private List<PatchNote> patchNotes = new List<PatchNote>();

        public List<PatchNote> GetPatchNotes()
        {
            List<PatchNote> result = new List<PatchNote>();
            result.AddRange(this.patchNotes);
            result.Reverse();
            return result;
        }
    }
}