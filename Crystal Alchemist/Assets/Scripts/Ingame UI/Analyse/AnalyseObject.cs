



using UnityEngine;
using UnityEngine.UI;

namespace CrystalAlchemist
{
    public class AnalyseObject : MonoBehaviour
    {
        [SerializeField]
        private Image ImageObjectitemPreview;

        [SerializeField]
        private SpriteRenderer ImageObjectitemPreviewOLD;

        [SerializeField]
        private GameObject parent;

        private Treasure treasureChest;
        private Breakable breakable;

        public void Initialize(Breakable breakable) => this.breakable = breakable;

        public void Initialize(Treasure treasure) => this.treasureChest = treasure;

        private void LateUpdate() => showObjectInfo();

        private void showObjectInfo()
        {
            if (this.treasureChest != null)
            {
                //Show Object Information
                if (treasureChest.itemDrop != null) Activate(treasureChest.itemDrop.stats);
                else Deactivate();
            }
            else if (this.breakable != null)
            {
                //Show Object Information
                if (this.breakable.values.itemDrop != null
                    && this.breakable.values.currentState != CharacterState.dead
                    && this.breakable.values.currentState != CharacterState.respawning) Activate(this.breakable.values.itemDrop.stats);
                else Deactivate();
            }
        }

        private void Activate(ItemStats stats)
        {
            this.parent.SetActive(true);
            if (ImageObjectitemPreview != null) this.ImageObjectitemPreview.sprite = stats.getSprite();
            if (ImageObjectitemPreviewOLD != null) this.ImageObjectitemPreviewOLD.sprite = stats.getSprite();
        }

        private void Deactivate() => this.parent.SetActive(false); //Wenn Truhe geöffnet wurde oder Gegner tot ist    
    }
}
