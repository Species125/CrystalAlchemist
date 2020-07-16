using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;

[RequireComponent(typeof(Player))]
public class PlayerMovement : PlayerComponent
{
    [SerializeField]
    private float directionLockTime = 0.25f;

    private Vector2 change;
    private Vector2 position;
    private Vector2 target;
    private float lockDuration = 0;


    #region Movement

    public void MovePlayer(InputAction.CallbackContext ctx) => SetChange(ctx);

    //public void MouseClick(InputAction.CallbackContext context) => Set();

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
        if(this.player.values.CanMove()) this.change = ctx.ReadValue<Vector2>();
    }

    private void SetToZero() => this.change = Vector2.zero;

    public void MoveToPosition()
    {
        /*if (Camera.main != null)
        {
            Vector2 pos = Input.mousePosition;
            pos = Camera.main.ScreenToWorldPoint(pos);
            target = pos;
        }

        if (Vector2.Distance(this.player.GetGroundPosition(), position.transform.position) > 0.3f)
        {
            this.change = ((Vector2)position.transform.position - this.player.GetGroundPosition()).normalized;
        }
        else this.change = Vector2.zero;*/
    }


    private void FixedUpdate()
    {
        UpdateAnimationAndMove(this.change);  //check if is menu
        if (this.lockDuration > 0) this.lockDuration -= Time.deltaTime;
    }

    private void UpdateAnimationAndMove(Vector2 direction)
    {
        if (this.player.values.CanMove() && direction != Vector2.zero) MoveCharacter(direction);
        else StopCharacter();        

        if (this.position != (Vector2)this.player.transform.position)
        {
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
        if (this.player.values.CanMove())
        {
            Vector2 movement = new Vector2(direction.x, direction.y + (this.player.values.steps * direction.x));
            Vector2 velocity = (movement * this.player.values.speed * this.player.values.timeDistortion);
            if (!this.player.values.isOnIce) this.player.myRigidbody.velocity = velocity;
        }

        if (this.lockDuration <= 0) this.player.ChangeDirection(direction);
    }

    private void SetDirectionLock() => this.lockDuration = this.directionLockTime;  

    #endregion
}
