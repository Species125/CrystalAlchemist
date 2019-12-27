﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUtils : MonoBehaviour
{
    [HideInInspector]
    public string mapID;
    [HideInInspector]
    public string areaID;

    public void changeMapLocation(string text)
    {
        this.mapID = text.Split('|')[0];
        this.areaID = text.Split('|')[1];
    }
}
