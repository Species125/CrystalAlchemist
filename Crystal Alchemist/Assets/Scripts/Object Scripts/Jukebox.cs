using Sirenix.OdinInspector;
using UnityEngine;

public class Jukebox : Interactable
{
    [BoxGroup("JukeBox")]
    [SerializeField]
    private BackgroundMusic defaultMusic;

    public override void DoOnSubmit()
    {
        MenuEvents.current.OpenJukeBox();
    }
}
