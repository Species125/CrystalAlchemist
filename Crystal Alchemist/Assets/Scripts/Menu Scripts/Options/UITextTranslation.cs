


using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace CrystalAlchemist
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class UITextTranslation : MonoBehaviour
    {
        [InfoBox("GameObject Name muss der gleiche sein wie in csv", InfoMessageType.Info)]
        [SerializeField]
        private LocalisationFileType type = LocalisationFileType.menues;

        private TextMeshProUGUI textMeshField;

        private string _id;

        private void Awake()
        {
            this.textMeshField = this.GetComponent<TextMeshProUGUI>();
            Id = this.gameObject.name;
        }

        public string Id { set { _id = value; } }

        private void Start() => SettingsEvents.current.OnLanguangeChanged += ChangeLanguageText;

        private void OnDestroy() => SettingsEvents.current.OnLanguangeChanged -= ChangeLanguageText;

        public virtual void OnEnable() => ChangeLanguageText();

        private void ChangeLanguageText()
        {
            string text = FormatUtil.GetLocalisedText(this._id, this.type);
            if (text != string.Empty) this.textMeshField.text = text;
        }
    }
}

