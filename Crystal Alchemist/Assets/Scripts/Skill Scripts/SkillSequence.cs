﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public enum modificationType
{
    none,
    target, 
    random
}

[System.Serializable]
public class customSequence
{
    public GameObject gameObject;
    public modificationType position = modificationType.none;

    [ShowIf("position", modificationType.random)]
    public Vector2 max;

    [ShowIf("position", modificationType.random)]
    public Vector2 min;

    public modificationType rotation = modificationType.none;

    [ShowIf("rotation", modificationType.random)]
    [Range(1, 8)]
    public int randomRotations;
}

public class SkillSequence : MonoBehaviour
{
    [SerializeField]
    private Character sender;

    [SerializeField]
    private Character target;

    [SerializeField]
    private List<customSequence> modifcations = new List<customSequence>();

    //TODO: no position override
    //TODO: rotate it
    //TODO: Randomize (Rotation, Position, etc) 
    //TODO: Spawn Adds
    //TODO: Drop Item 

    public void setSender(Character sender)
    {
        this.sender = sender;
    }

    public void setTarget(Character target)
    {
        this.target = target;
    }

    private int getRandomRotation(int randomRotations)
    {
        int rng = Random.Range(1, randomRotations);
        int result = (360 / randomRotations) * rng;
        return result;
    }

    private Vector2 getRandomPosition(Vector2 min, Vector2 max)
    {
        float x = Random.Range(min.x, max.x);
        float y = Random.Range(min.y, max.y);

        return new Vector2(x, y);
    }

    private void Start()
    {
        setChildObjects();
        initModification();
    }

    private void initModification()
    {
        foreach (customSequence modification in this.modifcations)
        {
            if (modification.position == modificationType.random)
            {
                modification.gameObject.transform.position = getRandomPosition(modification.min, modification.max);
            }
            else if (modification.position == modificationType.random)
            {
                modification.gameObject.transform.position = this.target.transform.position;
            }

            if (modification.rotation == modificationType.random)
            {
                modification.gameObject.transform.rotation = Quaternion.Euler(0, 0, getRandomRotation(modification.randomRotations));
            }
            else if (modification.rotation == modificationType.random)
            {
                Vector2 direction = (this.target.transform.position - modification.gameObject.transform.position).normalized;
                float angle = (Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg);

                modification.gameObject.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
            }
        }
    }

    private void setChildObjects()
    {
        if (this.sender != null)
        {
            for (int i = 0; i < this.transform.childCount; i++)
            {
                StandardSkill childSkill = this.transform.GetChild(i).GetComponent<StandardSkill>();
                if (childSkill != null)
                {
                    childSkill.sender = this.sender;
                    childSkill.setPositionAtStart = false;
                }
            }
        }
    }

    public void DestroyIt()
    {
        Destroy(this.gameObject);
    }
}
