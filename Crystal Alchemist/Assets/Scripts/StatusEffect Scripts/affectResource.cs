﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[System.Serializable]
public struct affectedResource
{
    public ResourceType resourceType;

    [ShowIf("resourceType", ResourceType.item)]
    [Tooltip("Benötigtes Item")]
    public Item item;

    [Range(-Utilities.maxFloatInfinite, Utilities.maxFloatInfinite)]
    public float amount;
}

public class affectResource : StatusEffect
{
    #region Attributes
    public List<affectedResource> affectedResources;
    #endregion


    #region Overrides
    public override void doEffect()
    {
        foreach (affectedResource resource in this.affectedResources)
        {
            base.doEffect();
            this.target.updateResource(resource.resourceType, resource.item, resource.amount);
        }
    }
    #endregion
}
