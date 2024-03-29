using Sirenix.OdinInspector;
using UnityEngine;

namespace CrystalAlchemist
{
    public class BackgroundMusic : MonoBehaviour
    {
        [SerializeField]
        private MusicTheme music;

        [SerializeField]
        private float fadeIn = 2f;

        [SerializeField]
        private float fadeOut;

        private void Start()
        {
            if(music) PlayMusic();
        }

        [Button]
        public void PlayMusic() => PlayMusic(this.music, this.fadeIn, this.fadeOut);

        [Button]
        public void PlayMusic(MusicTheme theme) => PlayMusic(theme, this.fadeIn, this.fadeOut);

        private void PlayMusic(MusicTheme music, float fadeIn, float fadeOut)
        {
            StopMusic(fadeOut);
            MusicEvents.current.PlayMusic(music, fadeIn);
        }

        [Button]
        public void StopMusic() => StopMusic(this.fadeOut);

        [Button]
        public void StopMusic(float fadeOut) => MusicEvents.current.StopMusic(fadeOut);

    }
}
