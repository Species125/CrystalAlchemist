using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

namespace CrystalAlchemist
{
    public class AIAggroSystem : MonoBehaviour
    {
        #region attributes

        [SerializeField]
        [BoxGroup("Required")]
        [Required]
        private AI npc;

        #endregion

        #region Start und Update

        private void LateUpdate()
        {
            if (this.GetComponent<CircleCollider2D>() == null) RotationUtil.rotateCollider(this.npc, this.gameObject);                    
        }

        #endregion

        #region Aggro-Collider   

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (this.npc == null || this.npc.aggroStats == null) return;
            if (this.npc.aggroStats.affections.IsAffected(this.npc, collision)) this.npc._IncreaseAggro(collision.GetComponent<Character>());            
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (this.npc == null || this.npc.aggroStats == null) return;
            this.npc._DecreaseAggro(collision.GetComponent<Character>());
        }        

        #endregion
    }
}
