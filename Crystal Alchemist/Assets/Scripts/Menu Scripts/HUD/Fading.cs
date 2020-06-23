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

    private void Start() => StartCoroutine(delayCo());    

    private void FadeIn() => this.image.DOFade(0, this.transitionDuration.GetValue());

    public void FadeOut() => this.image.DOFade(0, this.transitionDuration.GetValue());

    private IEnumerator delayCo()
    {        
        yield return new WaitForSeconds(0.1f);
        FadeIn();
    }
}
