using UnityEngine;

namespace CrystalAlchemist
{
    public class CharacterRenderer : CustomRenderer
    {
        [SerializeField]
        private bool useTint = false;

        [SerializeField]
        private Color color = Color.white;

        public override void Start()
        {
            base.Start();
            SetTint();
        }

        public void ChangeTint(Color color, bool useTint)
        {
            if (this.material == null) return;

            this.color = color;
            this.useTint = useTint;

            Invoke("SetTint", 0.1f); //small delay to get it work on scene changes
        }

        private void SetTint()
        {            
            this.material.SetFloat("_UseTint", this.useTint ? 1f : 0f);
            this.material.SetColor("_Tint", this.color);
        }
    }
}
