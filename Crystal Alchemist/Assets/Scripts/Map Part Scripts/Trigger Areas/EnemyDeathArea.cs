using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace CrystalAlchemist
{
    public class EnemyDeathArea : NetworkBehaviour
    {
        [SerializeField]
        private List<GameObject> enemies = new List<GameObject>();

        [SerializeField]
        private UnityEvent OnDeathAll;

        public void CheckEnemies()
        {
            this.enemies.RemoveAll(x => x == null);

            foreach(GameObject enemy in this.enemies)
            {
                if (enemy.activeInHierarchy) return;
            }

            this.photonView.RPC("RpcOnAllEnemiesDead", RpcTarget.All);
        }

        [PunRPC]
        protected void RpcOnAllEnemiesDead()
        {
            OnDeathAll?.Invoke();
        }
    }
}