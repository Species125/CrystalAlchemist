using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.UI;
using System;

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

    public bool ignoreUpdate = false;

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

    [Button]
    public void SetColors(Color color)
    {
        List<Color> colors = new List<Color>();
        colors.Add(color);
        SetColors(colors);
    }

    public void SetColors(List<Color> colors)
    {
        if (this.mat != null)
        {
            ColorEffect effect = this.property.GetEffect();

            Clear(this.mat);

            bool canSwapColors = SetBools(colors.Count, effect.addGlow);
            if (canSwapColors) SetColorGroup(colors);

            if (effect.addGlow) AddGlow(effect);
        }
    }

    private bool SetBools(int colors, bool addGlow)
    {
        bool swapColors = false;

        this.mat.SetInt("_UseColorGroup", 1);

        if (this.property.canBeColored && colors > 0) swapColors = true;
        this.mat.SetInt("_Swap", Convert.ToInt32(swapColors));

        this.mat.SetInt("_UseGlow", Convert.ToInt32(addGlow));

        return swapColors;
    }

    private void SetColorGroup(List<Color> colors)
    {
        int i = 0;

        while (i < this.property.GetColorTable().Count && i < colors.Count)
        {
            ChangeColorGroup(i, colors[i]);
            i++;
        }
    }


    private void ChangeColorGroup(int index, Color color)
    {
        ColorTable colorTable = this.property.GetColorTable()[index];

        this.mat.SetColor("_Color_" + ((index * 4) + 1), colorTable.highlight);
        this.mat.SetColor("_Color_" + ((index * 4) + 2), colorTable.main);
        this.mat.SetColor("_Color_" + ((index * 4) + 3), colorTable.shadows);
        this.mat.SetColor("_Color_" + ((index * 4) + 4), colorTable.lines);

        this.mat.SetColor("_New_ColorGroup_" + (index + 1), color);
    }

    private void AddGlow(ColorEffect effect)
    {
        this.mat.SetColor("_SelectGlow", effect.glow);
        this.mat.SetColor("_GlowColor", effect.default_glow);
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

        this.mat.SetColor("_SelectGlow", Color.black);
        this.mat.SetColor("_GlowColor", Color.black);
    }
}
