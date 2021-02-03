

using UnityEngine;

namespace CrystalAlchemist
{
    public class DeathAnimation : MonoBehaviour
    {
        private Character character;
        public void setCharacter(Character character)
        {
            this.character = character;
        }

        public void DestroyIt()
        {
            this.character.DestroyIt();
            Destroy(this.gameObject, 0.1f);
        }

        public void PlayDeathSoundEffect(AudioClip soundEffect)
        {
            AudioUtil.playSoundEffect(soundEffect);
        }
    }
}
