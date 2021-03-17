using Sirenix.OdinInspector;
using UnityEngine;

namespace CrystalAlchemist
{
    public class BasicCharacter : NetworkBehaviour
    {
        [Required]
        [BoxGroup("Easy Access")]
        public Rigidbody2D myRigidbody;

        [Required]
        [BoxGroup("Easy Access")]
        public Animator animator;

        [Required]
        [BoxGroup("Easy Access")]
        public Collider2D characterCollider;

        [BoxGroup("Position")]
        [Tooltip("Position von Sprechblasen")]
        public GameObject headPosition;

        [BoxGroup("Dialog")]
        [Tooltip("Position von Sprechblasen")]
        private float dialogDuration = 3f;

        public virtual Vector2 GetHeadPosition()
        {
            if (this.headPosition != null) return this.headPosition.transform.position;
            return this.transform.position;
        }

        public virtual CharacterType GetCharacterType()
        {
            return CharacterType.Object;
        }

        public virtual void SetCharacterType(CharacterType type)
        {

        }


        public void ShowMiniDialog(string textID)
        {
            string text = FormatUtil.GetLocalisedText(textID, LocalisationFileType.dialogs);
            ShowMiniDialog(text, dialogDuration);
        }

        public void ShowMiniDialog(string text, float duration)
        {
            MiniDialogBox dialogBox = Instantiate(MasterManager.miniDialogBox, this.transform);
            dialogBox.setDialogBox(text, duration, GetHeadPosition());
        }
    }
}
