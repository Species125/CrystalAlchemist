

using UnityEngine;

namespace CrystalAlchemist
{
    public class MiniGameRound : MonoBehaviour
    {
        private int difficulty;
        private float elapsed;
        private bool timerStopped = true;

        [SerializeField]
        private FloatValue time;

        [SerializeField]
        private FloatValue maxTime;

        public virtual void Start()
        {

        }

        public void Initialize(int difficulty, float time)
        {
            this.difficulty = difficulty;
            this.elapsed = time;
            this.time.SetValue(elapsed);
            this.maxTime.SetValue(time);
        }

        public virtual string GetDescription(string text, int difficulty)
        {
            return "";
        }

        public void StartTimer()
        {
            this.timerStopped = false;
        }

        public void StopTimer()
        {
            this.timerStopped = true;
        }

        private void FixedUpdate()
        {
            if (timerStopped) return;
            if (elapsed <= 0) EndRound(false);
            else this.elapsed -= Time.fixedDeltaTime;

            this.time.SetValue(this.elapsed);
        }

        public void EndRound(bool success)
        {
            StopTimer();
            MiniGameEvents.current.EndMiniGameRound(success);
        }

        public virtual void Check()
        {

        }

        public float GetElapsed()
        {
            return this.elapsed;
        }

        public int GetDifficulty()
        {
            return this.difficulty;
        }
    }
}
