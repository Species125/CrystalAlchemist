using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace CrystalAlchemist
{
    public class CharacterCreatorPart : MonoBehaviour
    {
        [InfoBox("Neccessary to set for Character Creation", InfoMessageType.Info)]
        [BoxGroup("Part")]
        public CharacterCreatorPartProperty property;

        [BoxGroup("Part")]
        public bool ignoreUpdate;

        [SerializeField]
        private SpriteRenderer sprite;

        [BoxGroup("Debug")]
        [SerializeField]
        private Material mat;

        [BoxGroup("Debug")]
        [SerializeField]
        private List<Color> colors = new List<Color>();

        private int maxAmount = 8;

        private void Awake()
        {
            if (sprite != null && this.mat == null) this.mat = sprite.material;
        }

        public void SetColors(List<Color> colors)
        {
            if (sprite != null) this.mat = sprite.material;

            this.colors = colors;

            if (this.mat != null)
            {
                ColorEffect effect = this.property.GetEffect();
                Clear();

                SetColorGroup(colors);
                AddGlow(effect);
            }
        }


        private void SetColorGroup(List<Color> colors)
        {
            bool swapColors = (this.property.canBeColored && colors.Count > 0);
            this.mat.SetInt("_Swap", Convert.ToInt32(swapColors));

            int i = 0;

            while (i < this.property.GetColorTable().Count && i < colors.Count)
            {
                ChangeColorGroup(i, colors[i]);
                i++;
            }
        }


        private void ChangeColorGroup(int index, Color color)
        {
            ColorTable colorTable = this.property.GetColorTable()[index];

            this.mat.SetColor("_Color_" + ((index * 4) + 1), colorTable.highlight);
            this.mat.SetColor("_Color_" + ((index * 4) + 2), colorTable.main);
            this.mat.SetColor("_Color_" + ((index * 4) + 3), colorTable.shadows);
            this.mat.SetColor("_Color_" + ((index * 4) + 4), colorTable.lines);

            this.mat.SetColor("_New_ColorGroup_" + (index + 1), color);
        }

        private void AddGlow(ColorEffect effect)
        {
            this.mat.SetInt("_UseGlow", Convert.ToInt32(effect.addGlow));
            this.mat.SetColor("_GlowColor", effect.default_glow);
        }

        private void Clear()
        {
            for (int i = 1; i <= this.maxAmount; i++) mat.SetColor("_Color_" + i, Color.black);

            this.mat.SetInt("_Swap", 0);
            this.mat.SetInt("_UseGlow", 0);
            this.mat.SetColor("_New_ColorGroup_1", Color.black);
            this.mat.SetColor("_New_ColorGroup_2", Color.black);
            this.mat.SetColor("_GlowColor", Color.black);
        }
    }
}
