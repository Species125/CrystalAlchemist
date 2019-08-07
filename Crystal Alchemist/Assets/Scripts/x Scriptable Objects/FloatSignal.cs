﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class FloatSignal : ScriptableObject
{
    public List<FloatSignalListener> listeners = new List<FloatSignalListener>();

    public void Raise(float value)
    {
        for (int i = listeners.Count - 1; i >= 0; i--)
        {
            this.listeners[i].OnSignalRaised(value);
        }
    }

    public void RegisterListener(FloatSignalListener listener)
    {
        this.listeners.Add(listener);
    }

    public void DeRegisterListener(FloatSignalListener listener)
    {
        this.listeners.Remove(listener);
    }
}