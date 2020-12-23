using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class AnimatorUtil
{
    /// <summary>
    /// returns the length of a clip with the name (not case sensitive)
    /// </summary>
    /// <param name="anim">the animator</param>
    /// <param name="name">name of the clip (not case sensitive)</param>
    /// <returns>length in float</returns>
    public static float GetAnimationLength(Animator anim, string name)
    {
        if (anim == null) return 0;
        AnimationClip[] clips = anim.runtimeAnimatorController.animationClips;
        foreach (AnimationClip clip in clips)
        {
            if (clip.name.ToUpper() == name.ToUpper()) return clip.length;
        }
        return 0;
    }


    /// <summary>
    /// returns length of the first animation
    /// </summary>
    /// <param name="anim">the animator object</param>
    /// <returns>length in float</returns>
    public static float GetAnimationLength(Animator anim)
    {
        if (anim == null) return 0;
        AnimationClip[] clips = anim.runtimeAnimatorController.animationClips;
        return clips[0].length;
    }


    public static void SetAnimDirection(Vector2 direction, Animator animator)
    {
        if (animator == null) return;

        //converting direction to max numbers (0.6 -> 1)
        Vector2 fixedDirection = Vector2Int.RoundToInt(direction);

        //converting diagonal directions to up or down
        if (fixedDirection.y == 1) fixedDirection = Vector2.up;
        else if (fixedDirection.y == -1) fixedDirection = Vector2.down;

        SetAnimatorParameter(animator, "moveX", fixedDirection.x);
        SetAnimatorParameter(animator, "moveY", fixedDirection.y);
    }

    public static void SetAnimatorParameter(List<Animator> animators, string parameter, bool value)
    {
        foreach (Animator animator in animators)
        {
            SetAnimatorParameter(animator, parameter, value);
        }
    }

    public static void SetAnimatorParameter(Animator animator, string parameter, bool value)
    {
        if (animator != null && animator.runtimeAnimatorController != null)
        {
            foreach (AnimatorControllerParameter param in animator.parameters)
            {
                if (param.name == parameter) animator.SetBool(parameter, value);
            }
        }
    }

    public static void SetAnimatorParameter(List<Animator> animators, string parameter)
    {
        foreach (Animator animator in animators)
        {
            SetAnimatorParameter(animator, parameter);
        }
    }

    public static void SetAnimatorParameter(Animator animator, string parameter)
    {
        if (animator != null && animator.runtimeAnimatorController != null)
        {
            foreach (AnimatorControllerParameter param in animator.parameters)
            {
                if (param.name == parameter) animator.SetTrigger(parameter);
            }
        }
    }

    public static void SetAnimatorParameter(List<Animator> animators, string parameter, float value)
    {
        foreach (Animator animator in animators)
        {
            SetAnimatorParameter(animator, parameter, value);
        }
    }

    public static void SetAnimatorParameter(Animator animator, string parameter, float value)
    {
        if (animator != null && animator.runtimeAnimatorController != null)
        {
            foreach (AnimatorControllerParameter param in animator.parameters)
            {
                if (param.name == parameter) animator.SetFloat(parameter, value);
            }
        }
    }

    public static void enableAnimator(List<Animator> animators, bool value)
    {
        foreach (Animator animator in animators)
        {
            animator.enabled = value;
        }
    }

    public static void SetAnimatorSpeed(List<Animator> animators, float value)
    {
        foreach (Animator animator in animators)
        {
            animator.speed = value;
        }
    }

    public static bool HasParameter(Animator animator, string parameter)
    {
        if (animator != null)
        {
            foreach (AnimatorControllerParameter param in animator.parameters)
            {
                if (param.name == parameter) return true;
            }
        }

        return false;
    }

    public static void ShowSmoke(Transform transform)
    {
        Scene currentScene = SceneManager.GetActiveScene();

        if (currentScene.isLoaded)
        {
            GameObject smoke = Object.Instantiate(MasterManager.itemDisappearSmoke, transform.position, Quaternion.identity);
            smoke.name = transform.gameObject.name + " Smoke";
        }
    }
}
