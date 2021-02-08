using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CrystalAlchemist
{
    public class OnlineMenuPlayerInfo : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI nameField;

        [SerializeField]
        private Image leadImage;

        [SerializeField]
        private Selectable kickButton;

        [SerializeField]
        private ButtonExtension SetNext;

        [BoxGroup("Debug")]
        [ReadOnly]
        public int ID;

        public void SetInfos(int ID, bool enableButtons)
        {
            Player player = NetworkUtil.GetPlayer(ID);
            this.nameField.text = player.GetCharacterName();
            this.ID = ID;

            if (player.isMaster)
            {
                this.leadImage.gameObject.SetActive(true);
                this.kickButton.gameObject.SetActive(false);
            }
            else
            {
                this.leadImage.gameObject.SetActive(false);
                this.kickButton.gameObject.SetActive(enableButtons);
            }            
        }

        public void KickPlayer()
        {
            NetworkEvents.current.DisconnectPlayer(this.ID);
            this.SetNext.Select();
        }
    }
}
