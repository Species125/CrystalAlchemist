using UnityEngine;

namespace CrystalAlchemist
{
    public class CharacterRenderer : CustomRenderer
    {
        public override void Start()
        {
            base.Start();
            ChangeTint(Color.white, false);
        }

        public void ChangeTint(Color color, bool useTint)
        {
            if (this.material == null) return;
            this.material.SetFloat("_UseTint", useTint ? 1f : 0f);
            this.material.SetColor("_Tint", color);
        }
    }
}
