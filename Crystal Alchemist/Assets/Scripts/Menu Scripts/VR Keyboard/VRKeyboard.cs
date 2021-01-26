using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace CrystalAlchemist
{
    public class VRKeyboard : MonoBehaviour
    {
        [SerializeField]
        private StringValue stringValue;

        [SerializeField]
        private TMP_InputField input;

        [SerializeField]
        private TextMeshProUGUI title;

        [SerializeField]
        private TextMeshProUGUI inputPlaceholder;

        [SerializeField]
        private int characterLimit = 15;

        [SerializeField]
        private GameObject upperCase;

        [SerializeField]
        private GameObject lowerCase;

        [SerializeField]
        private GameObject numbers;

        private GameObject lastMenu;

        private string result;


        private void OnEnable()
        {
            this.result = this.stringValue.GetValue();
            UpdateInputField();
        }

        private void Start()
        {
            this.input.characterLimit = this.characterLimit;
        }

        private void OnDisable()
        {
            this.upperCase.SetActive(true);
            this.lowerCase.SetActive(false);
            this.lowerCase.SetActive(false);
        }

        public void UpdateInputField()
        {
            this.input.text = this.result;
        }

        public void UpdateResult() => this.result = this.input.text;

        public void SetStringValue(StringValue value)
        {
            this.stringValue = value;
            this.result = this.stringValue.GetValue();
            UpdateInputField();
        }

        public void SetLastMenu(GameObject gameObject) => this.lastMenu = gameObject;

        public void SetCaption(string value)
        {
            if (this.inputPlaceholder.GetComponent<UITextTranslation>() != null) 
                this.inputPlaceholder.GetComponent<UITextTranslation>().ChangeName(value);

            if (this.title.GetComponent<UITextTranslation>() != null)
                this.title.GetComponent<UITextTranslation>().ChangeName(value);
        }

        public void AddChar(string ch)
        {
            if (this.result.Length < this.characterLimit) this.result += ch;
            UpdateInputField();
        }

        public void RemoveChar()
        {
            if (this.result.Length >= 1) this.result = this.result.Substring(0, this.result.Length - 1);
            UpdateInputField();
        }

        public void Back()
        {
            if (this.lastMenu) this.lastMenu.SetActive(true);
        }

        public void Confirm()
        {
            this.stringValue.SetValue(this.result);
            if (this.lastMenu) this.lastMenu.SetActive(true);            
        }
    }
}
