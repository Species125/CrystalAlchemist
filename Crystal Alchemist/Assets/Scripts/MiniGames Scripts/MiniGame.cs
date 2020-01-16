﻿using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class MiniGame : MonoBehaviour
{
    [BoxGroup("Required")]
    [Required]
    public GameObject lootParentObject;

    [SerializeField]
    [BoxGroup("MiniGame Related")]
    [Required]
    private MiniGameUI uI;

    [SerializeField]
    [BoxGroup("MiniGame Related")]
    [Required]
    private MiniGameRound miniGameRound;

    [Space(10)]
    [SerializeField]
    [BoxGroup("MiniGame Related")]
    private string miniGameTitle;

    [SerializeField]
    [BoxGroup("MiniGame Related")]
    private string miniGameTitleEnglish;

    [Space(10)]
    [SerializeField]
    [BoxGroup("MiniGame Related")]
    [TextArea]
    private string miniGameDescription;

    [SerializeField]
    [BoxGroup("MiniGame Related")]
    [TextArea]
    private string miniGameDescriptionEnglish;

    private List<MiniGameMatch> matches = new List<MiniGameMatch>();

    public List<MiniGameMatch> internalMatches = new List<MiniGameMatch>();

    private MiniGameUI activeUI;

    private void Start()
    {      
        this.activeUI = Instantiate(this.uI, this.transform);
        this.activeUI.setMiniGame(this, this.miniGameRound,  
                                  this.miniGameTitle, this.miniGameTitleEnglish, this.miniGameDescription, this.miniGameDescriptionEnglish);
    }

    public void setMiniGame(List<MiniGameMatch> matches)
    {
        this.matches = matches;
    }

    public void updateInternalMatches()
    {
        CustomUtilities.UnityFunctions.UpdateItemsInEditor(this.matches, this.internalMatches, this.lootParentObject);
    }


    public List<MiniGameMatch> getMatches()
    {
        return this.internalMatches;
    }

    public void DestroyIt()
    {
        Destroy(this.gameObject);
    }

}
