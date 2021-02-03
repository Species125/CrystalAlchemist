
using Sirenix.OdinInspector;
using UnityEngine;

namespace CrystalAlchemist
{
    public enum season
    {
        none,
        winter,
        autumn,
        christmas,
        halloween
    }


    [CreateAssetMenu(menuName = "Game/Time/Time Value")]
    public class TimeValue : ScriptableObject
    {
        private float seconds = 0;

        [BoxGroup("private fields")]
        [SerializeField]
        private int minute = 0;

        [BoxGroup("private fields")]
        [SerializeField]
        private int hour = 13;

        [BoxGroup("private fields")]
        [SerializeField]
        private float normalFactor = 1;

        [BoxGroup("private fields")]
        [SerializeField]
        private int update = 30;

        [BoxGroup("private fields")]
        [SerializeField]
        private float factor = 1;

        [BoxGroup("private fields")]
        [SerializeField]
        private Gradient colorGradient;

        [BoxGroup("public fields")]
        public bool night;

        [BoxGroup("public fields")]
        public season season;

        public void Clear() => SetStartTime();

        [Button]
        public void Reset() => this.factor = this.normalFactor;

        public void SetFactor(float value) => this.factor = value;

        private void SetStartTime()
        {
            this.hour = 13;
            this.minute = 0;
            //TODO: Fixed time or Random or Saved?
            /*
        DateTime origin = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0, 0);
        TimeSpan diff = DateTime.Now - origin;
        double difference = Math.Floor(diff.TotalSeconds);
        float minutes = (float)(difference / (double)factor); //elapsed ingame minutes
        float fhour = ((minutes / 60f) % 24f)-1;
        float fminute = (minutes % 60f);

        if (fhour >= 24) fhour = 0;
        if (fminute >= 60) fminute = 0;

        this.hour = Mathf.RoundToInt(fhour);
        this.minute = Mathf.RoundToInt(fminute);*/
        }

        public Color GetColor()
        {
            float temp = (float)(100f / 24f / 60f);
            float value = (float)((this.hour * 60) + this.minute);
            float percentage = (temp * value) / 100;
            return this.colorGradient.Evaluate(percentage);
        }

        public int GetMinute()
        {
            return this.minute;
        }

        public int GetHour()
        {
            return this.hour;
        }

        public void SetTime(int hour, int minute)
        {
            this.minute = minute;
            this.hour = hour;
            this.seconds = 0;
            updateTimer();
        }

        public void SetTime(float seconds)
        {
            this.seconds += seconds;
            updateTimer();
        }

        private void updateTimer()
        {
            if (hour < 18 && hour > 6) this.night = false;
            else this.night = true;

            if (this.seconds >= this.factor)
            {
                this.minute += 1;
                this.seconds = 0;

                if (this.minute % this.update == 0) GameEvents.current.DoNightChange();

                if (this.minute >= 60)
                {
                    this.minute = 0;
                    this.hour += 1;

                    if (this.hour >= 24) this.hour = 0;
                }
            }
        }
    }
}