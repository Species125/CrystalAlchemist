using Cinemachine;
using UnityEngine;

namespace CrystalAlchemist
{
    public class CameraNetworkTargetGroup : CinemachineTargetGroup
    {
        private Player player;

        private void Start() => AddLocalPlayer();

        /// <summary>
        /// Add Local Player to Target Group, so the camera can follow the player
        /// </summary>
        private void AddLocalPlayer()
        {
            this.player = NetworkUtil.GetLocalPlayer();
            if (this.player == null) return;

            AddCamera(this.player.transform);
        }

        /// <summary>
        /// Add Target to TargetGroup if not exists. If exists, set its weight to 1
        /// </summary>
        public void AddCamera(Transform transform)
        {
            bool found = false;

            for (int i = 0; i < this.m_Targets.Length && !found; i++)
            {
                if (this.m_Targets[i].target == transform)
                {
                    this.m_Targets[i].weight = 1f;
                    found = true;
                }
            }

            if (!found) this.AddMember(transform, 1f, 0f);            
        }

        /// <summary>
        /// Reset Camera back to normal
        /// </summary>
        public void ResetCamera()
        {         
            if (this.player == null) return;

            for (int i = 0; i < this.m_Targets.Length; i++)
            {
                if (this.m_Targets[i].target != this.player.transform) this.m_Targets[i].weight = 0f;
            }
        }
    }
}
