﻿using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class SkillLaser : SkillExtension
{
    #region Attributes

    [InfoBox("Kein Hit-Script notwendig, da kein Collider verwendet wird")]
    [SerializeField]
    private Skill impactEffect;

    [ShowIf("impactEffect")]
    [SerializeField]
    private float distanceBetweenImpacts = 1f;

    [SerializeField]
    private Light2D lights;

    [SerializeField]
    [MinValue(0)]
    private float distance = 0;

    [SerializeField]
    private SpriteRenderer laserSprite;

    [SerializeField]
    private bool targetRequired = false;

    private List<Skill> hitPoints = new List<Skill>();

    #endregion


    #region Unity Functions

    private void Update()
    {
        drawLine();
    }

    private void OnDestroy()
    {
        this.hitPoints.Clear();
    }

    #endregion


    #region Functions (private)
    private void drawLine()
    {
        Collider2D hitted = null;
        Vector2 hitPoint = Vector2.zero;

        if (targetRequired && this.skill.target == null) LineRenderUtil.Renderempty(this.laserSprite, this.lights);
        else LineRenderUtil.RenderLine(this.skill.sender, this.skill.target, this.distance, this.laserSprite, this.lights, out hitted, out hitPoint);

        if (hitted != null)
        {
            if(CollisionUtil.checkCollision(hitted, this.skill)) this.skill.hitIt(hitted);
            setImpactEffect(hitPoint);
        }
    }

    private void setImpactEffect(Vector2 hitpoint)
    {
        if (this.impactEffect != null)
        {
            bool impactPossible = true;
            this.hitPoints.RemoveAll(item => item == null);

            foreach (Skill skill in this.hitPoints)
            {
                if (Vector2.Distance(skill.transform.position, hitpoint) < this.distanceBetweenImpacts) impactPossible = false;
            }

            if (impactPossible)
            {
                Skill hitPointSkill = Instantiate(this.impactEffect, hitpoint, Quaternion.identity);
                hitPointSkill.transform.position = hitpoint;

                if (hitPointSkill != null)
                {
                    //Position nicht überschreiben
                    hitPointSkill.overridePosition = false;
                    hitPointSkill.sender = this.skill.sender;
                }

                this.hitPoints.Add(hitPointSkill);
            }
        }
    }
    #endregion
}
