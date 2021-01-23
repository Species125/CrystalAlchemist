using Sirenix.OdinInspector;
using UnityEngine;

namespace CrystalAlchemist
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class CustomRenderer : MonoBehaviour
    {
        [HideInInspector]
        public Material material;

        [HideInInspector]
        public SpriteRenderer spriteRenderer;

        [BoxGroup("Renderer")]
        [SerializeField]
        private bool useGlow = false;

        [BoxGroup("Renderer")]
        [ShowIf("useGlow")]
        [SerializeField]
        [ColorUsage(true, true)]
        private Color glowColor = Color.white;

        [BoxGroup("Renderer")]
        [SerializeField]
        private bool invert = false;

        public virtual void Awake()
        {
            if (this.spriteRenderer == null) this.spriteRenderer = this.GetComponent<SpriteRenderer>();
            if (this.material == null) this.material = this.GetComponent<SpriteRenderer>().material;
        }

        public virtual void Start()
        {
            AddGlow();
        }

        public void SetGlow(bool useGlow, Color glowColor)
        {
            this.useGlow = useGlow;
            this.glowColor = glowColor;
        }

        public void InvertColors(bool invert)
        {
            if (this.material == null) return;
            this.material.SetFloat("_Invert", invert ? 1f : 0f);
        }

        private void AddGlow()
        {
            if (this.material == null) return;
            this.material.SetFloat("_UseGlow", this.useGlow ? 1f : 0f);
            this.material.SetColor("_GlowColor", this.glowColor);
        }

        [Button]
        public virtual void Test()
        {
            SpriteRenderer spriteRenderer = this.GetComponent<SpriteRenderer>();
            Material mat = spriteRenderer.sharedMaterial;
            mat.SetFloat("_Invert", invert ? 1f : 0f);
            mat.SetFloat("_UseGlow", this.useGlow ? 1f : 0f);
            mat.SetColor("_GlowColor", this.glowColor);
        }

        [Button]
        public virtual void Clear()
        {
            SpriteRenderer spriteRenderer = this.GetComponent<SpriteRenderer>();
            Material mat = spriteRenderer.sharedMaterial;
            mat.SetFloat("_Invert", 0f);
            mat.SetFloat("_UseGlow", 0f);
            mat.SetColor("_GlowColor", Color.black);
        }
    }
}
