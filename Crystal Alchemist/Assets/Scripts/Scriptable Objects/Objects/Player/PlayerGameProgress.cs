using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CrystalAlchemist
{
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
        private string gameProgressID;

        [BoxGroup("Progress")]
        [ReadOnly]
        [SerializeField]
        private string location;

        [BoxGroup("Progress")]
        [HideIf("progressType", ProgressType.none)]
        [Required]
        [SerializeField]
        [OnValueChanged("SetLocation")]
        private bool addLocation = false;

        [BoxGroup("Progress")]
        [ShowIf("progressType", ProgressType.expire)]
        [Required]
        [SerializeField]
        [HideLabel]
        private UTimeSpan timespan;

        private void SetLocation()
        {
            this.location = SceneManager.GetActiveScene().name;
            if (!this.addLocation) this.location = "";
        }

        public bool ContainsProgress(PlayerGameProgress playerProgress)
        {
            if (this.progressType == ProgressType.none || playerProgress == null) return false;

            SetLocation();

            return playerProgress.Contains(this.location, this.gameProgressID, this.progressType);
        }

        public void AddProgress(PlayerGameProgress playerProgress)
        {
            if (this.progressType == ProgressType.none || playerProgress == null) return;

            SetLocation();

            playerProgress.AddProgress(this.location, this.gameProgressID, this.progressType, this.timespan);
        }

        public bool IsSame(ProgressDetails details)
        {
            if (details.key == this.gameProgressID
                && details.location == this.location
                && details.type == this.progressType) return true;

            return false;
        }
    }

    [System.Serializable]
    public class ProgressDetails
    {
        [BoxGroup("Group")]
        [ReadOnly]
        public ProgressType type;

        [SerializeField]
        [ReadOnly]
        [BoxGroup("Group")]
        private string ID;

        [HideInInspector]
        public string key;
        [HideInInspector]
        public string location;

        [HideInInspector]
        public UDateTime date;

        [HideInInspector]
        public UTimeSpan timespan;

        [SerializeField]
        [ReadOnly]
        [BoxGroup("Group")]
        private string progessDate;

        [SerializeField]
        [ReadOnly]
        [BoxGroup("Group")]
        [ShowIf("type", ProgressType.expire)]
        private string progressDuration;
        
        public void UpdateInspector()
        {
            this.ID = this.location + " -> " + this.key;
            this.progessDate = date.ToString();
            this.progressDuration = timespan.ToInspector();
        }
    }

    [CreateAssetMenu(menuName = "Game/Player/Game Progress")]
    public class PlayerGameProgress : ScriptableObject
    {        
        [SerializeField]
        private List<ProgressDetails> progressList = new List<ProgressDetails>();

        public bool Updating(ProgressValue progressValue = null)
        {
            bool result = false;

            for (int i = 0; i < this.progressList.Count; i++)
            {
                ProgressDetails progress = this.progressList[i];

                if (progress.type != ProgressType.permanent
                    && DateTime.Now > progress.date.ToDateTime() + progress.timespan.ToTimeSpan())
                {
                    if (progressValue != null && progressValue.IsSame(progress)) result = true;
                    this.progressList.RemoveAt(i);
                }
            }

            return result;
        }

        public void Clear()
        {
            progressList.Clear();
        }

        public void AddProgress(string location, string key, ProgressType type, UTimeSpan span)
        {
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
            progress.type = type;
            progress.UpdateInspector();

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
}