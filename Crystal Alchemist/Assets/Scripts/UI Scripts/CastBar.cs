﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CastBar : MonoBehaviour
{
    [SerializeField]
    private Image charging;
    [SerializeField]
    private Image full;
    [SerializeField]
    private TextMeshProUGUI skillName;
    [SerializeField]
    private TextMeshProUGUI percentage;

    private Ability skill;


    public void setCastBar(Character character, Ability skill)
    {
        this.transform.position = new Vector2(this.transform.position.x, this.transform.position.y + 1f);
        this.transform.parent = character.transform;
        this.skill = skill;
        this.skillName.text = this.skill.name;
    }

    private void Update()
    {
        float percent = skill.holdTimer / this.skill.castTime;
        this.charging.fillAmount = (percent);

        string text = (int)(percent * 100) + "%";
        if (percent * 100 >= 100) text = "BEREIT!";

        this.percentage.text = text;

        if (skill.holdTimer >= this.skill.castTime) this.full.enabled = true;
        else this.full.enabled = false;
    }

    public void destroyIt()
    {
        Destroy(this.gameObject, 0.1f);
    }
}
