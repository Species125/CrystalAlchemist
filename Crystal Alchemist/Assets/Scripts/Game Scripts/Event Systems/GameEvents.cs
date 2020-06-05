﻿using System;
using UnityEngine;

public class GameEvents : MonoBehaviour
{
    public static GameEvents current;

    private void Awake() => Initialize();

    private void Initialize()
    {
        current = this;
        SaveSystem.loadOptions();
    }

    public Action OnSubmit;
    public Action OnCancel;
    public Action<ItemStats> OnCollect;
    public Action<Costs> OnReduce;
    public Action<int> OnPage;
    public Action<CharacterState> OnMenuOpen;
    public Action<CharacterState> OnMenuClose;
    public Action<bool> OnMenuOverlay;
    public Action<StatusEffect> OnEffectAdded;
    public Action<Vector2, Action, Action> OnSleep;
    public Action<Vector2, Action, Action> OnWakeUp;
    public Action<WarningType> OnWarning;

    public void DoEffectAdded(StatusEffect effect) => this.OnEffectAdded?.Invoke(effect);  
    public void DoMenuOpen(CharacterState state) => this.OnMenuOpen?.Invoke(state);  
    public void DoMenuClose(CharacterState state) => this.OnMenuClose?.Invoke(state);
    public void DoMenuOverlay(bool value) => this.OnMenuOverlay?.Invoke(value);
    public void DoCollect(ItemStats stats) => this.OnCollect?.Invoke(stats);    
    public void DoReduce(Costs costs) => this.OnReduce?.Invoke(costs);    
    public void DoSubmit() => this.OnSubmit?.Invoke();  
    public void DoCancel() => this.OnCancel?.Invoke();
    public void DoPage(int page) => this.OnPage?.Invoke(page);
    public void DoWarning(WarningType type) => this.OnWarning?.Invoke(type);
    public void DoSleep(Vector2 position, Action before, Action after) => this.OnSleep?.Invoke(position, before, after);
    public void DoWakeUp(Vector2 position, Action before, Action after) => this.OnWakeUp?.Invoke(position, before, after);
}
