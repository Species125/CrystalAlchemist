using TMPro;
using UnityEngine;

public class VRKeyboard : MonoBehaviour
{
    [SerializeField]
    private StringValue playerName;

    [SerializeField]
    private TMP_InputField input;

    [SerializeField]
    private int characterLimit = 15;

    private string result;

    private void Start()
    {
        this.input.characterLimit = this.characterLimit;
        this.result = this.playerName.GetValue();
        UpdateInputField();
    }

    public void UpdateInputField() => this.input.text = this.result;

    public void UpdateResult() => this.result = this.input.text;

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

    }

    public void Confirm()
    {
        this.playerName.SetValue(this.result);
    }
}
