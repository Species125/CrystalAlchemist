using DG.Tweening;
using UnityEngine;
using TMPro;
using System.Collections;

namespace CrystalAlchemist
{
    public class IngameMessage : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI textField;

        [SerializeField]
        private float duration;

        [SerializeField]
        private float fade;

        [SerializeField]
        LocalisationFileType translationType;

        private void Awake() => this.textField.DOFade(0, 0);        

        private void Start() => GameEvents.current.OnIngameMessage += ShowMessage;
        
        private void OnDestroy() => GameEvents.current.OnIngameMessage -= ShowMessage;
        
        private void ShowMessage(string text)
        {
            this.textField.DOFade(1, 0);
            this.textField.text = FormatUtil.GetLocalisedText(text, translationType);
            StopCoroutine(ShowTextCo());
            StartCoroutine(ShowTextCo());            
        }

        private IEnumerator ShowTextCo()
        {
            yield return new WaitForSeconds(this.duration);
            this.textField.DOFade(0, this.fade);            
        }
    }
}
