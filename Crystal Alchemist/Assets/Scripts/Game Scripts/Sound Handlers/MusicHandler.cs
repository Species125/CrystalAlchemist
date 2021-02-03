using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;

namespace CrystalAlchemist
{
    public class MusicHandler : MonoBehaviour
    {
        private MusicObject backgroundMusic;
        private MusicObject tempMusic;

        private void Start()
        {
            MusicEvents.current.OnBackgroundMusicPlayed += PlayMusic;
            MusicEvents.current.OnMusicPlayedOnce += PlayMusicOnce;
            MusicEvents.current.OnMusicPlayedResume += PlayTemporaryMusic;
            MusicEvents.current.OnMusicPaused += PauseMusic;
            MusicEvents.current.OnMusicVolumeChanged += ChangeVolume;
            MusicEvents.current.OnMusicResumed += ResumeMusic;
            MusicEvents.current.OnMusicRestart += RestartMusic;
            MusicEvents.current.OnMusicStopped += StopMusic;
            MusicEvents.current.OnPauseToggle += TogglePause;
        }

        private void OnDestroy()
        {
            MusicEvents.current.OnBackgroundMusicPlayed -= PlayMusic;
            MusicEvents.current.OnMusicPlayedOnce -= PlayMusicOnce;
            MusicEvents.current.OnMusicPlayedResume -= PlayTemporaryMusic;
            MusicEvents.current.OnMusicPaused -= PauseMusic;
            MusicEvents.current.OnMusicVolumeChanged -= ChangeVolume;
            MusicEvents.current.OnMusicResumed -= ResumeMusic;
            MusicEvents.current.OnMusicRestart -= RestartMusic;
            MusicEvents.current.OnMusicStopped -= StopMusic;
            MusicEvents.current.OnPauseToggle -= TogglePause;
        }

        private void TogglePause()
        {
            if (this.backgroundMusic == null) return;

            if (!this.backgroundMusic.paused) this.backgroundMusic.Pause(0.5f);
            else this.backgroundMusic.Resume(0.5f);
        }

        private void PlayMusic(MusicTheme music, float fadeIn)
        {
            if (!music.IsValid()) return;
            GameObject newGameObject = new GameObject("Music: " + music.name);
            MusicObject musicObject = newGameObject.AddComponent<MusicObject>();
            musicObject.Instantiate(music, fadeIn);
            this.backgroundMusic = musicObject;
        }

        private void PlayMusicOnce(AudioClip music, float fadeNew, float fadeOld)
        {
            PlayTemporaryMusic(music, false, fadeNew, fadeOld);
        }

        private void PlayTemporaryMusic(AudioClip music, bool resume, float fadeNew, float fadeOld)
        {
            if (music == null) return;
            PauseMusic(fadeOld);

            if (this.tempMusic != null)
            {
                Destroy(this.tempMusic.gameObject);
                StopCoroutine(delayCo(music.length, fadeOld));
            }

            GameObject newGameObject = new GameObject("Music: " + music.name);
            MusicObject musicObject = newGameObject.AddComponent<MusicObject>();
            musicObject.Instantiate(music, fadeNew);
            this.tempMusic = musicObject;

            if (resume) StartCoroutine(delayCo(music.length, fadeOld));
        }

        private void PauseMusic(float fadeOut)
        {
            if (this.backgroundMusic != null) this.backgroundMusic.Pause(fadeOut);
        }

        private void ResumeMusic(float fadeIn)
        {
            if (this.backgroundMusic != null) this.backgroundMusic.Resume(fadeIn);
        }

        private void RestartMusic(float fadeIn)
        {
            if (this.backgroundMusic != null) this.backgroundMusic.RestartMusic(fadeIn);
        }

        private void ChangeVolume(float volume)
        {
            if (this.backgroundMusic != null) this.backgroundMusic.ChangeVolume(volume);
        }

        private void StopMusic(float fadeOut)
        {
            if (this.backgroundMusic != null) this.backgroundMusic.Stop(fadeOut);
        }

        private IEnumerator delayCo(float delay, float fadeIn)
        {
            yield return new WaitForSeconds(delay);
            ResumeMusic(fadeIn);
        }
    }

    public class MusicObject : MonoBehaviour
    {
        private AudioClip startMusic;
        private AudioClip loopMusic;
        private AudioSource audioSource;
        private float volume;
        private bool loop = false;
        private float fadeIn;
        public bool paused = false;

        public AudioSource GetSource()
        {
            return this.audioSource;
        }

        public void Instantiate(MusicTheme theme, float fadeIn)
        {
            this.fadeIn = fadeIn;
            this.loop = true;
            this.startMusic = theme.GetAudioClipIntro();
            this.loopMusic = theme.GetAudioClipLoop();
        }

        private void FadeIn(float value)
        {
            this.audioSource.volume = 0;
            this.audioSource.DOFade(this.volume, value);
        }

        private void FadeOut(float value)
        {
            this.audioSource.DOFade(0, value);
        }

        public void Instantiate(AudioClip music, float fadeIn)
        {
            this.fadeIn = fadeIn;
            this.loopMusic = music;
            Destroy(this.gameObject, music.length);
        }

        private void Start()
        {
            this.volume = MasterManager.settings.backgroundMusicVolume;
            this.audioSource = this.gameObject.AddComponent<AudioSource>();

            Initialize();
        }

        private void Initialize()
        {
            FadeIn(this.fadeIn);

            if (this.startMusic != null)
            {
                this.audioSource.loop = false;
                this.audioSource.PlayOneShot(this.startMusic);
            }
            else
            {
                playLoop();
            }
        }

        private void Update()
        {
            if (this.startMusic != null && !this.paused && !this.audioSource.isPlaying) playLoop();
        }

        public void RestartMusic(float fadeIn)
        {
            this.fadeIn = fadeIn;
            this.audioSource.Stop();
            Initialize();
        }

        public void ChangeVolume(float volume)
        {
            this.audioSource.volume = volume;
        }

        public void Pause(float fadeOut)
        {
            StartCoroutine(stopCO(fadeOut, this.audioSource.Pause));
            this.paused = true;
        }

        public void Stop(float fadeOut)
        {
            StartCoroutine(stopCO(fadeOut, null));
            Destroy(this.gameObject, fadeOut);
        }

        private IEnumerator stopCO(float fadeOut, Action action)
        {
            FadeOut(fadeOut);
            yield return new WaitForSeconds(fadeOut);
            action?.Invoke();
        }

        public void Resume(float fadeIn)
        {
            this.paused = false;
            this.audioSource.UnPause();
            FadeIn(fadeIn);
        }



        private void playLoop()
        {
            if (this.loopMusic != null)
            {
                this.audioSource.clip = this.loopMusic;
                this.audioSource.loop = this.loop;
                this.audioSource.Play();
            }
        }
    }
}