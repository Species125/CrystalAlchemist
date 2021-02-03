using Sirenix.OdinInspector;
using UnityEngine;

namespace CrystalAlchemist
{
    public class Interactable : NetworkBehaviour
    {
        #region Attribute
        [BoxGroup("Activation Requirements")]
        [HideLabel]
        public Costs costs;

        [BoxGroup("Activation Requirements")]
        [SerializeField]
        private bool masterOnly = false;

        [BoxGroup("ContextMenu")]
        [SerializeField]
        private bool customActionButton = false;

        [BoxGroup("ContextMenu")]
        [ShowIf("customActionButton")]
        [SerializeField]
        private string ID;

        [BoxGroup("Effects")]
        [SerializeField]
        private bool showEffectOnEnable = false;

        private bool showEffectOnDisable = true;

        [HideInInspector]
        public bool isPlayerInRange = false;
        [HideInInspector]
        public bool isPlayerLookingAtIt = false;
        [HideInInspector]
        public Player player;
        [HideInInspector]
        public ContextClue context;

        private bool canInteract = false;

        #endregion

        #region Start Funktionen (init, ContextClue, Item set bzw. Lootregeln)

        public virtual void Start()
        {
            GameEvents.current.OnSubmit += OnSubmit;
            this.context = Instantiate(MasterManager.contextClue, this.transform.position, Quaternion.identity, this.transform);
        }

        public string translationID
        {
            get { return this.ID; }
            set { this.ID = value; }
        }

        private void OnSubmit()
        {
            if (this.canInteract) DoOnSubmit();
        }

        public virtual void Update()
        {
            if (this.isPlayerInRange) CheckInteraction();
        }

        public virtual void DoOnSubmit() { }

        private void OnDestroy() => GameEvents.current.OnSubmit -= OnSubmit;

        public override void OnEnable()
        {
            base.OnEnable();
            if (this.showEffectOnEnable) AnimatorUtil.ShowSmoke(this.transform);
        }

        public override void OnDisable()
        {
            base.OnDisable();
            if (this.showEffectOnDisable) AnimatorUtil.ShowSmoke(this.transform);
            ShowContextClue(false);
        }

        public void SetSmoke(bool value)
        {
            this.showEffectOnEnable = value;
            this.showEffectOnDisable = value;
        }

        #endregion

        #region Context Clue Funktionen

        private void CheckInteraction()
        {
            if (PlayerCanInteract())
            {
                if (this.customActionButton) MasterManager.actionButtonText.SetValue(this.ID);
                else MasterManager.actionButtonText.SetValue(string.Empty);

                ShowContextClue(true);
                this.player.values.currentState = CharacterState.interact;
                this.canInteract = true;
            }
            else 
            {
                ShowContextClue(false);
                if(this.player.values.CanInteract()) this.player.values.currentState = CharacterState.idle;
                this.canInteract = false;
            }
        }

        public virtual void ShowContextClue(bool value)
        {
            if (this.context != null) this.context.gameObject.SetActive(value);
        }

        public virtual void OnEnter(Collider2D characterCollisionBox)
        {
            if (!characterCollisionBox.isTrigger)
            {
                Player player = characterCollisionBox.GetComponent<Player>();

                if (NetworkUtil.IsLocal(player)
                && (!this.masterOnly
                || (this.masterOnly && player.isMaster))
                && this.player != player)
                {
                    this.player = player;
                    this.isPlayerInRange = true;
                }
            }
        }

        public virtual void OnExit()
        {
            if (this.player != null)
            {
                this.player.values.currentState = CharacterState.idle;
                this.player = null;                
            }

            this.isPlayerInRange = false;
            this.canInteract = false;
            this.isPlayerLookingAtIt = false;
            ShowContextClue(false);
        }

        public virtual bool PlayerCanInteract()
        {
            this.isPlayerLookingAtIt = PlayerIsLooking();

            return (this.player != null
                    && this.isPlayerInRange
                    && this.isPlayerLookingAtIt
                    && this.player.values.CanInteract());
        }

        public virtual bool PlayerIsLooking()
        {
            if (this.isPlayerInRange && CollisionUtil.checkIfGameObjectIsViewed(this.player, this.gameObject)) return true;
            return false;
        }

        //private void OnTriggerStay2D(Collider2D characterCollisionBox) => OnEnterstay(characterCollisionBox);

        private void OnTriggerEnter2D(Collider2D characterCollisionBox) => OnEnter(characterCollisionBox);

        private void OnTriggerExit2D(Collider2D characterCollisionBox)
        {
            if (!characterCollisionBox.isTrigger) OnExit();
        }

        public void ShowDialog(DialogTextTrigger trigger, ItemStats stats)
        {
            if (this.GetComponent<DialogSystem>() != null) this.GetComponent<DialogSystem>().showDialog(this.player, this, trigger, stats);
        }

        public void ShowDialog(DialogTextTrigger trigger)
        {
            if (this.GetComponent<DialogSystem>() != null) this.GetComponent<DialogSystem>().showDialog(this.player, this, trigger);
        }

        public void ShowDialog()
        {
            if (this.GetComponent<DialogSystem>() != null) this.GetComponent<DialogSystem>().showDialog(this.player, this);
        }

        public void ShowMenuDialog()
        {
            if (this.GetComponent<MenuDialogBoxLauncher>() != null) this.GetComponent<MenuDialogBoxLauncher>().ShowDialogBox();
        }
        #endregion
    }
}
