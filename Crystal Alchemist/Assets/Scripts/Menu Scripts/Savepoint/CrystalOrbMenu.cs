using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace CrystalAlchemist
{
    public class CrystalOrbMenu : MenuBehaviour
    {
        [BoxGroup("Crystal Orb Menu")]
        [Required]
        [SerializeField]
        private PlayerTeleportList teleportList;

        [BoxGroup("Crystal Orb Menu")]
        [Required]
        [SerializeField]
        private SavePointInfo savePointInfo;

        [BoxGroup("Crystal Orb Menu")]
        [Required]
        [SerializeField]
        private Image star;


        public override void Start()
        {
            base.Start();
            SetStar();
        }

        public void SetStar()
        {
            if (teleportList.HasLast()
                && teleportList.GetLastTeleport().Exists(savePointInfo.stats.teleportName)) star.gameObject.SetActive(true);
            else star.gameObject.SetActive(false);
        }

        public void SetLastTeleport()
        {
            this.teleportList.SetLastTeleport(this.savePointInfo.stats);
        }
    }
}
