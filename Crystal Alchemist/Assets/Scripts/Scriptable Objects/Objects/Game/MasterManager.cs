﻿using UnityEngine;
using Sirenix.OdinInspector;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Game/Settings/Global Game Objects")]
public class MasterManager : SingletonScriptableObject<MasterManager>
{
    public static DamageNumbers damageNumber { get { return Instance._damageNumber; } }
    public static ContextClue contextClue { get { return Instance._contextClue; } }
    public static GameObject markAttack { get { return Instance._markAttack; } }
    public static GameObject markTarget { get { return Instance._markTargeting; } }
    public static MiniDialogBox miniDialogBox { get { return Instance._miniDialogBox; } }
    public static CastBar castBar { get { return Instance._castbar; } }
    public static AnalyseInfo analyseInfo { get { return Instance._analyseInfo; } }
    public static GameSettings settings { get { return Instance._gameSettings; } }
    public static DebugSettings debugSettings { get { return Instance._debugSettings; } }
    public static GlobalValues globalValues { get { return Instance._globalValues; } }
    public static List<ItemDrop> itemDrops { get { return Instance._itemDrops; } }
    public static List<ItemGroup> itemGroups { get { return Instance._itemGroups; } }
    public static List<Ability> abilities { get { return Instance._abilities; } }
    public static List<TeleportStats> teleportpoints { get { return Instance._teleportPoints; } }
    public static TargetingSystem targetingSystem { get { return Instance._targetingSystem; } }
    public static TimeValue timeValue { get { return Instance._timeValue; } }
    public static StringValue actionButtonText { get { return Instance._actionButtonText; } }
    public static GameObject itemCollectGlitter { get { return Instance._itemCollectGlitter; } }
    public static GameObject itemDisappearSmoke { get { return Instance._itemDisappearSmoke; } }


    [BoxGroup("Interaction")]
    [SerializeField]
    private ContextClue _contextClue;
    [BoxGroup("Interaction")]
    [SerializeField]
    private AnalyseInfo _analyseInfo;

    [BoxGroup("Combat")]
    [SerializeField]
    private DamageNumbers _damageNumber;
    [BoxGroup("Combat")]
    [SerializeField]
    private CastBar _castbar;
    [BoxGroup("Combat")]
    [SerializeField]
    private TargetingSystem _targetingSystem;

    [BoxGroup("Values")]
    [SerializeField]
    private TimeValue _timeValue;
    [BoxGroup("Values")]
    [SerializeField]
    private StringValue _actionButtonText;

    [BoxGroup("Bubbles")]
    [SerializeField]
    private MiniDialogBox _miniDialogBox;
    [BoxGroup("Bubbles")]
    [SerializeField]
    private GameObject _markAttack;
    [BoxGroup("Bubbles")]
    [SerializeField]
    private GameObject _markTargeting;

    [BoxGroup("Item")]
    [SerializeField]
    private GameObject _itemCollectGlitter;
    [BoxGroup("Item")]
    [SerializeField]
    private GameObject _itemDisappearSmoke;

    [BoxGroup("Settings")]
    [SerializeField]
    private GameSettings _gameSettings;
    [BoxGroup("Settings")]
    [SerializeField]
    private GlobalValues _globalValues;
    [BoxGroup("Settings")]
    [SerializeField]
    private DebugSettings _debugSettings;

    [BoxGroup("Loading")]
    [SerializeField]
    private List<ItemDrop> _itemDrops = new List<ItemDrop>();
    [BoxGroup("Loading")]
    [SerializeField]
    private List<ItemGroup> _itemGroups = new List<ItemGroup>();
    [BoxGroup("Loading")]
    [SerializeField]
    private List<Ability> _abilities = new List<Ability>();
    [BoxGroup("Loading")]
    [SerializeField]
    private List<TeleportStats> _teleportPoints = new List<TeleportStats>();


    [Button]
    public void LoadAll()
    {
        this._itemDrops.Clear();
        this._itemGroups.Clear();
        this._abilities.Clear();
        this._teleportPoints.Clear();

        this._itemDrops.AddRange(Resources.LoadAll<ItemDrop>("Scriptable Objects/Items/Item Drops/Key Items/"));
        this._itemGroups.AddRange(Resources.LoadAll<ItemGroup>("Scriptable Objects/Items/Item Groups/Inventory Items/"));
        this._itemGroups.AddRange(Resources.LoadAll<ItemGroup>("Scriptable Objects/Items/Item Groups/Currencies/"));
        this._abilities.AddRange(Resources.LoadAll<Ability>("Scriptable Objects/Abilities/Skills/Player Skills/"));
        this._teleportPoints.AddRange(Resources.LoadAll<TeleportStats>("Scriptable Objects/TeleportPoints/"));
    }

    public static ItemDrop getItemDrop(string name)
    {
        foreach(ItemDrop drop in itemDrops)
        {
            if (drop.name == name) return drop;
        }

        return null;
    }

    public static ItemGroup getItemGroup(string name)
    {
        foreach (ItemGroup group in itemGroups)
        {
            if (group.name == name) return group;
        }

        return null;
    }

    public static TeleportStats GetTeleportStats(string teleportName)
    {
        foreach (TeleportStats teleport in teleportpoints)
        {
            if (teleport.teleportName == teleportName) return teleport;
        }

        return null;
    }
}
