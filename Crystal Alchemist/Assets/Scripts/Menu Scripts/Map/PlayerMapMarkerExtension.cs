using UnityEngine;

namespace CrystalAlchemist
{
    [RequireComponent(typeof(Player))]
    public class PlayerMapMarkerExtension : MiniMapMarkerExtension
    {
        private Player player;

        [SerializeField]
        private GameObject guestPlayerMarker;

        public override void Start()
        {
            this.player = this.GetComponent<Player>();

            if (player.IsGuestPlayer()) Instantiate(this.guestPlayerMarker, this.transform);
            else Instantiate(this.marker, this.transform);
        }
    }
}