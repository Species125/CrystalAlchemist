﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ChangeVolumeMenu : TitleScreenMenues
{
    [SerializeField]
    private FloatSignal musicVolumeSignal;

    [SerializeField]
    private bool isTitleScreen = false;

    private void OnEnable()
    {        
        this.slider.value = (getVolumeFromEnum() * 100f);
        setVolumeText(this.slider.value);
    }

    private float getVolumeFromEnum()
    {
        switch (this.volumeType)
        {
            case VolumeType.effects: return GlobalValues.soundEffectVolume;
            case VolumeType.music: return GlobalValues.backgroundMusicVolume;
        }

        return 0;
    }

    public void ChangeVolume()
    {
        if (this.volumeType == VolumeType.music)
        {
            GlobalValues.backgroundMusicVolume = (this.slider.value / 100f);                     

            if(!this.isTitleScreen) this.musicVolumeSignal.Raise(GlobalValues.getMusicInMenu());
            else this.musicVolumeSignal.Raise(GlobalValues.backgroundMusicVolume);
        }
        else if (this.volumeType == VolumeType.effects)
        {
            GlobalValues.soundEffectVolume = (this.slider.value / 100f);            
        }

        setVolumeText(this.slider.value);
    }

    private void setVolumeText(float volume)
    {        
        this.textField.text = Mathf.RoundToInt(volume) + "%";
    }    
}
