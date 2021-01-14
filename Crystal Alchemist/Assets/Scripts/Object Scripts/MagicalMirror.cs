


using UnityEngine;

namespace CrystalAlchemist
{
    public class MagicalMirror : Interactable
    {
        [SerializeField]
        private Vector2 playerPosition = new Vector2(0, 0);

        public override void DoOnSubmit()
        {
            this.player.transform.position = this.playerPosition;
            this.player.SetDefaultDirection();
            MenuEvents.current.OpenCharacterCreation();
        }

        public void ChangeCharacterDirection(Vector2 direction)
        {
            this.player.ChangeDirection(direction);
        }
    }
}
