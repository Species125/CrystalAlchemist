﻿using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(menuName = "Game/Settings/Global Game Objects")]
public class GlobalGameObjects : ScriptableObject
{
    public static DamageNumbers damageNumber;
    public static ContextClue contextClue;
    public static GameObject markAttack;
    public static GameObject markTarget;
    public static MiniDialogBox miniDialogBox;
    public static CastBar castBar;
    public static AnalyseInfo analyseInfo;
    public static GameSettings settings;
    public static GlobalValues staticValues;

    [BoxGroup("Interaction")]
    [SerializeField]
    private ContextClue context;
    [BoxGroup("Interaction")]
    [SerializeField]
    private AnalyseInfo analyse;

    [BoxGroup("Combat")]
    [SerializeField]
    private DamageNumbers damage;
    [BoxGroup("Combat")]
    [SerializeField]
    private CastBar cast;

    [BoxGroup("Bubbles")]
    [SerializeField]
    private MiniDialogBox dialog;
    [BoxGroup("Bubbles")]
    [SerializeField]
    private GameObject attacking;
    [BoxGroup("Bubbles")]
    [SerializeField]
    private GameObject targeting;

    [BoxGroup("Settings")]
    [SerializeField]
    private GameSettings gameSettings;
    [BoxGroup("Settings")]
    [SerializeField]
    private GlobalValues globalValues;


    public void Initialize()
    {
        contextClue = this.context;
        damageNumber = this.damage;
        miniDialogBox = this.dialog;
        markAttack = this.attacking;
        markTarget = this.targeting;
        castBar = this.cast;
        analyseInfo = this.analyse;
        settings = this.gameSettings;
        staticValues = this.globalValues;
    }
}
