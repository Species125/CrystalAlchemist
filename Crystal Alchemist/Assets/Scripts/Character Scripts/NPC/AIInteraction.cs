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
        private Character npc;

        [BoxGroup("NPC")]
        [SerializeField]
        private UnityEvent onSubmit;

        [BoxGroup("NPC")]
        [SerializeField]
        private UnityEvent onMenuClosed;
        
        private bool interacted = false;

        public override void Start()
        {
            base.Start();

            //if (!NetworkUtil.IsMaster()) return;
            this.context.transform.position = this.npc.GetHeadPosition();
            GameEvents.current.OnMenuClosed += DoOnMenuClose;
        }

        public override void OnEnable()
        {
            base.OnEnable();
        }

        public override void OnDisable()
        {
            base.BaseOnDisable();
            ShowContextClue(false);
        }

        private void OnDestroy()
        {
            GameEvents.current.OnMenuClosed -= DoOnMenuClose;
        }

        public void DoOnMenuClose()
        {
            if (this.interacted)
            {
                this.onMenuClosed?.Invoke();
                this.interacted = false;
            }
        }

        public override void DoOnSubmit()
        {
            //if (!NetworkUtil.IsMaster()) return;
            this.onSubmit?.Invoke();
            this.interacted = true;
        }

        public void TurnHostile()
        {
            this.npc.SetCharacterType(CharacterType.Enemy);
        }

        public void TurnFriendly()
        {
            this.npc.SetCharacterType(CharacterType.Friend);
        }

        public override bool PlayerIsLooking()
        {
            //if (!NetworkUtil.IsMaster()) return false;

            if (this.isPlayerInRange
                && this.npc.GetCharacterType() == CharacterType.Friend
                && CollisionUtil.checkIfGameObjectIsViewed(this.player, this.npc.gameObject)) return true;
            return false;
        }
    }
}
