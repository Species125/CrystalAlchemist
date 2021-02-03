using TMPro;
using UnityEngine;
using Sirenix.OdinInspector;
using Photon.Pun;

namespace CrystalAlchemist
{
    public class DateLocation : MonoBehaviour
    {
        [BoxGroup("Location")]
        [SerializeField]
        [Required]
        private TextMeshProUGUI textField;

        [BoxGroup("Time")]
        [SerializeField]
        [Required]
        private TextMeshProUGUI timeField;

        [BoxGroup("Time")]
        [SerializeField]
        [Required]
        private TimeValue timeValue;

        [BoxGroup("Time")]
        [SerializeField]
        [Required]
        private GameObject sun;

        [BoxGroup("Time")]
        [SerializeField]
        [Required]
        private GameObject moon;

        [BoxGroup("Location")]
        [SerializeField]
        [Required]
        private StringValue locationID;

        [BoxGroup("Online")]
        [SerializeField]
        [Required]
        private GameObject online;

        [BoxGroup("Online")]
        [SerializeField]
        [Required]
        private TextMeshProUGUI roomField;

        private void Start()
        {
            SettingsEvents.current.OnLanguangeChanged += updateLocationText;
            UpdateOnlineStatus();
            updateLocationText();
        }

        private void Update() => UpdateTime();

        private void OnDestroy() => SettingsEvents.current.OnLanguangeChanged -= updateLocationText;

        private void UpdateOnlineStatus()
        {
            if (PhotonNetwork.OfflineMode) this.online.SetActive(false);
            else this.online.SetActive(true);

            this.roomField.text = PhotonNetwork.CurrentRoom.Name;
        }

        private void UpdateTime()
        {
            int hour = this.timeValue.GetHour();

            this.timeField.text = hour.ToString("00") + ":" + this.timeValue.GetMinute().ToString("00");

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
