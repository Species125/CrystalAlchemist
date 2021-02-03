using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CrystalAlchemist
{
    [CreateAssetMenu(menuName = "Game/Player/Teleport List")]
    public class PlayerTeleportList : ScriptableObject
    {
        [BoxGroup("Values")]
        [SerializeField]
        private List<TeleportStats> list = new List<TeleportStats>();

        [BoxGroup("Values")]
        [SerializeField]
        private TeleportStats latestTeleport;

        [BoxGroup("Values")]
        [SerializeField]
        private TeleportStats returnTeleport;

        [BoxGroup("Debug")]
        [ReadOnly]
        [SerializeField]
        private bool ShowSpawnIn;

        [BoxGroup("Debug")]
        [ReadOnly]
        [SerializeField]
        private bool ShowSpawnOut;


        public bool GetShowSpawnIn()
        {
            return this.ShowSpawnIn;
        }

        public bool GetShowSpawnOut()
        {
            return this.ShowSpawnIn;
        }

        public void SetNextTeleport(TeleportStats stats)
        {
            this.latestTeleport = stats;
            this.ShowSpawnIn = this.latestTeleport.showAnimationIn;
            this.ShowSpawnOut = this.latestTeleport.showAnimationOut;
        }

        public void SetAnimation(bool showIn, bool showOut)
        {
            this.ShowSpawnIn = showIn;
            this.ShowSpawnOut = showOut;
        }

        public void SetReturnTeleport(TeleportStats stats)
        {
            this.returnTeleport = stats;
        }

        public TeleportStats GetLatestTeleport()
        {
            return this.latestTeleport;
        }

        public TeleportStats GetReturnTeleport()
        {
            return this.returnTeleport;
        }

        public bool HasReturn()
        {
            return this.returnTeleport != null;
        }

        public bool HasLatest()
        {
            return this.latestTeleport != null;
        }

        public void SetReturnTeleport()
        {
            if (this.returnTeleport == null) return;
            SetNextTeleport(this.returnTeleport);
        }

        public void Initialize()
        {
            this.list.RemoveAll(item => item == null);
            this.list.OrderBy(o => o.scene);
        }

        public void AddTeleport(TeleportStats stat)
        {
            if (!Contains(stat)) this.list.Add(stat);
            this.list.OrderBy(o => o.scene);
        }

        public bool Contains(TeleportStats stat)
        {
            this.list.RemoveAll(item => item == null);

            for (int i = 0; i < this.list.Count; i++)
            {
                if (list[i].scene == stat.scene
                    && list[i].Exists(stat.teleportName)) return true;
            }

            return false;
        }

        public List<TeleportStats> GetStats()
        {
            return this.list;
        }

        public TeleportStats GetStats(int index)
        {
            return this.list[index];
        }
    }
}
