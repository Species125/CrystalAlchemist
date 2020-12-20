using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.UI;

public class CharacterCreatorPart : MonoBehaviour
{
    [InfoBox("Neccessary to set for Character Creation", InfoMessageType.Info)]
    public CharacterCreatorPartProperty property;

    [SerializeField]
    private bool isPreview = false;

    [ShowIf("isPreview", true)]
    public bool isFront = true;

    [ShowIf("isPreview", true)]
    public bool readOnly = false;

    private int maxAmount = 8;

    [SerializeField]
    private SpriteRenderer sprite;

    [SerializeField]
    private Image image;

    private Material mat;

    private void Awake()
    {
        if (sprite != null) this.mat = sprite.material;
        if (image != null)
        {
            this.image.material = Instantiate<Material>(this.image.material);
            this.mat = image.material;
        }
    }

    public void UpdatePreview(CharacterCreatorPartProperty property)
    {
        if (this.readOnly) return;
        if (property != null) this.property = property;

        Sprite sprite = this.property.GetSprite(this.isFront);

        if (sprite != null) image.sprite = sprite;
        else this.gameObject.SetActive(false);
    }

    public void SetColors(List<Color> colors)
    {
        if (mat != null)
        {
            mat.SetInt("_UseColorGroup", 1);
            mat.SetInt("_Swap", 1);

            Clear(mat);

            int i = 0;            

            while (i < this.property.colorTables.Count && i < colors.Count)
            {
                ChangeColorGroup(i, colors[i], mat);
                i++;
            }
        }
    }

    private void ChangeColorGroup(int index, Color color, Material mat)
    {
        ColorTable colorTable = this.property.colorTables[index];

        mat.SetColor("_Color_" + ((index * 4) + 1), colorTable.highlight);
        mat.SetColor("_Color_" + ((index * 4) + 2), colorTable.main);
        mat.SetColor("_Color_" + ((index * 4) + 3), colorTable.shadows);
        mat.SetColor("_Color_" + ((index * 4) + 4), colorTable.lines);

        mat.SetColor("_New_ColorGroup_" + (index + 1), color);
    }

    private void AddGlow()
    {
        /*mat.SetInt("_AddGlow", Convert.ToInt32(this.addGlow));
        mat.SetColor("_Color_Highlight", colorTable.glow);

        this.highlightColor.r = color.r;
        this.highlightColor.g = color.g;
        this.highlightColor.b = color.b;

        mat.SetColor("_New_Highlight", this.highlightColor);*/
    }

    private void Clear(Material mat)
    {
        for (int i = 1; i <= this.maxAmount; i++)
        {
            mat.SetColor("_Color_" + i, Color.black);
            mat.SetColor("_New_Color_" + i, Color.black);
        }

        mat.SetColor("_New_ColorGroup_1", Color.black);
        mat.SetColor("_New_ColorGroup_2", Color.black);

        mat.SetColor("_Color_Highlight", Color.black);
        mat.SetColor("_New_Highlight", Color.black);
    }
}
