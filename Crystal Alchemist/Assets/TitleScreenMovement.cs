﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TitleScreenMovement : PlayerComponent
{
    private Vector2 change;
    private Vector2 position;

    [SerializeField]
    private float speedModifier = 2f;

    //delay
    //mouse steuerung für ingame

    private Vector2 target;

    public override void Updating()
    {
        if (Camera.main != null)
        {
            Vector2 pos = Input.mousePosition;
            pos = Camera.main.ScreenToWorldPoint(pos);
            target = pos;
        }

        if (Vector2.Distance(this.player.GetGroundPosition(), target) > 0.3f)
        {
            this.change = (target - this.player.GetGroundPosition()).normalized;
        }
        else this.change = Vector2.zero;
    }

    private void FixedUpdate()
    {
        UpdateAnimationAndMove(this.change);  //check if is menu
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
        if (this.player.values.currentState != CharacterState.knockedback
            && this.player.values.currentState != CharacterState.attack
            && this.player.values.currentState != CharacterState.dead
            && this.player.values.currentState != CharacterState.respawning)
        {
            Vector2 movement = new Vector2(direction.x, direction.y + (this.player.values.steps * direction.x));
            Vector2 velocity = (movement * (this.player.values.speed/this.speedModifier) * this.player.values.timeDistortion);
            if (!this.player.values.isOnIce) this.player.myRigidbody.velocity = velocity;
        }

        SetDirection(direction);
    }

    private void SetDirection(Vector2 direction)
    {
        this.player.ChangeDirection(direction);
    }
}
