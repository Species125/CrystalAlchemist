using UnityEngine;

[CreateAssetMenu(menuName = "Values/BoolValue")]
public class BoolValue : ScriptableObject
{
    [SerializeField]
    private bool value;

    public bool GetValue()
    {
        return this.value;
    }

    public void setValue(bool value)
    {
        this.value = value;
    }
}
