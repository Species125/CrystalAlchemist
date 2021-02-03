using Sirenix.OdinInspector;
using UnityEngine;

namespace CrystalAlchemist
{
    [CreateAssetMenu(menuName = "Game/Music Theme")]
    public class MusicTheme : NetworkScriptableObject
    {

        [BoxGroup("Inspector")]
        [SerializeField]
        private string audioPath = "Audio/Music/";

        [BoxGroup("Music")]
        [SerializeField]
        [Required]
        private string loopName;

        [BoxGroup("Music")]
        public bool hasIntro = false;

        [BoxGroup("Music")]
        [ShowIf("hasIntro")]
        [SerializeField]
        private string introName = "";

        [BoxGroup("Music")]
        public string componist;

        [BoxGroup("Music")]
        public string type;

        [BoxGroup("Music")]
        public Color color = Color.white;

        [Button]
        public void SetAudio(AudioClip intro, AudioClip loop)
        {
            if (intro) this.introName = intro.name;
            else this.introName = "";

            if (loop) this.loopName = loop.name;
            else this.loopName = "";
        }

        public AudioClip GetAudioClipIntro()
        {
            if (!this.hasIntro || this.introName.Replace(" ", "").Length <= 1) return null;

            string path = this.audioPath + introName;
            return Resources.Load<AudioClip>(path);
        }

        public AudioClip GetAudioClipLoop()
        {
            if (this.loopName.Replace(" ", "").Length <= 1) return null;

            string path = this.audioPath + loopName;
            return Resources.Load<AudioClip>(path);
        }

        public bool IsValid()
        {
            return (this.loopName.Replace(" ", "").Length > 1);
        }
    }
}
