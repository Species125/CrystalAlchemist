﻿using UnityEngine;
using Sirenix.OdinInspector;
using TMPro;

public class ShopPrice : MonoBehaviour
{
    [BoxGroup("Text-Attribute")]
    [SerializeField]
    [Required]
    private GameObject child;

    [BoxGroup("Text-Attribute")]
    [SerializeField]
    private TextMeshPro priceText;

    [BoxGroup("Text-Attribute")]
    [SerializeField]
    private SpriteRenderer priceIcon;

    public void Initialize(Costs costs)
    {
        if (costs.resourceType == CostType.item && costs.item != null)
        {
            this.child.SetActive(true);
            FormatUtil.set3DText(this.priceText, costs.amount + "", true, costs.item.color, costs.item.outline, 0.25f);
            this.priceIcon.sprite = costs.item.shopIcon;
        }
        else this.child.SetActive(false);
    }
}
