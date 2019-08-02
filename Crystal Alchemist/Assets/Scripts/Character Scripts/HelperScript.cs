﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelperScript : MonoBehaviour
{
    [SerializeField]
    private Character character;

    public void DestroyIt()
    {
        character.DestroyIt();
    }

    public void DestroyItCompletely()
    {
        character.DestroyItCompletely();
    }

    public void PlayDeathSoundEffect()
    {
        character.PlayDeathSoundEffect();
    }
}
