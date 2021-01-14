using UnityEngine;

namespace CrystalAlchemist
{
    public class VRKeyboardHandler : MonoBehaviour
    {
        [SerializeField]
        private string letters = "QWERTZUIOPÜASDFGHJKLÖÄYXCVBNM";

        [SerializeField]
        private VRKeyboardButton template;

        [SerializeField]
        private Transform content;

        [SerializeField]
        private bool isFirst = false;

        private void Awake()
        {
            char[] array = letters.ToCharArray();

            for (int i = 0; i < array.Length; i++)
            {
                char ch = array[i];

                VRKeyboardButton button = Instantiate(template, content);
                button.SetButton(ch, i);
            }

            Destroy(this.template.gameObject);
            if (!this.isFirst) this.gameObject.SetActive(false);
        }
    }
}
