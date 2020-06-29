﻿using UnityEngine;
using TMPro;
using Sirenix.OdinInspector;
using UnityEngine.UI;

public class ChangeVolumeMenu : MonoBehaviour
{
    [SerializeField]
    private bool isTitleScreen = false;

    [BoxGroup("Volume Buttons")]
    [SerializeField]
    private TextMeshProUGUI textField;
    [BoxGroup("Volume Buttons")]
    [SerializeField]
    private Slider slider;
    [BoxGroup("Volume Buttons")]
    [SerializeField]
    private VolumeType volumeType;

    private void OnEnable()
    {        
        this.slider.value = (getVolumeFromEnum() * 100f);
        setVolumeText(this.slider.value);
    }

    private float getVolumeFromEnum()
    {
        switch (this.volumeType)
        {
            case VolumeType.effects: return MasterManager.settings.soundEffectVolume;
            case VolumeType.music: return MasterManager.settings.backgroundMusicVolume;
        }

        return 0;
    }

    public void ChangeVolume()
    {
        if (this.volumeType == VolumeType.music)
        {
            MasterManager.settings.backgroundMusicVolume = (this.slider.value / 100f);

            if (!this.isTitleScreen) MusicEvents.current.ChangeVolume(MasterManager.settings.GetMenuVolume());
            else MusicEvents.current.ChangeVolume(MasterManager.settings.backgroundMusicVolume);
        }
        else if (this.volumeType == VolumeType.effects)
        {
            MasterManager.settings.soundEffectVolume = (this.slider.value / 100f);            
        }

        setVolumeText(this.slider.value);
    }

    private void setVolumeText(float volume)
    {        
        this.textField.text = Mathf.RoundToInt(volume) + "%";
    }    
}
