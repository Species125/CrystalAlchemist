using Sirenix.OdinInspector;
using System;
using System.Globalization;
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

        public void ShowMiniDialog(string value)
        {
            string[] temp = value.Split(';');

            string textID = temp[0];
            string text = FormatUtil.GetLocalisedText(textID, LocalisationFileType.dialogs);

            float duration = this.dialogDuration;
            if(temp.Length == 2) duration = float.Parse(temp[1], CultureInfo.InvariantCulture.NumberFormat);

            ShowMiniDialog(text, duration);
        }

        public void ShowMiniDialog(string text, float duration)
        {
            MiniDialogBox dialogBox = Instantiate(MasterManager.miniDialogBox, this.transform);
            dialogBox.setDialogBox(text, duration, GetHeadPosition());
        }
    }
}
