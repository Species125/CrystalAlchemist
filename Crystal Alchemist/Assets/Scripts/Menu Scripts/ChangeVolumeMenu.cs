﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Sirenix.OdinInspector;
using UnityEngine.UI;

public class ChangeVolumeMenu : MonoBehaviour
{
    [SerializeField]
    private FloatSignal musicVolumeSignal;

    [SerializeField]
    private bool isTitleScreen = false;

    [BoxGroup("Volume Buttons")]
    public TextMeshProUGUI textField;
    [BoxGroup("Volume Buttons")]
    public Slider slider;
    [BoxGroup("Volume Buttons")]
    public VolumeType volumeType;

    private void OnEnable()
    {        
        this.slider.value = (getVolumeFromEnum() * 100f);
        setVolumeText(this.slider.value);
    }

    private float getVolumeFromEnum()
    {
        switch (this.volumeType)
        {
            case VolumeType.effects: return GlobalGameObjects.settings.soundEffectVolume;
            case VolumeType.music: return GlobalGameObjects.settings.backgroundMusicVolume;
        }

        return 0;
    }

    public void ChangeVolume()
    {
        if (this.volumeType == VolumeType.music)
        {
            GlobalGameObjects.settings.backgroundMusicVolume = (this.slider.value / 100f);                     

            if(!this.isTitleScreen) this.musicVolumeSignal.Raise(GlobalGameObjects.settings.getMusicInMenu());
            else this.musicVolumeSignal.Raise(GlobalGameObjects.settings.backgroundMusicVolume);
        }
        else if (this.volumeType == VolumeType.effects)
        {
            GlobalGameObjects.settings.soundEffectVolume = (this.slider.value / 100f);            
        }

        setVolumeText(this.slider.value);
    }

    private void setVolumeText(float volume)
    {        
        this.textField.text = Mathf.RoundToInt(volume) + "%";
    }    
}
