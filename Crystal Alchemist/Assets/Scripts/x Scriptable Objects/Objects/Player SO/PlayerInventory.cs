﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Player/Player Inventory")]
public class PlayerInventory : ScriptableObject
{
    [SerializeField]
    private List<ItemStats> inventory = new List<ItemStats>();
}
