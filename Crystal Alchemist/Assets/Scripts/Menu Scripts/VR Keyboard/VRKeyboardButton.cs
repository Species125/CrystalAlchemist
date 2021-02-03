

using TMPro;
using UnityEngine;

namespace CrystalAlchemist
{
    public class VRKeyboardButton : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI textfield;

        [SerializeField]
        private StringSignal signal;

        private VRKeyboardHandler handler;
        private string ch = "";

        public void SetButton(char ch, int i)
        {
            this.ch = ch.ToString();
            this.textfield.text = this.ch;

            if (i == 0)
            {
                this.GetComponent<ButtonExtension>().SetAsFirst();
                this.GetComponent<ButtonExtension>().ReSelect();
            }
        }

        public void Click()
        {
            this.signal.Raise(this.ch);
        }
    }
}
