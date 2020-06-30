﻿using System;
using UnityEngine;

public class MusicEvents : MonoBehaviour
{
    public static MusicEvents current;

    private void Awake() => current = this;    

    public Action<AudioClip, AudioClip, float> OnBackgroundMusicPlayed;
    public Action<AudioClip, float, float> OnMusicPlayedOnce;
    public Action<AudioClip, bool, float, float> OnMusicPlayedResume;
    public Action<float> OnMusicPaused;
    public Action<float> OnMusicVolumeChanged;
    public Action<float> OnMusicResumed;
    public Action<float> OnMusicRestart;
    public Action<float> OnMusicStopped;

    public void PlayMusic(AudioClip start, AudioClip loop, float fadeIn) => this.OnBackgroundMusicPlayed?.Invoke(start, loop, fadeIn);
    public void PlayMusicOnce(AudioClip music, float fadeOld, float fadeNew) => this.OnMusicPlayedOnce?.Invoke(music, fadeNew, fadeOld);
    public void PlayMusicAndResume(AudioClip music, bool resume, float fadeOld, float fadeNew) => this.OnMusicPlayedResume?.Invoke(music, resume, fadeNew, fadeOld);
    public void PauseMusic(float fadeOut) => this.OnMusicPaused?.Invoke(fadeOut);
    public void ChangeVolume(float value) => this.OnMusicVolumeChanged?.Invoke(value);
    public void ResumeMusic(float fadeIn) => this.OnMusicResumed?.Invoke(fadeIn);
    public void RestartMusic(float fadeIn) => this.OnMusicRestart?.Invoke(fadeIn);
    public void StopMusic(float fadeOut) => this.OnMusicStopped?.Invoke(fadeOut);
}
