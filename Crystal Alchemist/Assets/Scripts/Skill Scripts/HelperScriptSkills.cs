﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelperScriptSkills : MonoBehaviour
{
    [SerializeField]
    private StandardSkill skill;

    public void PlaySoundEffect(AudioClip clip)
    {
        this.skill.PlaySoundEffect(clip);
    }

    public void PlaySoundEffectOnce(AudioClip clip)
    {
        this.skill.PlaySoundEffectOnce(clip);
    }

    public void ActivateIt()
    {
        this.skill.ActivateIt();
    }

    public void SetTriggerActive(int value)
    {
        this.skill.SetTriggerActive(value);
    }

    public void DestroyIt()
    {
        this.skill.DestroyIt();
    }

    public void ResetRotation()
    {
        this.skill.resetRotation();
    }
}
