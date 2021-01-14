


using UnityEngine;

namespace CrystalAlchemist
{
    public class DayNightInterior : MonoBehaviour
    {
        [SerializeField]
        private GameObject lightGameObject;

        private TimeValue timeValue;

        private void Awake() => this.timeValue = MasterManager.timeValue;

        private void Start()
        {
            GameEvents.current.OnTimeChanged += switchInteriorLights;
            switchInteriorLights();
        }

        private void OnDestroy() => GameEvents.current.OnTimeChanged -= switchInteriorLights;

        public void switchInteriorLights()
        {
            if (this.timeValue.night) lightGameObject.SetActive(true);
            else lightGameObject.SetActive(false);
        }
    }
}
