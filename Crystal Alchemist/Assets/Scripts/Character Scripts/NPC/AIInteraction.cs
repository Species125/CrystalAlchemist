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
            this.context.transform.position = this.npc.GetHeadPosition();
        }

        public override void DoOnSubmit()
        {
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

        public void ClearAggro() => GameEvents.current.DoAggroClear(this.npc);

        public override bool PlayerIsLooking()
        {
            if (this.isPlayerInRange
                && this.npc.values.characterType == CharacterType.Friend
                && CollisionUtil.checkIfGameObjectIsViewed(this.player, this.npc.gameObject)) return true;
            return false;
        }
    }
}
