﻿using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class Fading : MonoBehaviour
{
    [SerializeField]
    private Image image;

    [SerializeField]
    private FloatValue transitionDuration;

    [SerializeField]
    private Color colorFadeIn = Color.black;

    [SerializeField]
    private Color colorFadeOut = Color.white;

    private void Start()
    {
        StartCoroutine(delayCo());
        MenuEvents.current.OnFadeOut += FadeOut;
    }

    private void OnDestroy()
    {
        MenuEvents.current.OnFadeOut -= FadeOut;
    }

    private void FadeIn()
    {
        this.image.DOFade(1, 0);
        this.image.DOFade(0, this.transitionDuration.GetValue());
    }

    public void FadeOut()
    {
        this.image.DOFade(0, 0);
        this.image.DOFade(1, this.transitionDuration.GetValue());
    }

    private IEnumerator delayCo()
    {        
        yield return new WaitForSeconds(0.1f);
        FadeIn();
    }
}
