using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Values/StringListValue")]
public class StringListValue : ScriptableObject
{
    [SerializeField]
    private List<string> value = new List<string>();

    public List<string> GetValue()
    {
        return this.value;
    }

    public void SetValue(List<string> value)
    {
        this.value = value;
    }
}
