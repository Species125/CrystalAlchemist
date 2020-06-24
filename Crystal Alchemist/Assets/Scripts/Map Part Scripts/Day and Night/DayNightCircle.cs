﻿using System.Collections;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

[RequireComponent(typeof(Light2D))]
public class DayNightCircle : MonoBehaviour
{
    private Light2D Lighting;
    private bool isRunning = false;
    private TimeValue timeValue;

    private void Start()
    {
        this.Lighting = this.GetComponent<Light2D>();
        //this.isActive = MasterManager.debugSettings.activateLight;
        this.timeValue = MasterManager.timeValue;
        this.Lighting.color = this.timeValue.GetColor();
    }

    IEnumerator startCo()
    {
        yield return new WaitForEndOfFrame();
        this.Lighting.color = this.timeValue.GetColor();
    }

    public void changeColor()
    {
        StopAllCoroutines();
        Color newColor = this.timeValue.GetColor();
        float duration = this.timeValue.factor * (this.timeValue.update - 1);
        if (newColor != Lighting.color && !this.isRunning) StartCoroutine(lerpColor(Lighting, Lighting.color, newColor, duration));
    }

    IEnumerator lerpColor(Light2D light, Color fromColor, Color toColor, float duration)
    {
        this.isRunning = true;
        float counter = 0;

        while (counter < duration)
        {
            counter += Time.deltaTime;

            float colorTime = counter / duration;

            //Change color
            light.color = Color.Lerp(fromColor, toColor, counter / duration);

            //Wait for a frame
            yield return null;
        }

        this.isRunning = false;
    }
}
