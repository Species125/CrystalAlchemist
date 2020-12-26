using UnityEngine;
using Sirenix.OdinInspector;

public class CharacterCreatorMenu : MenuBehaviour
{
    [BoxGroup("Character Creator")]
    [Required]
    public CharacterPreset playerPreset;

    [BoxGroup("Character Creator")]
    [Required]
    [SerializeField]
    private SimpleSignal signal;

    private CharacterPreset backup;


    public override void Start()
    {
        base.Start();

        this.backup = ScriptableObject.CreateInstance<CharacterPreset>();
        GameUtil.setPreset(this.playerPreset, this.backup);
    }

    public void Abort()
    {
        Undo();
        base.ExitMenu();
    }

    public void Undo()
    {
        GameUtil.setPreset(this.backup, this.playerPreset);
        UpdatePreview();
    }

    public void UpdatePreview() => this.signal.Raise();    
}
