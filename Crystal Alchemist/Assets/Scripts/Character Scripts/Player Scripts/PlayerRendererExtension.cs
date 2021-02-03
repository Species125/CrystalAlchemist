using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace CrystalAlchemist 
{
    public class PlayerRendererExtension : PlayerRendererSync
    {
        [BoxGroup("Renderer Extension")]
        [SerializeField]
        private CharacterCreatorProperty activationProperty;

        [BoxGroup("Renderer Extension")]
        [SerializeField]
        private GameObject extension;

        public override void SetRenderer(CharacterCreatorProperty property, List<Color> colors)
        {
            base.SetRenderer(property, colors);
            extension.SetActive(this.property == activationProperty);
        }
    }
}
