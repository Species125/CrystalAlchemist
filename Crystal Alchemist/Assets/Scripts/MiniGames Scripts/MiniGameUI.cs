﻿using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine.UI;

public class MiniGameUI : MenuControls
{
    public MiniGameRound miniGameRound;

    [BoxGroup("Easy Access")]
    [SerializeField]
    [Required]
    private TextMeshProUGUI timeField;

    [BoxGroup("Easy Access")]
    [SerializeField]
    [Required]
    private Image timeImage;

    [BoxGroup("Easy Access")]
    [SerializeField]
    [Required]
    private TextMeshProUGUI titleField;

    [BoxGroup("Easy Access")]
    [SerializeField]
    [Required]
    private TextMeshProUGUI descriptionField;

    [BoxGroup("Easy Access")]
    [SerializeField]
    [Required]
    private GameObject mainBoard;

    [BoxGroup("Easy Access")]
    [SerializeField]
    [Required]
    private GameObject textGameObject;

    [BoxGroup("Easy Access")]
    [SerializeField]
    [Required]
    private MiniGameTrys trySlots;

    [BoxGroup("Easy Access")]
    [SerializeField]
    [Required]
    private MiniGameDialogbox dialogBox;


    [BoxGroup("Texts")]
    [SerializeField]
    [Required]
    private MiniGameText successText;

    [BoxGroup("Texts")]
    [SerializeField]
    [Required]
    private MiniGameText failText;

    [BoxGroup("Texts")]
    [SerializeField]
    [Required]
    private MiniGameText winText;

    [BoxGroup("Texts")]
    [SerializeField]
    [Required]
    private MiniGameText loseText;

    private MiniGame miniGameObject;
    private MiniGameRound activeRound;
    private MiniGameMatch match;
    private int matchIndex = 0;

    [HideInInspector]
    public string mainDescription = "";

    private void Start()
    {
        this.dialogBox.setValues(this.miniGameObject.getMatches().Count);
        showDialog();
    }

    public override void Update()
    {
        base.Update();
        if (this.activeRound != null)
        {
            this.timeField.text = (int)this.activeRound.getSeconds() + "s";
            this.timeImage.fillAmount = (float)((float)this.activeRound.getSeconds() / this.match.maxDuration);
        }
    }

    public void setMiniGame(MiniGame main, MiniGameRound miniGameRound, List<MiniGameMatch> matches, 
                            string title, string titleEnglish, string description, string descriptionEnglish)
    {
        this.miniGameObject = main;
        this.miniGameRound = miniGameRound;        
        this.titleField.text = Utilities.Format.getLanguageDialogText(title, titleEnglish); ;
        this.mainDescription = Utilities.Format.getLanguageDialogText(description, descriptionEnglish);
    }

    public void startRound()
    {
        MiniGameState state = this.trySlots.canStartNewRound();

        if (state == MiniGameState.play)
        {
            endRound();

            this.activeRound = Instantiate(this.miniGameRound, this.mainBoard.transform);
            this.activeRound.setParameters(this.match.maxDuration, (this.matchIndex + 1), this.match.difficulty, this.cursor, this);
        }
        else
        {
            this.activeRound.stopTimer();

            if (state == MiniGameState.win)
            {
                this.player.collect(this.match.loot, false);
                showTexts(this.winText);
            }
            else if (state == MiniGameState.lose)
            {
                showTexts(this.loseText);
            }
        }
    }

    public void resetTrys()
    {
        this.trySlots.reset();
    }

    public void setMatch(int difficulty)
    {
        this.matchIndex = difficulty - 1;

        if (this.miniGameObject.getMatches().Count > 0)
        {
            this.match = this.miniGameObject.getMatches()[this.matchIndex];
            this.trySlots.setValues(this.match.winsNeeded, this.match.maxRounds);
        }
        resetTrys();
    }

    public MiniGameMatch getMatch()
    {
        return this.match;
    }

    public void startMatch()
    {
        Utilities.Items.reduceCurrency(ResourceType.item, match.item, this.player, match.price);
        startRound();
    }

    public void setMarkAndEndRound(bool success) //SIGNAL
    {
        MiniGameState state = this.trySlots.canStartNewRound();

        if (success)
        {
            this.trySlots.updateSlots(true);
            if (state == MiniGameState.play) showTexts(this.successText);
        }
        else
        {
            this.trySlots.updateSlots(false);
            if (state == MiniGameState.play) showTexts(this.failText);
        }
    }

    private void showTexts(MiniGameText textObject)
    {
        if (textObject != null) textObject.gameObject.SetActive(true);
    }

    private void endRound()
    {
        if (this.activeRound != null) Destroy(this.activeRound.gameObject);
    }

    public void endMiniGame()
    {
        this.gameObject.SetActive(false);
    }

    public void showDialog()
    {
        endRound();
        this.dialogBox.gameObject.SetActive(true);
    }

    public override void OnDisable()
    {
        base.OnDisable();
        this.miniGameObject.DestroyIt();
    }

    private void OnDestroy()
    {
        this.exitMenu();
    }

}
