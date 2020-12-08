using UnityEngine;

public class TimeHandler : MonoBehaviour
{
    [SerializeField]
    private TimeValue timeValue;

    private void Start()
    {
        this.timeValue.Reset();
    }

    private void FixedUpdate()
    {
        this.timeValue.setTime(Time.fixedDeltaTime);
    }
}
