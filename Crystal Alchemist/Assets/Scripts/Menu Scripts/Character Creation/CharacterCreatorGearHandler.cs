using Sirenix.OdinInspector;
using UnityEngine;

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

    private CharacterCreatorPartProperty currentProperty;

    private void Awake()
    {
        this.template.gameObject.SetActive(false);

        foreach (CharacterCreatorPartProperty property in this.group.properties)
        {
            CreateButton(property);
        }
    }

    private void CreateButton(CharacterCreatorPartProperty property)
    {
        CharacterCreatorGear button = Instantiate(template, content);
        button.gameObject.SetActive(true);
        button.SetButton(property, this);        
        this.buttons.Add(button);
    }

    public bool FindGear(CharacterCreatorPartProperty property)
    {
        CharacterPartData data = this.mainMenu.creatorPreset.GetCharacterPartData(property);
        return data != null;
    }

    public void UpdateGear(CharacterCreatorPartProperty property)
    {
        if (this.currentProperty == property) this.currentProperty = null;
        else this.currentProperty = property;

        if (this.currentProperty != null) this.mainMenu.creatorPreset.AddCharacterPartData(property.parentName, property.partName);
        else this.mainMenu.creatorPreset.RemoveCharacterPartData(property.parentName);
    }
}
