using System.Collections.Generic;



using UnityEngine;

namespace CrystalAlchemist
{
    public class CharacterCreatorPartHandler : MonoBehaviour
    {
        [SerializeField]
        private Transform parent;

        private CharacterPreset preset = null;

        [SerializeField]
        private CharacterRenderingHandler handler;

        private List<CharacterCreatorPart> parts = new List<CharacterCreatorPart>();

        public void SetPreset(CharacterPreset preset)
        {
            this.preset = preset;
        }

        public void UpdateCharacterParts()
        {
            this.parts.Clear();
            UnityUtil.GetChildObjects<CharacterCreatorPart>(this.parent, this.parts);

            foreach (CharacterCreatorPart part in this.parts)
            {
                part.gameObject.SetActive(false);

                CharacterPartData data = this.preset.GetCharacterPartData(part.property.parentName, part.property.partName);
                if (data != null || part.property.mandatory())
                {
                    part.gameObject.SetActive(true);

                    List<Color> colors = this.preset.getColors(part.property.GetColorTable());
                    part.SetColors(colors);
                }
            }

            handler.Start();
        }
    }
}
