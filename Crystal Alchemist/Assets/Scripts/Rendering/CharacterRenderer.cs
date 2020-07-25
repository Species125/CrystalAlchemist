using System.Collections.Generic;
using UnityEngine;

public class CharacterRenderer : CustomRenderer
{
    private List<Color> colors = new List<Color>();

    public void Reset()
    {
        this.colors.Clear();
        ChangeTint(Color.white, false);
    }

    public void RemoveTint(Color color)
    {
        this.colors.Remove(color);
        if (colors.Count > 0) ChangeTint(this.colors[this.colors.Count - 1], true);
        else ChangeTint(Color.white, false);
    }

    private void ChangeTint(Color color, bool useTint)
    {
        this.material.SetFloat("_Use_Tint", useTint ? 1f : 0f);
        this.material.SetColor("_Tint", color);        
    }

    public void ChangeTint(Color color)
    {
        if (this.colors.Contains(color))
        {
            ChangeTint(this.colors[this.colors.IndexOf(color)], true);
        }
        else
        {
            this.colors.Add(color);
            ChangeTint(this.colors[this.colors.Count - 1], true);
        }
    }
}
