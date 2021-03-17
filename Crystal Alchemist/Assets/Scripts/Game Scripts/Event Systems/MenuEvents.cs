using System;
using UnityEngine;

namespace CrystalAlchemist
{
    public class MenuEvents : MonoBehaviour
    {
        public static MenuEvents current;

        private void Awake() => current = this;

        public Action OnInventory;
        public Action OnSkills;
        public Action OnMap;
        public Action OnAttributes;
        public Action OnPause;
        public Action OnDeath;
        public Action OnEditor;
        public Action OnSave;
        public Action OnMiniGame;
        public Action<Ability> OnAbilitySelected;
        public Func<Ability> OnAbilitySet;
        public Action OnAbilityAssigned;
        public Action OnDialogBox;
        public Action OnMenuDialogBox;
        public Action OnTutorial;
        public Action OnJukeBox;
        public Action OnOnlineMenu;
        public Action OnAttributeUpdate;
        public Action OnShop;
        public Action<Action> OnPostProcessingFade;
        public Action OnFadeOut;
        public Action OnCloseMenu;

        public void OpenMenuDialogBox() => this.OnMenuDialogBox?.Invoke();
        public void OpenDialogBox() => this.OnDialogBox?.Invoke();
        public void OpenInventory() => this.OnInventory?.Invoke();
        public void OpenSkillBook() => this.OnSkills?.Invoke();
        public void OpenMap() => this.OnMap?.Invoke();
        public void OpenAttributes() => this.OnAttributes?.Invoke();
        public void OpenPause() => this.OnPause?.Invoke();
        public void OpenDeath() => this.OnDeath?.Invoke();
        public void OpenCharacterCreation() => this.OnEditor?.Invoke();
        public void OpenSavepoint() => this.OnSave?.Invoke();
        public void OpenMiniGame() => this.OnMiniGame?.Invoke();
        public void OpenTutorial() => this.OnTutorial?.Invoke();
        public void OpenJukeBox() => this.OnJukeBox?.Invoke();
        public void OpenOnlineMenu() => this.OnOnlineMenu?.Invoke();
        public void OpenShop() => this.OnShop?.Invoke();
        public void SelectAbility(Ability ability) => this.OnAbilitySelected?.Invoke(ability);
        public Ability SetAbility() => this.OnAbilitySet?.Invoke();
        public void AssignAbility() => this.OnAbilityAssigned?.Invoke();
        public void UpdateAttributes() => this.OnAttributeUpdate?.Invoke();
        public void DoPostProcessingFade(Action OnAfterFadeCompleted) => this.OnPostProcessingFade?.Invoke(OnAfterFadeCompleted);
        public void DoFadeOut() => this.OnFadeOut?.Invoke();
        public void DoCloseMenu() => this.OnCloseMenu?.Invoke();
    }
}
