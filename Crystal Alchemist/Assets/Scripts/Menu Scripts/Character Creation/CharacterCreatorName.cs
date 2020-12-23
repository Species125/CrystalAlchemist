using UnityEngine;
using TMPro;
using UnityEngine.UI;

[RequireComponent(typeof(TMP_Dropdown))]
public class CharacterCreatorName : MonoBehaviour
{
    [SerializeField]
    private StringValue playerName;

    [SerializeField]
    private StringListValue values;

    private TMP_Dropdown nameDropDown;

    private void Start()
    {
        this.nameDropDown = GetComponent<TMP_Dropdown>();
        this.nameDropDown.AddOptions(values.GetValue());
        UnityUtil.SelectDropDown(this.nameDropDown, this.playerName.GetValue());
    }

    public void SetName()
    {
        this.playerName.SetValue(this.nameDropDown.captionText.text);
    }  
}
