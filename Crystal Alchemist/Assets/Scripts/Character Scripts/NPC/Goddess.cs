using UnityEngine.Events;
using UnityEngine;
using Sirenix.OdinInspector;

public class Goddess : AI
{
    [BoxGroup("Defeat")]
    [SerializeField]
    private UnityEvent onDefeated;

    public override void KillCharacter(bool animate)
    {
        for (int i = 0; i < this.values.activeSkills.Count; i++)
        {
            if (this.values.activeSkills[i].isAttachedToSender()) this.values.activeSkills[i].DeactivateIt();
        }

        RemoveAllStatusEffects();
        this.values.currentState = CharacterState.idle;

        if (this.myRigidbody != null && this.myRigidbody.bodyType != RigidbodyType2D.Static) this.myRigidbody.velocity = Vector2.zero;
                
        this.onDefeated?.Invoke();
    }



    public void SetAnimationTrigger(string value)
    {
        AnimatorUtil.SetAnimatorParameter(this.animator, value);
    }

    public void ShowMiniDialog(string translationID)
    {
        string text = FormatUtil.GetLocalisedText(translationID, LocalisationFileType.dialogs);
        ShowMiniDialog(text, 6f);
    }    
}
