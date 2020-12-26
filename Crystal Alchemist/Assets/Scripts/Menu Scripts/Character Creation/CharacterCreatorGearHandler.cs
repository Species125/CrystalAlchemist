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
        for(int i = 0; i < this.group.properties.Count; i++)
        {
            CharacterCreatorPartProperty property = this.group.properties[i];
            CreateButton(property, i);
            if (FindGear(property))SetCurrentGear(property);  
        }

        Destroy(this.template.gameObject);
    }

    private void CreateButton(CharacterCreatorPartProperty property, int i)
    {
        CharacterCreatorGear button = Instantiate(template, content);
        button.gameObject.SetActive(true);
        button.SetButton(property, this);
        this.SetFirst(button, i);
        this.buttons.Add(button);
    }

    private void SetCurrentGear(CharacterCreatorPartProperty property)
    {
        if (this.currentProperty == property && this.group.canRemove) this.currentProperty = null;
        else this.currentProperty = property;
    }

    private bool FindGear(CharacterCreatorPartProperty property)
    {
        CharacterPartData data = this.mainMenu.creatorPreset.GetCharacterPartData(property);
        return data != null;
    }

    public bool ContainsGear(CharacterCreatorPartProperty property)
    {
        return this.currentProperty == property;
    }

    public void UpdateGear(CharacterCreatorPartProperty property)
    {
        SetCurrentGear(property);

        if (this.currentProperty != null) this.mainMenu.creatorPreset.AddCharacterPartData(property.parentName, property.partName);
        else this.mainMenu.creatorPreset.RemoveCharacterPartData(property.parentName);

        this.UpdatePreview();
    }
}
