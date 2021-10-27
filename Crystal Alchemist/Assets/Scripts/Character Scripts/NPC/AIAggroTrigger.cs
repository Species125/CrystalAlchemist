using Sirenix.OdinInspector;
using UnityEngine;

namespace CrystalAlchemist
{
    public class AIAggroTrigger : MonoBehaviour
    {
        #region attributes

        [SerializeField]
        [BoxGroup("Required")]
        [InfoBox("A trigger collider is required")]
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
