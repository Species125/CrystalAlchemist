using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

namespace CrystalAlchemist
{
    [RequireComponent(typeof(Light2D))]
    public class DayNightCircle : MonoBehaviour
    {
        private Light2D Lighting;
        private bool isRunning = false;
        private TimeValue timeValue;

        private void Awake()
        {
            this.Lighting = this.GetComponent<Light2D>();
            this.timeValue = MasterManager.timeValue;
        }

        private void Start()
        {
            GameEvents.current.OnTimeChanged += changeColor;
            changeColor();
        }

        private void OnDestroy() => GameEvents.current.OnTimeChanged -= changeColor;

        public void changeColor() => Lighting.color = this.timeValue.GetColor();
    }
}
