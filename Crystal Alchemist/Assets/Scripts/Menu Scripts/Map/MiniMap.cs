﻿using UnityEngine;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine.UI;

public class MiniMap : MenuBehaviour
{
    [BoxGroup]
    [SerializeField]
    private PlayerTeleportList teleportList;

    [BoxGroup]
    [SerializeField]
    private TextMeshProUGUI returnName;

    [BoxGroup]
    [SerializeField]
    private Image returnIcon;

    [BoxGroup]
    [SerializeField]
    private TextMeshProUGUI lastName;

    [BoxGroup]
    [SerializeField]
    private Image lastIcon;

    public override void Start()
    {
        base.Start();
        SetText(teleportList.GetLastTeleport(), this.returnIcon, this.returnName);
        SetText(teleportList.GetNextTeleport(), this.lastIcon, this.lastName);
    }

    private void SetText(TeleportStats stats, Image image, TextMeshProUGUI textField)
    {
        if (stats == null) textField.text = "-";
        textField.text = stats.GetTeleportName();
        if (stats.icon == null) image.enabled = false;
        else image.sprite = stats.icon;
    }
}
