﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class ProjectileSkill : StandardSkill
{
    [FoldoutGroup("Projektil Attribute", expanded: false)]
    [SerializeField]
    private StandardSkill skillOnImpact;

    #region Overrides
    public override void OnTriggerEnter2D(Collider2D hittedCharacter)
    {
        base.OnTriggerEnter2D(hittedCharacter);
        //Stop Arrow on Hit
        if (this.sender != null 
            && hittedCharacter.tag != this.sender.tag 
            && !hittedCharacter.isTrigger
            && !hittedCharacter.CompareTag("Room"))
        {       
            if (!this.playEndEffectAlready && this.endSoundEffect != null)
            {
                Utilities.playSoundEffect(this.audioSource, this.endSoundEffect);
                this.playEndEffectAlready = true;
            }

            if (!this.rotateEndSprite) this.transform.rotation = Quaternion.Euler(0,0,0);
            Utilities.SetAnimatorParameter(this.animator, "Hit");
            if (this.myRigidbody != null) this.myRigidbody.velocity = Vector2.zero;

            placeFire(hittedCharacter);

            this.isActive = false;
        }
    }

    private void placeFire(Collider2D hittedCharacter)
    {
        if (this.skillOnImpact != null)
        {
            Vector2 hitpoint = hittedCharacter.transform.position;

            if (!Utilities.checkCollision(hittedCharacter, this)) hitpoint = this.transform.position;

            GameObject fire = Instantiate(this.skillOnImpact.gameObject, hitpoint, Quaternion.identity);
            //fire.transform.position = hittedCharacter.transform.position;
            StandardSkill fireSkill = fire.GetComponent<StandardSkill>();

            if (fireSkill != null)
            {
                //Position nicht überschreiben
                fireSkill.setPositionAtStart = false;
                fireSkill.sender = this.sender;
            }
        }
    }

    #endregion
}
