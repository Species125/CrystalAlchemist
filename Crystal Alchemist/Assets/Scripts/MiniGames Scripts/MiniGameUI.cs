﻿using UnityEngine;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine.UI;

public class MiniGameUI : MenuBehaviour
{
    public PlayerInventory inventory;

    [BoxGroup("Mandatory")]
    [SerializeField]
    [Required]
    private CustomCursor cursor;

    [BoxGroup("Mandatory")]
    [SerializeField]
    [Required]
    private MiniGameInfo UIInfo;

    [HideInInspector]
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

    private MiniGameInfo info;
    private MiniGameRound activeRound;
    private MiniGameMatch match;
    private int matchIndex = 0;    

    [HideInInspector]
    public string mainDescription = "";

    public override void Start()
    {
        base.Start();
        SetMiniGame();
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

    public void SetMiniGame()
    {
        this.info = this.UIInfo;

        this.miniGameRound = this.info.miniGameUI;
        this.titleField.text = this.info.GetName();
        this.mainDescription = this.info.GetDescription();        

        showDialog();
        this.dialogBox.setValues(this.info.matches.GetMatches().Count);
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
                GameEvents.current.DoCollect(this.match.getItem().stats);
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
        
        if (this.info.matches.GetMatches().Count > 0)
        {
            this.match = this.info.matches.GetMatches()[this.matchIndex];
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
        GameEvents.current.DoReduce(match.price);
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
        this.ExitMenu();
    }

    public void showDialog()
    {
        endRound();

        this.dialogBox.UpdateDialogBox();
        this.dialogBox.gameObject.SetActive(true);
    }
}
