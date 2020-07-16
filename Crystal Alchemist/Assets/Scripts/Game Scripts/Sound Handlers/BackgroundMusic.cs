using Sirenix.OdinInspector;
using UnityEngine;
using UnityEditor;

public class BackgroundMusic : MonoBehaviour
{
    [SerializeField]
    private bool playOnAwake = true;

    [SerializeField]
    private MusicTheme music;

    [SerializeField]
    private float fadeIn = 2f;

    [SerializeField]
    private float fadeOut;

    private void Start()
    {
        if (this.playOnAwake) PlayMusic();
    }

    [Button]
    public void PlayMusic()
    {
        StopMusic();
        MusicEvents.current.PlayMusic(this.music, this.fadeIn);
    }

    public void StopMusic() => MusicEvents.current.StopMusic(this.fadeOut);

    public void PlayMusic(AudioClip music)
    {
        StopMusic();
        //MusicEvents.current.PlayMusic(null, music, this.fadeIn);
    }
}
