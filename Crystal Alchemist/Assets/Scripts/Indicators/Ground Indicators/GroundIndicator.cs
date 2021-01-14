
using Sirenix.OdinInspector;
using UnityEngine;

namespace CrystalAlchemist
{
    public class GroundIndicator : MonoBehaviour
    {
        [BoxGroup("Outer")]
        public DrawPrimitive outline;

        [BoxGroup("Outer")]
        [ShowIf("outline")]
        [SerializeField]
        [Range(0, 5)]
        private float lineWidth = 0.05f;

        [BoxGroup("Outer")]
        [ShowIf("outline")]
        [SerializeField]
        private Color outlineColor = Color.white;

        [BoxGroup("Outer")]
        [ShowIf("outline")]
        [SerializeField]
        private Material outlineMaterial;

        private void OnValidate()
        {
            SetIndicator();
        }

        public virtual void SetIndicator()
        {
            SetOuter();
        }

        public virtual void SetOuter()
        {
            if (this.outline != null) this.outline.SetPrimitive(this.outlineMaterial, this.lineWidth, this.outlineColor);
        }
    }
}
