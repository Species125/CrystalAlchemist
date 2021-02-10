using UnityEngine;

namespace CrystalAlchemist
{
    public class ControlsUI : MonoBehaviour
    {
        [SerializeField]
        private CharacterValues values;
        [SerializeField]
        private GameObject combat;
        [SerializeField]
        private GameObject menu;
        [SerializeField]
        private GameObject interaction;

        private void FixedUpdate() => ShowButtons();
        
        private void ShowButtons()
        {
            if (this.values.currentState == CharacterState.inMenu)
            {
                if (this.combat) this.combat.SetActive(false);
                if (this.interaction) this.interaction.SetActive(false);
                if (this.menu) this.menu.SetActive(true);
            }
            else if (this.values.currentState == CharacterState.interact)
            {
                if (this.combat) this.combat.SetActive(false);
                if (this.interaction) this.interaction.SetActive(true);
                if (this.menu) this.menu.SetActive(false);
            }
            else
            {
                if (this.combat) this.combat.SetActive(true);
                if (this.interaction) this.interaction.SetActive(false);
                if (this.menu) this.menu.SetActive(false);
            }
        }
    }
}
