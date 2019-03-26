﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;

public enum TargetingMode
{
    single,
    multi,
    manual, 
    autoSingle,
    autoMulti
}

public class TargetingSystem : MonoBehaviour
{
    private List<Character> targetsInRange = new List<Character>();

    public GameObject singleTargetWithMark;
    public List<GameObject> listOfTargetsWithMark = new List<GameObject>();

    [Header("Ausgewählte Ziele")]
    public List<Character> sortedTargets = new List<Character>();
    public Character currentTarget;

    [Header("Pflichtfelder")]
    public GameObject lockon;

    public Character sender;
    public StandardSkill skill;
    public string button;

    private int index = 0;
    private bool buttonPressed = false;
    public bool selectAll = false;
    private bool inputPossible = true;
    public bool targetsSet = false;

    public float durationTime = 0;
    public int maxAmountOfTargets = 1;
    public TargetingMode targetMode = TargetingMode.manual;
    public TextMeshProUGUI text;
    private float elapsed = 0;
    public List<GameObject> RangeIndicators = new List<GameObject>();
    
    void Start()
    {        
        if (this.sender == null || this.skill == null) throw new System.Exception("Sender oder Skill nicht übergeben.\nSender: " + this.sender + "\nSkill: " + this.skill);

        this.durationTime = this.skill.targetingDuration;
        this.maxAmountOfTargets = this.skill.maxAmountOfTargets;
        this.targetMode = this.skill.targetingMode;

        this.elapsed = this.durationTime;

        if (!skill.showRange)
        {
            foreach(GameObject obj in this.RangeIndicators)
            {
                obj.SetActive(false);
            }
        }
    }

    void Update()
    {
        if(this.durationTime < Utilities.maxFloatInfinite)
        {
            this.text.text = "LOCK ON\n"+Utilities.setDurationToString(this.elapsed);
        }

        if(elapsed <= 0)
        {
            for(int i = 0; i< this.listOfTargetsWithMark.Count; i++)
            {
                Destroy(this.listOfTargetsWithMark[i].gameObject);
            }

            if(this.singleTargetWithMark != null) Destroy(singleTargetWithMark.gameObject);
            this.sortedTargets.Clear();
            this.currentTarget = null;
            this.sender.activeLockOnTarget = null;
            Destroy(this.gameObject);
        }
        else
        {
            this.elapsed -= Time.deltaTime;
        }

        removeNullCharacters();
        sortNearestTargets();
        setLockOnNearestTarget();

        if (this.sortedTargets.Count > 0 
            && (this.currentTarget == null || !this.buttonPressed))
            this.currentTarget = this.sortedTargets[0];

        checkInputs();
        setActiveLockOn();
    }


    private void checkInputs()
    {
        //Button press here
        if (Input.GetButtonDown(this.button) || this.targetMode == TargetingMode.autoSingle)
        {
            this.targetsSet = true;
        }
        else if (!Input.GetButtonDown(this.button) && this.targetMode == TargetingMode.manual)
        {            
            //Switch targets
            float valueX = Input.GetAxisRaw("Cursor Horizontal");
            float valueY = Input.GetAxisRaw("Cursor Vertical");

            if (this.inputPossible)
            {
                if (valueY != 0)
                {                    
                    this.buttonPressed = true;
                    StartCoroutine(selectionMode((int)valueY));
                }
                else if (valueX != 0)
                {                   
                    this.buttonPressed = true;
                    StartCoroutine(selectTarget((int)valueX));
                }
            }
        }        
        else if (!Input.GetButtonDown(this.button) && this.targetMode == TargetingMode.multi)
        {
            StartCoroutine(selectionMode(1));
        }
        else if (this.targetMode == TargetingMode.autoMulti)
        {
            StartCoroutine(selectionMode(1));
            this.targetsSet = true;
        }       
    }



    private void setActiveLockOn()
    {
        if (this.currentTarget != null && this.singleTargetWithMark != null)
        {
            this.singleTargetWithMark.transform.parent = this.currentTarget.transform;
            this.singleTargetWithMark.transform.position = this.currentTarget.transform.position;
            this.singleTargetWithMark.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = this.currentTarget.characterName;
        }
    }

