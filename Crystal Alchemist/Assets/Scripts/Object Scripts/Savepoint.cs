





using Sirenix.OdinInspector;
using UnityEngine;

namespace CrystalAlchemist
{
    public class Savepoint : Interactable
    {
        [BoxGroup("SavePoint")]
        [Tooltip("Teleport Info of this savepoint")]
        [SerializeField]
        private TeleportStats teleportPoint;

        [BoxGroup("Player")]
        [Tooltip("To add this Teleport Point to quicktravel")]
        [SerializeField]
        private PlayerTeleportList teleportList;

        [BoxGroup("UI")]
        [Tooltip("To store info for UI (Respawn)")]
        [SerializeField]
        private SavePointInfo savePointInfo;

        public override void DoOnSubmit()
        {
            this.player.UpdateResource(CostType.life, this.player.values.maxLife);
            this.player.UpdateResource(CostType.mana, this.player.values.maxMana);

            this.teleportList.AddTeleport(this.teleportPoint); //add to teleport list    
            this.savePointInfo.stats = this.teleportPoint;

            MenuEvents.current.OpenSavepoint();
        }
    }
}
