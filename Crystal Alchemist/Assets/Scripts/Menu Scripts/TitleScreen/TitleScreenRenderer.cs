using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace CrystalAlchemist
{
    public class TitleScreenRenderer : MonoBehaviour
    {
        [HideInInspector]
        private Material material;

        [HideInInspector]
        private SpriteRenderer spriteRenderer;

        [BoxGroup("Renderer")]
        [SerializeField]
        [Range(-1, 1)]
        private float progress = 0f;

        [BoxGroup("Renderer")]
        [SerializeField]
        [ColorUsage(true, true)]
        private Color color = Color.white;

        [BoxGroup("Renderer")]
        [Range(-360,360)]
        [SerializeField]
        private float rotation = -90;

        [BoxGroup("Renderer")]
        [Range(0, 10)]
        [SerializeField]
        private float width = 1;

        [BoxGroup("Renderer")]
        [SerializeField]
        private bool useCircle = false;

        [BoxGroup("Renderer")]
        [ShowIf("useCircle")]
        [SerializeField]
        private Vector2 circlePosition = new Vector2(0.5f,0.5f);

        [BoxGroup("Renderer")]
        [ShowIf("useCircle")]
        [SerializeField]
        [Range(-5, 5)]
        private float circleRatio = 0.66f;



        private void Awake()
        {
            if (this.spriteRenderer == null) this.spriteRenderer = this.GetComponent<SpriteRenderer>();
            this.material = this.spriteRenderer.material;
        }

        private void FixedUpdate()
        {
            UpdateMaterial(this.material);
        }


        private void UpdateMaterial(Material material)
        {
            material.SetFloat("_Progress", this.progress);
            material.SetColor("_Color", this.color);
            material.SetFloat("_Rotation", this.rotation);
            material.SetFloat("_Width", this.width);
            material.SetFloat("_UseCircle", this.useCircle ? 1f : 0f);
            material.SetVector("_CirclePosition", this.circlePosition);
            material.SetFloat("_CircleRatio", this.circleRatio);
        }


        [Button]
        public void Test()
        {
            SpriteRenderer spriteRenderer = this.GetComponent<SpriteRenderer>();
            Material mat = spriteRenderer.sharedMaterial;
            UpdateMaterial(mat);
        }
    }
}
