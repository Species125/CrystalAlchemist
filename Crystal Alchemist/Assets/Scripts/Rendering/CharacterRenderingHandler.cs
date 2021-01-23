using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace CrystalAlchemist
{
    public class CharacterRenderingHandler : MonoBehaviour
    {
        [Required]
        public GameObject characterSprite;

        private List<CharacterRenderer> colorpalettes = new List<CharacterRenderer>();

        private List<Color> colors = new List<Color>();

#if UNITY_EDITOR
        [Button]
        public void setExtensions()
        {
            if (this.characterSprite != null)
            {
                List<SpriteRenderer> renderers = new List<SpriteRenderer>();
                renderers.Clear();

                UnityUtil.GetChildObjects<SpriteRenderer>(this.characterSprite.transform, renderers);

                foreach (SpriteRenderer renderer in renderers)
                {
                    if (renderer.gameObject.GetComponent<CharacterRenderer>() == null)
                    {
                        renderer.gameObject.AddComponent<CharacterRenderer>();
                        Debug.Log("Set Extension for " + renderer.name);
                    }
                }
            }
        }
#endif

        public virtual void Start()
        {
            this.colorpalettes.Clear();
            UnityUtil.GetChildObjects<CharacterRenderer>(this.characterSprite.transform, this.colorpalettes);
        }

        public void Reset()
        {
            this.colors.Clear();
            foreach (CharacterRenderer colorPalette in this.colorpalettes) colorPalette.ChangeTint(Color.white, false);
        }

        public Color RemoveTint(Color color)
        {
            this.colors.Remove(color);

            Color newColor = Color.white;
            bool useTint = false;

            if (colors.Count > 0)
            {
                newColor = this.colors[this.colors.Count - 1];
                useTint = true;
            }

            foreach (CharacterRenderer colorPalette in this.colorpalettes) colorPalette.ChangeTint(newColor, useTint);

            return newColor;
        }

        public Color ChangeTint(Color color)
        {
            if (!this.colors.Contains(color)) this.colors.Add(color);
            foreach (CharacterRenderer colorPalette in this.colorpalettes) colorPalette.ChangeTint(color, true);

            return color;
        }

        public void Invert(bool value)
        {
            foreach (CharacterRenderer colorPalette in this.colorpalettes) colorPalette.InvertColors(value);
        }

        public void enableSpriteRenderer(bool value) => this.characterSprite.SetActive(value);
    }
}
