using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine.UI;
using System;

namespace CrystalAlchemist
{
    public class OnlineMenuJoinGroup : MonoBehaviourPunCallbacks
    {
        [BoxGroup("UI Room")]
        [SerializeField]
        private TextMeshProUGUI nameField;

        [BoxGroup("UI Password")]
        [SerializeField]
        private GameObject passwordBox;

        [BoxGroup("UI Password")]
        [SerializeField]
        private Toggle toggle;

        [BoxGroup("UI Password")]
        [SerializeField]
        private StringValue passwordValue;

        [BoxGroup("UI Password")]
        [SerializeField]
        private TextMeshProUGUI passwordField;

        [BoxGroup("UI Password")]
        [SerializeField]
        private TextMeshProUGUI playerCountField;

        [BoxGroup("UI Password")]
        [SerializeField]
        private Selectable confirmButton;

        [BoxGroup("UI Password")]
        [SerializeField]
        private TextMeshProUGUI errormessage;

        [BoxGroup("Buttons")]
        [SerializeField]
        private GameObject menu;

        [BoxGroup("Buttons")]
        [SerializeField]
        private CustomCursor cursor;

        public RoomInfo info;
        private string roomName;
        private bool isPrivate;
        private string password;

        public void SetInfo(OnlineMenuGroupButton button)
        {
            this.info = button.info;
            UpdateTexts();
            DisableButton();
        }

        public void Reset()
        {
            this.passwordValue.SetValue("");
        }

        public override void OnEnable()
        {
            base.OnEnable();
            this.errormessage.gameObject.SetActive(false);
            Invoke("SetInfosDelayed", 0.1f);
        }

        private void SetInfosDelayed()
        {
            UpdateTexts();
            DisableButton();
        }

        private void UpdateTexts()
        {
            this.roomName = this.info.Name;
            this.nameField.text = this.roomName;
            this.passwordField.text = this.passwordValue.GetValue();
            this.playerCountField.text = this.info.PlayerCount + "/" + this.info.MaxPlayers;

            this.isPrivate = Convert.ToBoolean(this.info.CustomProperties["Private"]);
            this.password = this.info.CustomProperties["Password"].ToString();

            this.passwordBox.SetActive(this.isPrivate);
            this.toggle.isOn = this.isPrivate;
        }

        private void DisableButton()
        {            
            if (this.toggle.isOn && this.passwordValue.GetValue().Replace(" ", "").Length == 0) this.confirmButton.interactable = false;
            else this.confirmButton.interactable = true;
        }

        public void Confirm()
        {
            if (isPrivate && password != this.passwordValue.GetValue())
            {
                this.errormessage.gameObject.SetActive(true);
                this.passwordValue.SetValue("");
                this.passwordField.text = this.passwordValue.GetValue();
                DisableButton();
                return;
            }

            PhotonNetwork.JoinRoom(this.roomName);

            this.menu.SetActive(false);
            this.cursor.gameObject.SetActive(false);
        }

        public override void OnJoinRoomFailed(short returnCode, string message)
        {
            
        }
    }
}