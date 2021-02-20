using Photon.Pun;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CrystalAlchemist
{
    public class Door : Interactable
    {
        public enum DoorType
        {
            normal,
            enemy,
            button,
            closed
        }

        [Required]
        [BoxGroup("Mandatory")]
        public Animator animator;

        [BoxGroup("Tür-Attribute")]
        [SerializeField]
        private DoorType doorType = DoorType.closed;

        [BoxGroup("Tür-Attribute")]
        [ShowIf("doorType", DoorType.normal)]
        [SerializeField]
        private bool autoClose = true;

        private bool isOpen;

        public override void OnExit(Collider2D characterCollisionBox)
        {
            Player player = characterCollisionBox.GetComponent<Player>();
            if (!NetworkUtil.IsLocal(player) || characterCollisionBox.isTrigger) return;

            base.OnExit(characterCollisionBox);

            if (this.isOpen && this.doorType == DoorType.normal)
            {
                //Normale Tür fällt von alleine wieder zu
                if (this.autoClose) OpenCloseDoor(false);
            }
        }

        public override void DoOnSubmit()
        {
            if (this.doorType != DoorType.enemy && this.doorType != DoorType.button)
            {
                if (!this.isOpen)
                {
                    if (this.doorType == DoorType.normal)
                    {
                        if (this.player.canUseIt(this.costs))
                        {
                            //Tür offen!
                            this.player.ReduceResource(this.costs);
                            OpenCloseDoor(true);
                            ShowDialog(DialogTextTrigger.success);
                        }
                        else
                        {
                            //Tür kann nicht geöffnet werden
                            ShowDialog(DialogTextTrigger.failed);
                        }
                    }
                    else
                    {
                        //Tür verschlossen
                        ShowDialog(DialogTextTrigger.failed);
                    }
                }
            }
            else if (this.doorType == DoorType.enemy)
            {
                //Wenn alle Feinde tot sind, OpenDoor()
            }
            else if (this.doorType == DoorType.button)
            {
                //Wenn Knopf gedrückt wurde, OpenDoor()
            }

            ShowDialog(DialogTextTrigger.none);
        }

        private void OpenCloseDoor(bool isOpen)
        {
            this.photonView.RPC("RpcOpenCloseDoor", RpcTarget.All, isOpen);
        }

        [PunRPC]
        protected void RpcOpenCloseDoor(bool isOpen)
        {
            this.isOpen = isOpen;

            if (this.isOpen) AnimatorUtil.SetAnimatorParameter(this.animator, "Open");
            else AnimatorUtil.SetAnimatorParameter(this.animator, "Close");

            ShowContextClue();
        }

        private void ShowContextClue()
        {
            //Wenn Spieler in Reichweite ist und Tür zu ist -> Context Clue anzeigen! Ansonsten nicht.
            if (this.isOpen) ShowContextClue(false);
            else if (!this.isOpen && PlayerCanInteract()) ShowContextClue(true);
            else ShowContextClue(false);
        }
    }
}
