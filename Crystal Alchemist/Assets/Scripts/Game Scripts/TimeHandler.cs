using UnityEngine;
using Mirror;

public class TimeHandler : NetworkBehaviour
{
    [SerializeField]
    private TimeValue timeValue;

    private void Start()
    {
        if (this.isServer) this.timeValue.Reset();
    }

    private void FixedUpdate()
    {
        if (this.isServer) this.timeValue.setTime(Time.fixedDeltaTime);
    }
}
