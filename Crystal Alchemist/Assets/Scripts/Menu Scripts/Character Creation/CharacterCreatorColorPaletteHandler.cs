using UnityEngine;
using Sirenix.OdinInspector;

public class CharacterCreatorColorPaletteHandler : CharacterCreatorButtonHandler
{
    [Required]
    [SerializeField]
    private CharacterCreatorColorPalette palette;

    [Required]
    [SerializeField]
    private CharacterCreatorColor template;

    [SerializeField]
    private bool setFirst = false;

    public ColorGroup colorGroup;

    private Color currentColor;

    private void Awake()
    {
        template.gameObject.SetActive(false);

        for(int i = 0; i < this.palette.colors.Count; i++)
        {
            Color color = this.palette.colors[i];

            CharacterCreatorColor colorOption = Instantiate(this.template, this.transform);        
            if (i == 0 && this.setFirst) colorOption.GetComponent<ButtonExtension>().SetAsFirst();

            colorOption.gameObject.SetActive(true);
            colorOption.SetButton(color, this);

            this.buttons.Add(colorOption);
        }
    }

    public bool HasColor(Color color)
    {
        ColorGroupData data = this.mainMenu.creatorPreset.GetColorGroupData(this.colorGroup);
        if (data != null && data.color == color) return true;
        return false;
    }

    public void UpdateColor(Color color)
    {
        this.mainMenu.creatorPreset.AddColorGroup(colorGroup, color);
    }
}
