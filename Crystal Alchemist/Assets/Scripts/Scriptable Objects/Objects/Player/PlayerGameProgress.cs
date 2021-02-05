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

        public bool IsSame(ProgressDetails details)
        {
            if (details.key == this.gameProgressID
                && details.location == this.location
                && details.type == this.progressType) return true;

            return false;
        }        

        public string GetLocation()
        {
            return this.location;
        }

        public ProgressType GetProgressType()
        {
            return this.progressType;
        }

        public string GetKey()
        {
            return this.gameProgressID;
        }

        public UTimeSpan GetSpan()
        {
            return this.timespan;
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

        public void Updating()
        {
            for (int i = 0; i < this.progressList.Count; i++)
            {
                ProgressDetails progress = this.progressList[i];

                if (progress.type != ProgressType.permanent
                    && DateTime.Now > progress.date.ToDateTime() + progress.timespan.ToTimeSpan())
                {
                    this.progressList.RemoveAt(i);
                }
            }
        }

        public void Clear()
        {
            progressList.Clear();
        }

        public void AddProgress(ProgressValue value)
        {
            AddProgress(value.GetLocation(), value.GetKey(), value.GetProgressType(), new UDateTime(DateTime.Now), value.GetSpan());
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

        public bool Contains(ProgressValue value)
        {
            return Contains(value.GetLocation(), value.GetKey(), value.GetProgressType());
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