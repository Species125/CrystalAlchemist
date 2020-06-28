﻿using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CrystalOrbMenu : MenuBehaviour
{
    [BoxGroup("Crystal Orb Menu")]
    [Required]
    [SerializeField]
    private PlayerTeleportList teleportList;

    [BoxGroup("Crystal Orb Menu")]
    [Required]
    [SerializeField]
    private SavePointInfo savePointInfo;

    [BoxGroup("Crystal Orb Menu")]
    [Required]
    [SerializeField]
    private Image star;
    

    public override void Start()
    {
        base.Start();
        SetStar();
    }

    public void SetStar()
    {
        if (teleportList.lastTeleport.teleportName == savePointInfo.stats.teleportName) star.gameObject.SetActive(true);
        else star.gameObject.SetActive(false);
    }

    public void SetLastTeleport()
    {
        this.teleportList.SetLastTeleport(this.savePointInfo.stats);
    }
}
