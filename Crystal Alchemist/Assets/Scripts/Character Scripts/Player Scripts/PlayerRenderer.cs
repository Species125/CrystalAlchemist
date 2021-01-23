using System;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace CrystalAlchemist {
    public class PlayerRenderer : CharacterRenderer
    {
        [BoxGroup("Renderer")]
        public BodyPart type;

        [BoxGroup("Renderer")]
        [SerializeField]
        private bool isReadOnly = false;

        [ShowIf("isReadOnly")]
        [BoxGroup("Renderer")]
        [Required]
        public CharacterCreatorProperty property;

        [BoxGroup("Renderer")]
        [SerializeField]
        [Required]
        private Material defaultMaterial;

        private List<Color> colors = new List<Color>();
        private int maxAmount = 8;

        public override void Awake()
        {
            base.Awake();
            if (!this.isReadOnly) this.property = null;            
        }

        public virtual void UpdateRenderer()
        {
            base.Start();

            if (this.property != null 
                && this.property.useCustomShader 
                && this.property.material != null) this.spriteRenderer.material = this.property.material;
            else this.spriteRenderer.material = this.defaultMaterial;            

            this.material = this.spriteRenderer.material;

            if (this.material != null && this.property != null)
            {
                ColorEffect effect = this.property.GetEffect();
                Clear();

                SetColorGroup(colors);
                AddGlow(effect.addGlow, effect.default_glow);
            }
        }

        public virtual void SetRenderer(CharacterCreatorProperty property, List<Color> colors)
        {      
            if (!this.isReadOnly 
                && ((property != null && !property.dontUpdateSprites)
                || property == null)) this.property = property;

            this.colors = colors;            

            UpdateRenderer();
        }

        private void SetColorGroup(List<Color> colors)
        {
            bool swapColors = (this.property.canBeColored && colors.Count > 0);
            this.material.SetInt("_Swap", Convert.ToInt32(swapColors));

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

            this.material.SetColor("_Color_" + ((index * 4) + 1), colorTable.highlight);
            this.material.SetColor("_Color_" + ((index * 4) + 2), colorTable.main);
            this.material.SetColor("_Color_" + ((index * 4) + 3), colorTable.shadows);
            this.material.SetColor("_Color_" + ((index * 4) + 4), colorTable.lines);

            this.material.SetColor("_New_ColorGroup_" + (index + 1), color);
        }

        public override void Clear()
        {
            base.Clear();
            for (int i = 1; i <= this.maxAmount; i++) this.material.SetColor("_Color_" + i, Color.black);

            this.material.SetInt("_Swap", 0);
            this.material.SetInt("_UseGlow", 0);
            this.material.SetColor("_New_ColorGroup_1", Color.black);
            this.material.SetColor("_New_ColorGroup_2", Color.black);
            this.material.SetColor("_GlowColor", Color.black);
        }
    }
}
