﻿using UnityEngine;
using UnityEngine.Events;

public class Trap : Terrain
{
    [SerializeField]
    private UnityEvent events;

    [SerializeField]
    private bool repeatEvents;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Trigger();
    }

    public void Trigger()
    {
        this.events?.Invoke();
        if (!this.repeatEvents) this.enabled = false;
    }
}
