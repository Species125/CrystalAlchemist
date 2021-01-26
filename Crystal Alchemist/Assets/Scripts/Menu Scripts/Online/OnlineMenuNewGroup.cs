using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

namespace CrystalAlchemist
{
    public class OnlineMenuNewGroup : MonoBehaviourPunCallbacks
    {
        [BoxGroup("UI")]
        [SerializeField]
        private Toggle toggle;

        [BoxGroup("UI Room")]
        [SerializeField]
        private TextMeshProUGUI errormessage;

        [BoxGroup("UI Room")]
        [SerializeField]
        private StringValue roomName;

        [BoxGroup("UI Room")]
        [SerializeField]
        private TextMeshProUGUI nameField;

        [BoxGroup("UI Password")]
        [SerializeField]
        private GameObject passwordBox;

        [BoxGroup("UI Password")]
        [SerializeField]
        private StringValue password;

        [BoxGroup("UI Password")]
        [SerializeField]
        private TextMeshProUGUI passwordField;

        [BoxGroup("UI Password")]
        [SerializeField]
        private Selectable confirmButton;

        public void Reset()
        {
            this.roomName.SetValue("");
            this.password.SetValue("");
            this.toggle.isOn = false;
            this.passwordBox.SetActive(false);
        }

        public override void OnEnable()
        {
            base.OnEnable();
            this.errormessage.gameObject.SetActive(false);
            OnToggleChanged();
            DisableButton();
        }

        private void UpdateTexts()
        {
            this.nameField.text = this.roomName.GetValue();
            this.passwordField.text = this.password.GetValue();
        }

        public void OnToggleChanged()
        {
            this.passwordBox.SetActive(this.toggle.isOn);
            if (!this.toggle.isOn) this.password.SetValue("");
            UpdateTexts();
            DisableButton();
        }

        private void DisableButton()
        {
            if (this.toggle.isOn && this.password.GetValue().Replace(" ", "").Length == 0) this.confirmButton.interactable = false;
            else this.confirmButton.interactable = true;
        }

        public void Confirm()
        {
            NetworkUtil.CreateRoom(this.roomName.GetValue(), 4, true, this.toggle.isOn, this.password.GetValue());
        }

        public override void OnCreatedRoom()
        {
            
        }

        public override void OnCreateRoomFailed(short returnCode, string message)
        {
            Debug.Log(returnCode + ": " + message);

            this.errormessage.gameObject.SetActive(true);
            this.roomName.SetValue("");
            this.nameField.text = this.roomName.GetValue();
        }
    }    
}
