using UnityEngine;

namespace CrystalAlchemist
{
    [CreateAssetMenu(menuName = "Game/Settings/Game Settings")]
    public class GameSettings : ScriptableObject
    {
        public float audioVolume = 1f;

        public float soundEffectVolume = 1f;
        public float soundEffectPitch = 1f;

        public float backgroundMusicVolume = 1f;
        public float backgroundMusicPitch = 1f;

        public float backgroundMusicVolumeMenu = 0.5f;

        public Language language = Language.German;
        public bool healthBar = false;
        public bool manaBar = false;
        public int cameraDistance = 1;
        public float UISize = 1f;

        public float GetMenuVolume()
        {
            return backgroundMusicVolume * backgroundMusicVolumeMenu * audioVolume;
        }

        public float GetMusicVolume()
        {
            return backgroundMusicVolume * audioVolume;
        }

        public float GetEffectVolume()
        {
            return soundEffectVolume * audioVolume;
        }
    }
}
