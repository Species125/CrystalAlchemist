using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Sirenix.OdinInspector;

public enum ProgressType
{
    permanent,
    expire,
    none
}

[System.Serializable]
public class ProgressValue
{
    [BoxGroup("Progress")]
    [SerializeField]
    private ProgressType progressType = ProgressType.none;

    [BoxGroup("Progress")]
    [HideIf("progressType", ProgressType.none)]
    [Required]
    [SerializeField]
    private PlayerGameProgress playerProgress;

    [BoxGroup("Progress")]
    [HideIf("progressType", ProgressType.none)]
    [Required]
    [SerializeField]
    private string gameProgressID;

    [BoxGroup("Progress")]
    [HideIf("progressType", ProgressType.none)]
    [Required]
    [SerializeField]
    private bool onlyThisArea = false;

    [BoxGroup("Progress")]
    [ShowIf("progressType", ProgressType.expire)]
    [Required]
    [SerializeField]
    [HideLabel]
    private UTimeSpan timespan;

    public bool ContainsProgress()
    {
        if (this.progressType == ProgressType.none) return false;

        string location = SceneManager.GetActiveScene().name;
        if (!this.onlyThisArea) location = "";

        return this.playerProgress.Contains(location, this.gameProgressID, this.progressType);
    }

    public void AddProgress()
    {
        if (this.progressType == ProgressType.none) return;
        this.playerProgress.AddProgress(this.gameProgressID, this.progressType, this.timespan, this.onlyThisArea);
    }
}

[System.Serializable]
public struct ProgressDetails
{
    [HorizontalGroup("Group", LabelWidth = 40, MaxWidth = 125, MarginRight = 10)]
    public ProgressType type;
    [HorizontalGroup("Group", LabelWidth = 5, MaxWidth = 150, MarginRight = 10)]
    public string key;
    [HorizontalGroup("Group", LabelWidth = 50, MaxWidth = 150)]
    public string location;


    public UDateTime date;
    public UTimeSpan timespan;
}

[CreateAssetMenu(menuName = "Game/Player/Game Progress")]
public class PlayerGameProgress : ScriptableObject
{    
    [SerializeField]
    private List<ProgressDetails> progressList = new List<ProgressDetails>();

    public void Initialize()
    {
        for (int i = 0; i < this.progressList.Count; i++)
        {
            ProgressDetails progress = this.progressList[i];
            if (progress.type != ProgressType.permanent
                && DateTime.Now > progress.date.ToDateTime() + progress.timespan.ToTimeSpan())
                this.progressList.RemoveAt(i);
        }
    }

    public void Clear()
    {
        progressList.Clear();
    }

    public void AddProgress(string key, ProgressType type, UTimeSpan span, bool addLocation)
    {
        string location = SceneManager.GetActiveScene().name;
        if (!addLocation) location = "";

        AddProgress(location, key, type, new UDateTime(DateTime.Now), span);
    }

    public void AddProgress(string location, string key, ProgressType type, UDateTime date, UTimeSpan span)
    {
        if (type == ProgressType.none) return;
        ProgressDetails progress = new ProgressDetails();

        progress.location = location;
        progress.key = key;
        progress.date = date;
        progress.timespan = span;

        if (!Contains(location, key, type)) this.progressList.Add(progress);
    }

    public bool Contains(string location, string key, ProgressType type)
    {
        for (int i = 0; i < this.progressList.Count; i++)
        {
            ProgressDetails progress = this.progressList[i];
            if (progress.key == key 
                && progress.location == location 
                && progress.type == type) return true;            
        }

        return false;
    }

    public int GetAmount()
    {
        return this.progressList.Count;
    }

    public List<string[]> GetProgressRaw()
    {
        List<string[]> result = new List<string[]>();

        foreach (ProgressDetails prog in this.progressList)
        {
            string[] temp = new string[5];
            temp[0] = prog.location;
            temp[1] = prog.key;
            temp[2] = prog.date.ToString();
            temp[3] = prog.timespan.ToString();
            temp[4] = prog.type.ToString();
            result.Add(temp);
        }

        return result;
    }
}
