﻿using System;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ResolutionMenu : MonoBehaviour
{
    private FullScreenMode fullscreen = FullScreenMode.Windowed;
    private Vector2Int resolution = new Vector2Int(1920, 1080);

    [SerializeField]
    private TMP_Dropdown resolutionDropDown;

    [SerializeField]
    private TMP_Dropdown screenModeDropDown;

    [SerializeField]
    private Slider cameraSlider;

    [SerializeField]
    private TextMeshProUGUI cameraSliderValue;

    [SerializeField]
    private Slider UISlider;

    [SerializeField]
    private TextMeshProUGUI UISliderValue;

    private int cameraSize;
    private float UIsize;

    private void OnEnable()
    {
        SelectDropDown(this.resolutionDropDown, GetCurrentResolution());
        SelectDropDown(this.screenModeDropDown, GetCurrentScreenMode());
        SetSliders();
    }

    private void SelectDropDown(TMP_Dropdown dropDown, string text)
    {
        for (int i = 0; i < dropDown.options.Count; i++)
        {
            if (dropDown.options[i].text == text) { dropDown.value = i; break; }
        }
    }

    private void SetSliders()
    {
        this.cameraSize = MasterManager.settings.cameraDistance;
        this.cameraSlider.value = cameraSize;
        this.cameraSliderValue.text = this.cameraSize + "";

        this.UIsize = MasterManager.settings.UISize;
        this.UISlider.value = UIsize*100;
        this.UISliderValue.text = this.UIsize + "";
    }

    public void SetCameraValue()
    {
        this.cameraSize = (int)this.cameraSlider.value;
        this.cameraSliderValue.text = this.cameraSize + "";
    }

    public void SetUIValue()
    {
        this.UIsize = (int)this.UISlider.value;
        this.UISliderValue.text = this.UIsize + "";
    }

    public void ConfirmCamera()
    {
        MasterManager.settings.cameraDistance = this.cameraSize;
        SettingsEvents.current.DoCameraChange();

        MasterManager.settings.UISize = this.UIsize/100f;
        SettingsEvents.current.DoUISizeChange();
    }

    public void Confirm()
    {
        Screen.SetResolution(this.resolution.x, this.resolution.y, this.fullscreen);
        this.resolutionDropDown.GetComponent<ButtonExtension>().Select();        
    }

    public void ChangeResolution()
    {
        string[] resolution = this.resolutionDropDown.captionText.text.Split('x');
        int x = Convert.ToInt32(resolution[0]);
        int y = Convert.ToInt32(resolution[1]);
        this.resolution = new Vector2Int(x, y);
    }

    public void ChangeFullScreen()
    {
        string text = this.screenModeDropDown.captionText.text;
        switch (text)
        {
            case "Fullscreen": this.fullscreen = FullScreenMode.FullScreenWindow; break;
            case "Window": this.fullscreen = FullScreenMode.Windowed; break;
        }
    }

    private string GetCurrentScreenMode()
    {
        switch (Screen.fullScreenMode)
        {
            case FullScreenMode.FullScreenWindow: return "Fullscreen";
            case FullScreenMode.Windowed: return "Window";
        }
        return "";
    }

    private string GetCurrentResolution()
    {
        Resolution resolution = Screen.currentResolution;
        return resolution.width + "x" + resolution.height;
    }
}