    private IEnumerator selectionMode(int value)
    {
        this.inputPossible = false;

        if (value == 1) this.selectAll = true;
        else this.selectAll = false;

        if (this.selectAll)
        {
            foreach (Character character in this.sortedTargets)
            {
                if (Utilities.hasChildWithTag(character, "LockOn") == null)
                {
                    GameObject multipleLockOns = Instantiate(this.lockon, character.transform.position, Quaternion.identity, character.transform);
                    multipleLockOns.hideFlags = HideFlags.HideInHierarchy;
                    multipleLockOns.name = multipleLockOns.name + character.characterName + Time.deltaTime;
                    multipleLockOns.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = character.characterName;
                    this.listOfTargetsWithMark.Add(multipleLockOns);
                }
            }
        }
        else
        {
            for (int i = 0; i < this.listOfTargetsWithMark.Count; i++)
            {
                Destroy(this.listOfTargetsWithMark[i].gameObject);
            }
            this.listOfTargetsWithMark.Clear();

            setLockOnNearestTarget();
        }

        yield return new WaitForSeconds(0.1f);
        this.inputPossible = true;
    }

    private IEnumerator selectTarget(int value)
    {
        this.inputPossible = false;

        this.index += value;
        if (this.index < 0) this.index = this.sortedTargets.Count - 1;
        else if (this.index >= this.sortedTargets.Count) this.index = 0;

        if (this.sortedTargets.Count > 0)
        {
            if (this.sortedTargets[this.index] != this.currentTarget || this.currentTarget == null)
            {
                this.currentTarget = this.sortedTargets[this.index];

                if(this.singleTargetWithMark != null)
                {
                    Destroy(this.singleTargetWithMark);
                    this.singleTargetWithMark = null;
                }
            }
        }

        yield return new WaitForSeconds(0.1f);
        this.inputPossible = true;
    }

    private void removeNullCharacters()
    {
        List<Character> nullCharactersToRemove = new List<Character>();

        foreach (Character character in this.targetsInRange)
        {
            if (character == null) nullCharactersToRemove.Add(character);
        }

        foreach (Character character in nullCharactersToRemove)
        {
            this.targetsInRange.Remove(character);
        }
    }

    private void sortNearestTargets()
    {
        this.sortedTargets.Clear();
        List<Character> sorted = this.targetsInRange.ToArray().OrderBy(o => (Vector3.Distance(o.transform.position, this.sender.transform.position))).ToList<Character>();

        for(int i = 0; i< this.maxAmountOfTargets && i < sorted.Count; i++)
        {
            this.sortedTargets.Add(sorted[i]);
        }
    }

    private void setLockOnNearestTarget()
    {       
        if (this.sortedTargets.Count > 0 && this.listOfTargetsWithMark.Count == 0)
        {            
            if (this.currentTarget != null)
            {
                if (Utilities.hasChildWithTag(this.currentTarget, "LockOn") == null)
                {
                    this.singleTargetWithMark = Instantiate(this.lockon, currentTarget.transform.position, Quaternion.identity, currentTarget.transform);
                    this.singleTargetWithMark.name = this.singleTargetWithMark.name + currentTarget.characterName + Time.deltaTime;
                    this.singleTargetWithMark.hideFlags = HideFlags.HideInHierarchy;
                }
            }
        }
        else if (this.sortedTargets.Count == 0 || this.listOfTargetsWithMark.Count > 0)
        {
            Destroy(this.singleTargetWithMark);
            this.currentTarget = null;
            this.singleTargetWithMark = null;
            this.buttonPressed = false;
        }
    }

    public static bool getChildFromTag(Character character, string searchTag)
    {
        bool found = false;

        for (int i = 0; i < character.transform.childCount; i++)
        {
            if (character.transform.GetChild(i).tag == searchTag)
            {
                found = true;
                break;
            }
        }

        return found;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (Utilities.checkCollision(collision, this.skill) && collision.GetComponent<Character>() != null)
            this.targetsInRange.Add(collision.GetComponent<Character>());
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (Utilities.checkCollision(collision, this.skill) && collision.GetComponent<Character>() != null)
        {
            GameObject lockOn = Utilities.hasChildWithTag(collision.GetComponent<Character>(), "LockOn");
            if (this.singleTargetWithMark == lockOn) this.singleTargetWithMark = null;
            this.listOfTargetsWithMark.Remove(lockon);
            Destroy(lockOn);
            this.targetsInRange.Remove(collision.GetComponent<Character>());
        }
    }
}
