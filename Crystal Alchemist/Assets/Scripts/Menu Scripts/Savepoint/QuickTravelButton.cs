


using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CrystalAlchemist
{
    public class QuickTravelButton : MonoBehaviour
    {
        [SerializeField]
        [Required]
        private PlayerTeleportList playerTeleport;

        [SerializeField]
        [Required]
        private TextMeshProUGUI textField;

        [SerializeField]
        [Required]
        private Image image;

        private TeleportStats location;

        public void SetLocation(TeleportStats stat)
        {
            this.location = stat;
            this.textField.text = this.location.GetTeleportName();
            this.image.sprite = this.location.icon;
        }

        public void Teleport()
        {
            this.playerTeleport.SetNextTeleport(this.location);
            GameEvents.current.DoTeleport();
        }
    }
}
