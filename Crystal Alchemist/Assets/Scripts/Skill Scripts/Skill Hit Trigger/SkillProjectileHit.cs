﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class SkillProjectileHit : SkillHitTrigger
{
    [SerializeField]
    [Required]
    private StandardSkill skill;

    [FoldoutGroup("Projektil Attribute", expanded: false)]
    [SerializeField]
    private StandardSkill skillOnImpact;

    private void OnTriggerEnter2D(Collider2D hittedCharacter)
    {
        //Stop Arrow on Hit
        if (this.skill.sender != null
            && hittedCharacter.tag != this.skill.sender.tag
            && !hittedCharacter.isTrigger
            && !hittedCharacter.CompareTag("Room"))
        {
            Utilities.UnityUtils.SetAnimatorParameter(this.skill.animator, "Hit");
            // if (this.shadow != null) this.shadow.gameObject.SetActive(false);
            if (this.skill.myRigidbody != null) this.skill.myRigidbody.velocity = Vector2.zero;

            placeFire(hittedCharacter);

            this.skill.isActive = false;
        }
    }

    private void placeFire(Collider2D hittedCharacter)
    {
        if (this.skillOnImpact != null)
        {
            //if (!Utilities.Collisions.checkCollision(hittedCharacter, this)) hitpoint = this.transform.position;

            GameObject fire = Instantiate(this.skillOnImpact.gameObject, this.transform.position, Quaternion.identity);
            //fire.transform.position = hittedCharacter.transform.position;
            StandardSkill fireSkill = fire.GetComponent<StandardSkill>();

            if (fireSkill != null)
            {
                //Position nicht überschreiben
                if (fireSkill.GetComponent<SkillTransformModule>() != null) fireSkill.GetComponent<SkillTransformModule>().setPositionAtStart = false;
                fireSkill.sender = this.skill.sender;
            }
        }
    }
}