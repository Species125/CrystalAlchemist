using UnityEngine;
using TMPro;

public class TextMeshProExtension : MonoBehaviour
{
    [Header("Attribute")]
    [SerializeField]
    private Color fontColor;
    [SerializeField]
    private Color outlineColor;
    [SerializeField]
    private float outlineWidth = 0.25f;
    [SerializeField]
    private bool bold = false;

    private void Start() => UpdateTextMesh();    

    private void UpdateTextMesh()
    {
        TMP_Text text = this.GetComponent<TMP_Text>();
        if (text != null) FormatUtil.set3DText(text, null, this.bold, this.fontColor, this.outlineColor, this.outlineWidth);
    }
}
