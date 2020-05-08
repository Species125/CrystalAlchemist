﻿using UnityEngine;
using Sirenix.OdinInspector;

public class SkillPositionZModule : SkillModule
{
    [BoxGroup("Position Z")]
    [Range(-1, 2)]
    [Tooltip("Positions-Höhe")]
    public float positionHeight = 0f;

    [BoxGroup("Position Z")]
    [Tooltip("Schattencollider Höhe")]
    [Range(-1, 0)]
    public float colliderHeightOffset = -0.5f;
}
