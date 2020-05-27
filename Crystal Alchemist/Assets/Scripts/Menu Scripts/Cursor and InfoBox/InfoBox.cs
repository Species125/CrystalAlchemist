﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class InfoBox : MonoBehaviour
{
    [SerializeField]
    private Image previewImage;

    [SerializeField]
    private TextMeshProUGUI nameField;

    [SerializeField]
    private TextMeshProUGUI descriptionField;

    [SerializeField]
    private GameObject additionalInfo;

    [SerializeField]
    private Image statusEffectPreviewImage;

    [SerializeField]
    private TextMeshProUGUI statusEffectNameField;

    [SerializeField]
    private TextMeshProUGUI statusEffectDescriptionField;


    #region setInfo

    private void setInfo(Ability ability)
    {
        this.additionalInfo.SetActive(false);

        this.nameField.text = ability.GetName();

        if (ability.hasSkillBookInfo && ability.info != null)
        {
            this.previewImage.sprite = ability.info.icon;
            this.descriptionField.text = ability.info.getDescription();
        }

        if (ability.skill.GetComponent<SkillTargetModule>() != null && ability.skill.GetComponent<SkillTargetModule>().statusEffects.Count > 0)
        {
            this.additionalInfo.SetActive(true);

            StatusEffect statusEffect = ability.skill.GetComponent<SkillTargetModule>().statusEffects[0];

            this.statusEffectPreviewImage.sprite = statusEffect.iconSprite;
            this.statusEffectNameField.text = statusEffect.GetName();
            this.statusEffectDescriptionField.text = statusEffect.GetDescription();
        }
    }

    private void setInfo(ItemGroup item)
    {
        this.additionalInfo.SetActive(false);

        this.previewImage.sprite = item.info.getSprite();

        this.nameField.text = item.getName();
        this.descriptionField.text = item.info.getDescription();
    }


    private void setInfo(ItemStats item)
    {
        this.additionalInfo.SetActive(false);

        this.previewImage.sprite = item.getSprite();

        this.nameField.text = item.getName();
        this.descriptionField.text = item.getDescription();
    }

    private void setInfo(CharacterAttributeStats stats)
    {
        this.additionalInfo.SetActive(false);

        this.previewImage.sprite = stats.icon.sprite;
        this.nameField.text = stats.GetName();
        this.descriptionField.text = stats.GetDescription();
    }


    #endregion


    #region show and hide

    public void Hide()
    {
        this.gameObject.SetActive(false);
    }

    public void Show(Ability ability)
    {
        this.gameObject.SetActive(true);
        setInfo(ability);
    }

    public void Show(ItemStats item)
    {
        this.gameObject.SetActive(true);
        setInfo(item);
    }

    public void Show(ItemGroup item)
    {
        this.gameObject.SetActive(true);
        setInfo(item);
    }

    public void Show(CharacterAttributeStats stats)
    {
        this.gameObject.SetActive(true);
        setInfo(stats);
    }
    #endregion
}