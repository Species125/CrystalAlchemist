using Sirenix.OdinInspector;
using UnityEngine;

namespace CrystalAlchemist
{
    [RequireComponent(typeof(Animator))]
    public class RespawnAnimation : NetworkBehaviour
    {
        private Character character;
        private Animator animator;
        private bool isReverse = false;

        private void Awake() => this.animator = this.GetComponent<Animator>();

        public void Initialize(Character character) => this.character = character;

        public void Reverse(Character character)
        {
            this.isReverse = true;
            this.character = character;
            AnimatorUtil.SetAnimatorParameter(animator, "Reverse");
        }

        public float getAnimationLength()
        {
            return AnimatorUtil.GetAnimationLength(this.animator);
        }

        public void HideCharacter()
        {
            if (this.character != null) this.character.SetCharacterSprites(false);
        }

        public void ShowCharacter()
        {
            if (this.character != null) this.character.SetCharacterSprites(true);
        }

        public void PlayAnimation()
        {
            if (this.character != null) this.character.PlayRespawnAnimation();
        }

        public void PlaySoundEffect(AudioClip clip) => AudioUtil.playSoundEffect(clip);

        public void DestroyIt()
        {
            if (this.character != null && !this.isReverse) this.character.SpawnIn();
            Destroy(this.gameObject, 0.1f);
        }
    }
}
