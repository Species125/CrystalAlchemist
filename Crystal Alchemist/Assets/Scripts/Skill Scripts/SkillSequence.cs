﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

#region Attributes

public enum modificationType
{
    none,
    target,
    sender,
    randomArea,
    random,
    fix,
    normalized
}

[System.Serializable]
public class customSequence
{
    public GameObject gameObject;

    public modificationType position = modificationType.none;
    public bool initActive = false;
    public float spawnDelay = 0f;

    [ShowIf("position", modificationType.randomArea)]
    public GameObject min;
    [ShowIf("position", modificationType.randomArea)]
    public GameObject max;

    [ShowIf("position", modificationType.random)]
    public List<GameObject> spawnPoints = new List<GameObject>();

    public modificationType rotation = modificationType.none;
    [ShowIf("rotation", modificationType.random)]
    [Range(1, 8)]
    public int randomRotations;
}

#endregion

public class SkillSequence : MonoBehaviour
{
    [SerializeField]
    private Character sender;

    [SerializeField]
    private Character target;

    [SerializeField]
    private List<customSequence> modifcations = new List<customSequence>();

    private bool useDurationTime = false;
    private float duration = 0;
    private float timeElapsed = 0;

    private void Start()
    {
        setChildObjects();
        initModification();
    }

    private void Update()
    {
        this.timeElapsed += Time.deltaTime;

        if ((this.useDurationTime && this.timeElapsed >= this.duration) || this.noActiveGameObjects())
        {
            this.DestroyIt();
        }

        activateGameObject();
    }

    private bool noActiveGameObjects()
    {
        foreach(customSequence custom in this.modifcations)
        {
            if (custom.gameObject != null) return false;
        }

        return true;
    }

    private void activateGameObject()
    {
        foreach(customSequence mod in this.modifcations)
        {
            if (mod.gameObject != null && this.timeElapsed >= mod.spawnDelay)
            {
                if (!mod.gameObject.activeInHierarchy)
                {
                    Skill skill = mod.gameObject.GetComponent<Skill>();

                    if (skill != null)
                    {
                        if (skill.GetComponent<SkillAnimationModule>() != null) skill.GetComponent<SkillAnimationModule>().showCastingAnimation();
                    }
                    else
                    {
                        mod.gameObject.SetActive(true);
                    }
                }
            }
        }
    }

    /////////////////////////////////////////

    #region Init

    private void initModification()
    {
        foreach (customSequence modification in this.modifcations)
        {
            if(!modification.initActive) modification.gameObject.SetActive(false);
            setPosition(modification);
            setRotation(modification);
        }
    }

    private void setPosition(customSequence modification)
    {
        if (modification.position == modificationType.randomArea)
        {
            //set Position in Area
            modification.gameObject.transform.position = getRandomPosition(modification.min.transform.position, modification.max.transform.position);
        }
        else if (modification.position == modificationType.random)
        {
            //set Position of a set of Spawn-Points
            modification.gameObject.transform.position = getRandomPosition(modification.spawnPoints);
        }
        else if (modification.position == modificationType.target)
        {
            //set Position on Target
            modification.gameObject.transform.position = this.target.transform.position;
            if (this.target.shadowRenderer != null) modification.gameObject.transform.position = this.target.shadowRenderer.transform.position;
        }
        else if (modification.position == modificationType.sender)
        {
            //set Position on Sender
            modification.gameObject.transform.position = this.sender.transform.position;
        }
    }

    private void setRotation(customSequence modification)
    {
        if (modification.rotation == modificationType.random)
        {
            modification.gameObject.transform.rotation = Quaternion.Euler(0, 0, getRandomRotation(modification.randomRotations));
        }
        else if (modification.rotation == modificationType.target)
        {
            Vector2 direction = (this.target.transform.position - modification.gameObject.transform.position).normalized;
            float angle = (Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg);

            modification.gameObject.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        }
        else if (modification.rotation == modificationType.normalized)
        {
            modification.gameObject.transform.rotation = Quaternion.identity;
        }
    }

    private void setChildObjects()
    {
        if (this.sender != null)
        {
            for (int i = 0; i < this.transform.childCount; i++)
            {
                Skill childSkill = this.transform.GetChild(i).GetComponent<Skill>();
                if (childSkill != null)
                {
                    childSkill.sender = this.sender;
                    childSkill.target = this.target;
                    childSkill.overridePosition = false;
                }
            }
        }
    }

    #endregion

    /////////////////////////////////////////

    #region Trigger

    public void DestroyIt()
    {
        Destroy(this.gameObject);
    }

    #endregion

    /////////////////////////////////////////

    #region Random Functions

    public void setSender(Character sender)
    {
        this.sender = sender;
    }

    public void setTarget(Character target)
    {
        this.target = target;
    }

    public void setPosition(modificationType positionType, Vector2 fixPosition)
    {
        switch (positionType)
        {
            case modificationType.fix: this.transform.position = fixPosition; break;
            case modificationType.sender: this.transform.position = this.sender.transform.position; break;
            case modificationType.target: this.transform.position = this.target.transform.position; break;
        }
    }

    private int getRandomRotation(int randomRotations)
    {
        int rng = Random.Range(1, randomRotations);
        int result = (360 / randomRotations) * rng;
        return result;
    }

    private Vector2 getRandomPosition(List<GameObject> positions)
    {
        int rng = Random.Range(0, positions.Count - 1);
        return positions[rng].transform.position;
    }

    private Vector2 getRandomPosition(Vector2 min, Vector2 max)
    {
        float x = Random.Range(min.x, max.x);
        float y = Random.Range(min.y, max.y);

        return new Vector2(x, y);
    }

    #endregion

}
