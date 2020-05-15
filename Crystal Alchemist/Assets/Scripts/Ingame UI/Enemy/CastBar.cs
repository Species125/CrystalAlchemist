﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CastBar : MonoBehaviour
{
    [SerializeField]
    private SpriteFillBar charging;
    [SerializeField]
    private SpriteRenderer full;
    [SerializeField]
    private TextMeshPro skillName;
    [SerializeField]
    private TextMeshPro percentage;

    private Ability ability;


    public void setCastBar(Character character, Ability ability)
    {
        this.transform.parent = character.transform;
        this.transform.position = new Vector2(this.transform.position.x, this.transform.position.y + 1f);
        this.ability = ability;
        this.skillName.text = this.ability.GetName();
    }

    private void Update()
    {
        float percent = ability.holdTimer / this.ability.castTime;
        this.charging.fillAmount(percent);

        string text = (int)(percent * 100) + "%";
        if (percent * 100 >= 100) text = "BEREIT!";

        this.percentage.text = text;

        if (ability.holdTimer >= this.ability.castTime) this.full.enabled = true;
        else this.full.enabled = false;
    }

    public void destroyIt()
    {
        Destroy(this.gameObject, 0.1f);
    }
}
