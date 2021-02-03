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

        void Update() => showButtons();

        private void showButtons()
        {
            if (this.values.currentState == CharacterState.inMenu)
            {
                if (!this.menu.activeInHierarchy)
                {
                    this.combat.SetActive(false);
                    this.interaction.SetActive(false);
                    this.menu.SetActive(true);
                }
            }
            else if (this.values.currentState == CharacterState.inDialog)
            {
                this.combat.SetActive(false);
                this.interaction.SetActive(false);
                this.menu.SetActive(false);
            }
            else if (this.values.currentState == CharacterState.interact)
            {
                if (!this.interaction.activeInHierarchy)
                {
                    this.combat.SetActive(false);
                    this.interaction.SetActive(true);
                    this.menu.SetActive(false);
                }
            }
            else
            {
                if (!this.combat.activeInHierarchy)
                {
                    this.combat.SetActive(true);
                    this.interaction.SetActive(false);
                    this.menu.SetActive(false);
                }
            }
        }
    }
}
