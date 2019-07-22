﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProtectedArea : MonoBehaviour
{
    [SerializeField]
    private List<Enemy> protectingNPCs = new List<Enemy>();

    [SerializeField]
    [Range(0, 120)]
    private float aggroIncreaseFactor = 25;

    [SerializeField]
    [Range(-120, 0)]
    private float aggroDecreaseFactor = -25f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        foreach (Enemy enemy in this.protectingNPCs)
        {
            if(enemy.gameObject.activeInHierarchy && enemy.GetComponent<AIAggroSystem>() != null)
                enemy.GetComponent<AIAggroSystem>().increaseAggro(collision.GetComponent<Character>(), this.aggroIncreaseFactor);
        }        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        foreach (Enemy enemy in this.protectingNPCs)
        {
            if (enemy.gameObject.activeInHierarchy && enemy.GetComponent<AIAggroSystem>() != null)
                enemy.GetComponent<AIAggroSystem>().decreaseAggro(collision.GetComponent<Character>(), this.aggroIncreaseFactor);
        }
    }
}
