using UnityEngine;

[CreateAssetMenu(menuName = "Values/FloatValue")]
public class FloatValue : ScriptableObject
{    
    [SerializeField]
    private float value;

    public float GetValue()
    {
        return this.value;
    }

    public void SetValue(float value)
    {
        this.value = value;
    }
}
