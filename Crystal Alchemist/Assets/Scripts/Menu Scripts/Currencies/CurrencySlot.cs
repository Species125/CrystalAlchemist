﻿using UnityEngine;
using TMPro;

public class CurrencySlot : MonoBehaviour
{
    [SerializeField]
    private PlayerInventory playerItems;

    [SerializeField]
    private ItemGroup item;

    [SerializeField]
    private TextMeshProUGUI textField;

    public void UpdateCurrency()
    {
        int amount = this.playerItems.GetAmount(this.item);
        this.textField.text = FormatUtil.formatString(amount, this.item.maxAmount);
    }
}
