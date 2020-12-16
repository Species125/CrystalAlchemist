using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.Events;

public enum DialogTextTrigger
{
    none,
    success,
    failed,
    empty
}

[System.Serializable]
public class DialogText
{
    [BoxGroup]
    public DialogTextTrigger trigger;
    [BoxGroup]
    public string ID;
    [BoxGroup]
    public LocalisationFileType type = LocalisationFileType.objects;
    [BoxGroup]
    public UnityEvent eventOnClose;
}


public class DialogSystem : MonoBehaviour
{
    [ButtonGroup("Add Text")]
    private void ScriptableObjects()
    {
        this.textValue = Resources.Load<StringValue>("Scriptable Objects/Menu/DialogText");
        this.eventValue = Resources.Load<EventValue>("Scriptable Objects/Menu/DialogEvent");
    }

    [ButtonGroup("Add Text")]
    private void AddShopText()
    {
        DialogText text = new DialogText();
        text.ID = "Bought";
        text.trigger = DialogTextTrigger.success;
        texts.Add(text);

        text = new DialogText();
        text.ID = "Cannot_Buy";
        text.trigger = DialogTextTrigger.failed;
        texts.Add(text);
    }

    [ButtonGroup("Add Text")]
    private void AddTreasureText()
    {
        DialogText text = new DialogText();
        text.ID = "Obtained";
        text.trigger = DialogTextTrigger.success;
        texts.Add(text);

        text = new DialogText();
        text.ID = "Cannot_Open";
        text.trigger = DialogTextTrigger.failed;
        texts.Add(text);

        text = new DialogText();
        text.ID = "Empty";
        text.trigger = DialogTextTrigger.empty;
        texts.Add(text);
    }

    [BoxGroup("Texts")]
    [SerializeField]
    private List<DialogText> texts = new List<DialogText>();

    [BoxGroup("Required")]
    [SerializeField]
    [Required]
    private StringValue textValue;

    [BoxGroup("Required")]
    [SerializeField]
    [Required]
    private EventValue eventValue;

    [HideLabel]
    [SerializeField]
    private ProgressValue progress;


    private DialogTextTrigger internalTrigger = DialogTextTrigger.none;

    public void SetDialogTrigger(DialogTextTrigger trigger) => this.internalTrigger = trigger;

    public void SetDialogSucces() => this.internalTrigger = DialogTextTrigger.success;

    public void SetDialogEmpty() => this.internalTrigger = DialogTextTrigger.empty;

    public void SetDialogNone() => this.internalTrigger = DialogTextTrigger.none;

    public void SetDialogFailed() => this.internalTrigger = DialogTextTrigger.failed;


    public void showDialog(Player player, Collectable collectable)
    {
        showDialog(player, null, DialogTextTrigger.none, collectable.GetStats());
    }

    public void showDialog(Player player, Interactable interactable)
    {
        showDialog(player, interactable, null);
    }

    public void showDialog(Player player, Interactable interactable, DialogTextTrigger trigger)
    {
        SetDialogTrigger(trigger);
        showDialog(player, interactable, null);
    }

    public void showDialog(Player player, Interactable interactable, ItemStats loot)
    {
        show(player, interactable, loot);
    }

    public void showDialog(Player player, Interactable interactable, DialogTextTrigger trigger, ItemStats loot)
    {
        SetDialogTrigger(trigger);
        show(player, interactable, loot);
    }

    private void show(Player player, Interactable interactable, ItemStats loot)
    {
        foreach (DialogText text in this.texts)
        {
            if (text.trigger == this.internalTrigger)
            {
                List<object> list = new List<object>();

                if (player != null) list.Add(player);
                if (interactable != null) list.Add(interactable);
                if (interactable != null && interactable.costs != null) list.Add(interactable.costs);
                if (loot != null) list.Add(loot);

                string result = FormatUtil.GetLocalisedText(text.ID, text.type, list);
                ShowDialogBox(player, result, text.eventOnClose);
                break;
            }
        }
    }

    private void ShowDialogBox(Player player, string text, UnityEvent onClose)
    {
        if (!this.progress.ContainsProgress()
            && player.values.currentState != CharacterState.inDialog)
        {
            this.textValue.SetValue(text);
            this.eventValue.SetValue(onClose);
            MenuEvents.current.OpenDialogBox();
            this.progress.AddProgress();
        }
    }
}
