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
            MenuEvents.current.OpenCharacterCreation();
            Invoke("SetDefaultDirection", 0.3f);
        }

        private void SetDefaultDirection() => this.player.SetDefaultDirection();        

        public void ChangeCharacterDirection(Vector2 direction) => this.player.ChangeDirection(direction); //signal
    }
}
