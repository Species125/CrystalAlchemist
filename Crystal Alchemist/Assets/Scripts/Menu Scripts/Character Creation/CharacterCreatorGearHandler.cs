
using Sirenix.OdinInspector;
using UnityEngine;

namespace CrystalAlchemist
{
    public class CharacterCreatorGearHandler : CharacterCreatorButtonHandler
    {
        [Required]
        [SerializeField]
        private Transform content;

        [Required]
        [SerializeField]
        private CharacterCreatorGear template;

        [Required]
        [SerializeField]
        private CharacterCreatorPropertyGroup group;

        private CharacterCreatorProperty currentProperty;

        private void Awake()
        {
            for (int i = 0; i < this.group.GetProperties().Count; i++)
            {
                CharacterCreatorProperty property = this.group.GetProperties()[i];
                CreateButton(property, i);
                if (FindGear(property)) SetCurrentGear(property);
            }

            Destroy(this.template.gameObject);
        }

        private void CreateButton(CharacterCreatorProperty property, int i)
        {
            CharacterCreatorGear button = Instantiate(template, content);
            button.gameObject.SetActive(true);
            button.SetButton(property, this);
            this.SetFirst(button, i);
            this.buttons.Add(button);
        }

        private void SetCurrentGear(CharacterCreatorProperty property)
        {
            if (this.currentProperty == property && this.group.canRemove) this.currentProperty = null;
            else this.currentProperty = property;
        }

        private bool FindGear(CharacterCreatorProperty property)
        {
            return this.mainMenu.playerPreset.ContainsProperty(property);
        }

        public bool ContainsGear(CharacterCreatorProperty property)
        {
            return this.currentProperty == property;
        }

        public void UpdateGear(CharacterCreatorProperty property)
        {
            SetCurrentGear(property);

            if (this.currentProperty != null) this.mainMenu.playerPreset.AddProperty(property);
            else this.mainMenu.playerPreset.RemoveProperty(property);

            this.UpdatePreview();
        }
    }
}
