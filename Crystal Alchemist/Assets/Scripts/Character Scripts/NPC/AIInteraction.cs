using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace CrystalAlchemist
{
    public class AIInteraction : Interactable
    {
        [BoxGroup("NPC")]
        [SerializeField]
        [Required]
        private AI npc;

        [BoxGroup("NPC")]
        [SerializeField]
        private UnityEvent onSubmit;

        private void Awake() => this.SetSmoke(false);

        public override void Start()
        {
            base.Start();

            //if (!NetworkUtil.IsMaster()) return;
            this.context.transform.position = this.npc.GetHeadPosition();
        }

        public override void DoOnSubmit()
        {
            //if (!NetworkUtil.IsMaster()) return;
            this.onSubmit?.Invoke();
        }

        public void TurnHostile()
        {
            this.npc.values.characterType = CharacterType.Enemy;
        }

        public void TurnFriendly()
        {
            this.npc.values.characterType = CharacterType.Friend;
        }

        public void ClearAggro() => this.npc.ClearAggro();

        public override bool PlayerIsLooking()
        {
            //if (!NetworkUtil.IsMaster()) return false;

            if (this.isPlayerInRange
                && this.npc.values.characterType == CharacterType.Friend
                && CollisionUtil.checkIfGameObjectIsViewed(this.player, this.npc.gameObject)) return true;
            return false;
        }
    }
}
