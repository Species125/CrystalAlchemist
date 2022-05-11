using UnityEngine;


namespace CrystalAlchemist
{
    [RequireComponent(typeof(UnityEngine.Rendering.Universal.Light2D))]
    public class DayNightCircle : MonoBehaviour
    {
        private UnityEngine.Rendering.Universal.Light2D Lighting;
        private bool isRunning = false;
        private TimeValue timeValue;

        private void Awake()
        {
            this.Lighting = this.GetComponent<UnityEngine.Rendering.Universal.Light2D>();
            this.timeValue = MasterManager.timeValue;
        }

        private void Start()
        {
            GameEvents.current.OnNightChange += changeColor;
            changeColor();
        }

        private void OnDestroy() => GameEvents.current.OnNightChange -= changeColor;

        public void changeColor() => Lighting.color = this.timeValue.GetColor();
    }
}
