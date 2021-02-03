using UnityEngine;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace CrystalAlchemist
{
    [System.Serializable]
    public class AnimationMod
    {
        public string name = string.Empty;
        public Vector2 position = Vector2.zero;
        public int order = 0;

        public AnimationMod(int order)
        {
            this.order = order;
        }
    }

    public class PlayerRendererSync : PlayerRenderer
    {
        [BoxGroup("Renderer Sync")]
        [Tooltip("Set to true if this is a child sprite, like an outfit, eye color, etc.")]
        [SerializeField]
        private bool isMaster = false;

        [BoxGroup("Renderer Sync")]
        [HideIf("isMaster")]
        [Required]
        [SerializeField]
        private PlayerRendererSync master;

        [BoxGroup("Renderer Sync")]
        [SerializeField]
        private bool modifyAnimation;

        [ShowIf("modifyAnimation")]
        [BoxGroup("Renderer Sync")]
        [SerializeField]
        [HideLabel]
        private AnimationMod mod;

        private Sprite[] newSpritesheet;
        private int frameIndex = 0;
        private AnimationMod defaultMod;

        public override void Awake()
        {
            base.Awake();

            if (this.isMaster) this.newSpritesheet = this.property.GetSprites();

            this.defaultMod = new AnimationMod(this.spriteRenderer.sortingOrder);            
        }

        private void LateUpdate()
        {
            if (isMaster) GetIndex(spriteRenderer.sprite.name);
            else if (this.newSpritesheet != null)
            {
                frameIndex = master.GetParentFrameIndex();
                spriteRenderer.sprite = newSpritesheet[frameIndex];

                if (!this.modifyAnimation) return;

                if (this.mod != null && spriteRenderer.sprite.name.Contains(this.mod.name))
                {
                    spriteRenderer.sortingOrder = this.mod.order;
                    this.transform.localPosition = this.mod.position;
                }
                else
                {
                    spriteRenderer.sortingOrder = this.defaultMod.order;
                    this.transform.localPosition = this.defaultMod.position;
                }
            }
        }

        public int GetParentFrameIndex()
        {
            return frameIndex;
        }

        private void GetIndex(string name)
        {
            int.TryParse(Regex.Replace(name, "[^0-9]", ""), out this.frameIndex);    
        }

        public override void UpdateRenderer()
        {
            base.UpdateRenderer();

            this.spriteRenderer.sprite = null;

            if (this.isMaster || this.property == null) this.newSpritesheet = null;                
            else this.newSpritesheet = this.property.GetSprites();
        }

        public override void SetRenderer(CharacterCreatorProperty property, List<Color> colors)
        {
            if (this.isMaster) return;
            base.SetRenderer(property, colors);
        }
    }
}