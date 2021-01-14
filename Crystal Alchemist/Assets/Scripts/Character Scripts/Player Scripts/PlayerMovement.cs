using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace CrystalAlchemist
{
    [RequireComponent(typeof(Player))]
    public class PlayerMovement : PlayerComponent
    {
        [SerializeField]
        private float directionLockTime = 0.25f;

        [SerializeField]
        private float mouseInputDelay = 0.2f;

        [SerializeField]
        private MouseMarker marker;

        private Vector2 change;
        private Vector2 position;
        private Vector3 mouseTargetPosition;
        private Vector2 mousePosition;

        private float lockDuration = 0;
        private bool inputPossible = true;

        #region Movement

        public void MovePlayer(InputAction.CallbackContext ctx) => SetChange(ctx);

        public void MouseClick(InputAction.CallbackContext ctx) => SetChangeMouseClick(ctx);

        public void MouseMovement(InputAction.CallbackContext ctx) => SetMousePosition(ctx);

        private void Awake()
        {
            this.mouseTargetPosition = MasterManager.globalValues.nullVector;
        }

        private void Start()
        {
            GameEvents.current.OnCutScene += SetToZero;
            GameEvents.current.OnLockDirection += SetDirectionLock;
        }

        private void OnDestroy()
        {
            GameEvents.current.OnCutScene -= SetToZero;
            GameEvents.current.OnLockDirection -= SetDirectionLock;
        }

        private void SetChange(InputAction.CallbackContext ctx)
        {
            if (!NetworkUtil.IsLocal(this.player)) return;

            if (this.player.values.CanMove())
            {
                this.change = ctx.ReadValue<Vector2>();
                this.mouseTargetPosition = MasterManager.globalValues.nullVector;
            }
        }

        private void SetToZero()
        {
            this.change = Vector2.zero;
            this.mouseTargetPosition = MasterManager.globalValues.nullVector;
        }

        private void SetMousePosition(InputAction.CallbackContext ctx)
        {
            if (!NetworkUtil.IsLocal(this.player)) return;

            this.mousePosition = ctx.ReadValue<Vector2>();

            if (!this.player.values.CanMove()
                || !Camera.main
            ) return;

            SetMouseDirection();
        }

        private void SetMouseDirection()
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(this.mousePosition);
            Vector2 direction = (mousePos - this.player.GetGroundPosition()).normalized;
            if (this.player.values.CanMove() && direction != Vector2.zero) SetDirection(direction);
        }

        private void SetChangeMouseClick(InputAction.CallbackContext ctx)
        {
            if (!NetworkUtil.IsLocal(this.player)) return;

            if (!this.player.values.CanMove()
                || !this.inputPossible
                || !Camera.main) return;

            mouseTargetPosition = Camera.main.ScreenToWorldPoint(this.mousePosition);

            Instantiate(this.marker.gameObject, (Vector2)mouseTargetPosition, Quaternion.identity);
            StartCoroutine(delayInput());
        }

        private IEnumerator delayInput()
        {
            this.inputPossible = false;
            yield return new WaitForSeconds(this.mouseInputDelay);
            this.inputPossible = true;
        }


        private void FixedUpdate()
        {
            if (!NetworkUtil.IsLocal(this.player)) return;

            MoveToMousePosition(); //Move to mouse position if target is not null
            UpdateAnimationAndMove(this.change);  //check if is menu and move to direction
            if (this.lockDuration > 0) this.lockDuration -= Time.deltaTime;
        }

        private void MoveToMousePosition()
        {
            if (this.mouseTargetPosition != MasterManager.globalValues.nullVector)
            {
                if (Vector2.Distance(this.player.GetGroundPosition(), this.mouseTargetPosition) > 0.3f)
                    this.change = ((Vector2)this.mouseTargetPosition - this.player.GetGroundPosition()).normalized;
                else SetToZero();
            }
        }

        private void UpdateAnimationAndMove(Vector2 direction)
        {
            if (this.player.values.CanMove() && direction != Vector2.zero) MoveCharacter(direction);
            else StopCharacter();

            if (this.position != (Vector2)this.player.transform.position)
            {
                SetDirection(direction);

                this.position = this.player.transform.position;
                AnimatorUtil.SetAnimatorParameter(this.player.animator, "isWalking", true);
                if (this.player.values.CanMove()) this.player.values.currentState = CharacterState.walk;
            }
            else
            {
                AnimatorUtil.SetAnimatorParameter(this.player.animator, "isWalking", false);
                if (this.player.values.currentState == CharacterState.walk) this.player.values.currentState = CharacterState.idle;
            }
        }

        private void StopCharacter()
        {
            if (this.player.values.currentState != CharacterState.knockedback
                && !this.player.values.isOnIce
                && this.player.myRigidbody.bodyType != RigidbodyType2D.Static) this.player.myRigidbody.velocity = Vector2.zero;
        }

        private void MoveCharacter(Vector2 direction)
        {
            Vector2 movement = new Vector2(direction.x, direction.y + (this.player.values.steps * direction.x));
            Vector2 velocity = (movement * this.player.values.speed * this.player.values.timeDistortion);
            if (!this.player.values.isOnIce) this.player.myRigidbody.velocity = velocity;
        }

        private void SetDirection(Vector2 direction)
        {
            if (this.player.values.CanMove() && direction != Vector2.zero && this.lockDuration <= 0)
                this.player.ChangeDirection(direction);
        }

        private void SetDirectionLock() => this.lockDuration = this.directionLockTime;

        #endregion
    }
}
