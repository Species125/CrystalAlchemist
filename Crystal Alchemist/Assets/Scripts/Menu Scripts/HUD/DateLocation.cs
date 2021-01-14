using TMPro;
using UnityEngine;

namespace CrystalAlchemist
{
    public class DateLocation : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI textField;

        [SerializeField]
        private TextMeshProUGUI timeField;

        [SerializeField]
        private TimeValue timeValue;

        [SerializeField]
        private GameObject sun;

        [SerializeField]
        private GameObject moon;

        [SerializeField]
        private StringValue locationID;

        private void Start()
        {
            SettingsEvents.current.OnLanguangeChanged += updateLocationText;
            updateLocationText();
        }

        private void Update() => UpdateTime();

        private void OnDestroy() => SettingsEvents.current.OnLanguangeChanged -= updateLocationText;


        private void UpdateTime()
        {
            int hour = this.timeValue.getHour();

            this.timeField.text = hour.ToString("00") + ":" + this.timeValue.getMinute().ToString("00");

            if (!this.timeValue.night && !sun.activeInHierarchy)
            {
                sun.SetActive(true);
                moon.SetActive(false);
            }
            else if (this.timeValue.night && !moon.activeInHierarchy)
            {
                sun.SetActive(false);
                moon.SetActive(true);
            }
        }

        private void updateLocationText()
        {
            this.textField.text = FormatUtil.GetLocalisedText(this.locationID.GetValue(), LocalisationFileType.maps);
        }
    }
}
