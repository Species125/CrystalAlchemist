using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.UI;

public class CharacterCreatorButton : MonoBehaviour
{
    [Required]
    public GameObject selectedFrame;

    [Required]
    public Image preview;

    [HideInInspector]
    public CharacterCreatorButtonHandler handler;

    public virtual bool IsSelected()
    {
        return false;
    }

    public virtual void Click()
    {
        if (this.handler == null) return;
        this.handler.UpdatePreview();
        this.handler.SetSelection();
    }

    public void SetSelection()
    {
        this.selectedFrame.SetActive(IsSelected());
    }
}
