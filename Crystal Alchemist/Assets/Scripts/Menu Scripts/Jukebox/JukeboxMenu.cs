using UnityEngine;

public class JukeboxMenu : MenuBehaviour
{
    [SerializeField]
    private float fadeIn = 2f;

    [SerializeField]
    private float fadeOut;

    public void PlayMusic(JukeboxButton button)
    {        
        StopMusic();
        MusicEvents.current.PlayMusic(button.GetTheme(), this.fadeIn);
    }

    public void Pause() => MusicEvents.current.TogglePause();    

    public void StopMusic() => MusicEvents.current.StopMusic(this.fadeOut);
}
