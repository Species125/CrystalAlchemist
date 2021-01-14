
using UnityEngine;

namespace CrystalAlchemist
{
    public class SkillMoveItemHit : SkillHitTrigger
    {
        private Collectable item;

        public override void Updating()
        {
            if (this.item != null && this.item.GetComponent<Rigidbody2D>() != null)
            {
                this.item.GetComponent<Rigidbody2D>().MovePosition(this.skill.transform.position);
            }
        }

        public void moveItem(Collider2D hittedCharacter)
        {
            Collectable hittedItem = hittedCharacter.GetComponent<Collectable>();
            if (hittedItem != null && this.item == null) this.item = hittedItem;
        }

        private void OnTriggerEnter2D(Collider2D hittedCharacter) => moveItem(hittedCharacter);

        private void OnTriggerExit2D(Collider2D hittedCharacter) => moveItem(hittedCharacter);
    }
}
