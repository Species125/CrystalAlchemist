﻿using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Player))]
public class PlayerMovement : MonoBehaviour
{
    private Vector2 change;
    private Player player;

    #region Movement

    private void Start() => this.player = this.GetComponent<Player>();
    
    public void MovePlayer(InputAction.CallbackContext ctx) => this.change = ctx.ReadValue<Vector2>();    

    private void FixedUpdate()
    {
        if (this.player.values.currentState != CharacterState.knockedback
            && !this.player.values.isOnIce
            && this.player.myRigidbody.bodyType != RigidbodyType2D.Static) this.player.myRigidbody.velocity = Vector2.zero;

        if(this.player.values.CanMove()) UpdateAnimationAndMove(this.change);  //check if is menu
        else AnimatorUtil.SetAnimatorParameter(this.player.animator, "isWalking", false);
    }

    private void UpdateAnimationAndMove(Vector2 direction)
    {
        if (direction != Vector2.zero)
        {
            MoveCharacter(direction);
            SetDirection(direction);
            AnimatorUtil.SetAnimatorParameter(this.player.animator, "isWalking", true);
        }
        else
        {
            AnimatorUtil.SetAnimatorParameter(this.player.animator, "isWalking", false);
            if (this.player.values.currentState == CharacterState.walk) this.player.values.currentState = CharacterState.idle;
        }
    }

    private void MoveCharacter(Vector2 direction)
    {
        if (this.player.values.currentState != CharacterState.knockedback
            && this.player.values.currentState != CharacterState.attack
            && this.player.values.currentState != CharacterState.dead
            && this.player.values.currentState != CharacterState.respawning)
        {
            if (this.player.values.currentState != CharacterState.interact) this.player.values.currentState = CharacterState.walk;

            Vector2 movement = new Vector2(direction.x, direction.y + (this.player.values.steps * direction.x));
            Vector2 velocity = (movement * this.player.values.speed * this.player.values.timeDistortion);
            if (!this.player.values.isOnIce) this.player.myRigidbody.velocity = velocity;
        }
    }

    private void SetDirection(Vector2 direction)
    {
        if (!IsDirectionLocked()) this.player.ChangeDirection(direction);        
    }

    private bool IsDirectionLocked()
    {
        foreach (Skill skill in this.player.values.activeSkills)
        {
            if (skill.isDirectionLocked()) return true;            
        }
        return false;
    }

    #endregion
}
