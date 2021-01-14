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
        private TeleportStats nextTeleport;

        [BoxGroup("Values")]
        [SerializeField]
        private TeleportStats lastTeleport;

        [BoxGroup("Debug")]
        [SerializeField]
        private bool ShowSpawnIn;

        [BoxGroup("Debug")]
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
            this.nextTeleport = stats;
            this.ShowSpawnIn = this.nextTeleport.showAnimationIn;
            this.ShowSpawnOut = this.nextTeleport.showAnimationOut;
        }

        public void SetAnimation(bool showIn, bool showOut)
        {
            this.ShowSpawnIn = showIn;
            this.ShowSpawnOut = showOut;
        }

        public void SetLastTeleport(TeleportStats stats)
        {
            this.lastTeleport = stats;
        }

        public TeleportStats GetNextTeleport()
        {
            return this.nextTeleport;
        }

        public TeleportStats GetLastTeleport()
        {
            return this.lastTeleport;
        }

        public bool HasLast()
        {
            return this.lastTeleport != null;
        }

        public bool HasNext()
        {
            return this.nextTeleport != null;
        }

        public void SetReturnTeleport()
        {
            if (this.lastTeleport == null) return;
            SetNextTeleport(this.lastTeleport);
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
