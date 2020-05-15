﻿
using UnityEngine;
using UnityEngine.Events;

public class SimpleSignalListener : MonoBehaviour
{
    public SimpleSignal signal;
    public UnityEvent signalEvent;

    public void OnSignalRaised()
    {
        this.signalEvent.Invoke();
    }

    private void OnEnable()
    {
        if (signal != null) signal.RegisterListener(this);
        else Debug.Log(this.gameObject+" - "+this.transform.parent.gameObject);
    }

    private void OnDisable()
    {
        if (signal != null) signal.DeRegisterListener(this);
        else Debug.Log(this.gameObject + " - " + this.transform.parent.gameObject);
    }
}