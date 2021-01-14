using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace CrystalAlchemist
{
    public class CharacterCreatorButton : MonoBehaviour
    {
        [Required]
        public GameObject selectedFrame;

        [Required]
        public Image preview;

        public virtual bool IsSelected()
        {
            return false;
        }

        public virtual void Click()
        {

        }

        public void SetSelection() => this.selectedFrame.SetActive(IsSelected());
    }
}
