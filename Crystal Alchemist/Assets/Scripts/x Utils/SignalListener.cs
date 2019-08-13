﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SignalListener : MonoBehaviour
{
    public SimpleSignal signal;
    public UnityEvent signalEvent;

    public void OnSignalRaised()
    {
        this.signalEvent.Invoke();
    }

    private void OnEnable()
    {
        signal.RegisterListener(this);
    }

    private void OnDisable()
    {
        signal.DeRegisterListener(this);
    }
}
