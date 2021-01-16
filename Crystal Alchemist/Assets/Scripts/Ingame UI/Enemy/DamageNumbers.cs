using TMPro;
using UnityEngine;

namespace CrystalAlchemist
{
    public enum NumberColor : byte
    {
        white = 0,
        green,
        blue,
        yellow,
        red
    }

    public class DamageNumbers : MonoBehaviour
    {
        //TODO: NETWORKING RPC OTHER

        [SerializeField]
        private float factor = 1;

        [SerializeField]
        private TMP_Text textField;

        public void Initialize(string value, NumberColor color)
        {
            this.textField.text = value;
            SetColor(color);
        }

        public void Initialize(float value, NumberColor color)
        {
            float number = value * factor;
            string text = "";

            if (number != 0)
            {
                if (number > 0) text = "+" + FormatUtil.formatFloatToString(number, 1f);
                else text = FormatUtil.formatFloatToString(number, 1f);
            }

            Initialize(text, color);
        }

        private void SetColor(NumberColor mode)
        {
            this.textField.outlineWidth = 0.25f;

            if (mode == NumberColor.red)
            {
                this.textField.color = MasterManager.globalValues.red[0];
                this.textField.outlineColor = MasterManager.globalValues.red[1];
            }
            else if (mode == NumberColor.green)
            {
                this.textField.color = MasterManager.globalValues.green[0];
                this.textField.outlineColor = MasterManager.globalValues.green[1];
            }
            else if (mode == NumberColor.blue)
            {
                this.textField.color = MasterManager.globalValues.blue[0];
                this.textField.outlineColor = MasterManager.globalValues.blue[1];
            }
            else if (mode == NumberColor.yellow)
            {
                this.textField.color = MasterManager.globalValues.yellow[0];
                this.textField.outlineColor = MasterManager.globalValues.yellow[1];
            }
            else
            {
                this.textField.color = new Color32(255, 255, 255, 255);
                this.textField.outlineColor = new Color32(125, 125, 125, 255);
            }
        }

        public void DestroyIt()
        {
            Destroy(this.gameObject);
        }
    }
}