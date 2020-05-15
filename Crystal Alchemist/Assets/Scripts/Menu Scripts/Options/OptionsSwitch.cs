﻿using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

public enum VolumeType
{
    music,
    effects
}

public class OptionsSwitch : MonoBehaviour
{
    [BoxGroup("Switch Buttons")]
    public Image firstButton;
    [BoxGroup("Switch Buttons")]
    public Image secondButton;    

    public void switchButtons(Image active, Image notActive)
    {
        active.color = setAlpha(active, 1f);
        notActive.color = setAlpha(notActive, 0.2f);
    }

    private Color setAlpha(Image image, float transparency)
    {
        return new Color(image.color.r, image.color.g, image.color.b, transparency);
    }
}
