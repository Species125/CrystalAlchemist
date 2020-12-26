using System;
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

    public Action<bool> OnCurrencyChanged;
    public Action<ItemStats> OnCollect;
    public Action<Costs> OnReduce;
    public Action<int> OnPage;
    public Action<CharacterState> OnStateChanged;
    public Action<bool> OnMenuOverlay;
    public Action<StatusEffect> OnEffectAdded;
    public Action<Vector2, Action, Action> OnSleep;
    public Action<Vector2, Action, Action> OnWakeUp;
    public Action<WarningType> OnWarning;
    public Action<float, float, float> OnCameraShake;
    public Action<float> OnCameraStill;
    public Action OnLockDirection;
    public Action<Character, bool> OnRangeTriggered;

    public Action<Character, Character, float> OnAggroHit;
    public Action<Character, Character, float> OnAggroIncrease;
    public Action<Character, Character, float> OnAggroDecrease;
    public Action<Character> OnAggroClear;

    public Func<string, bool> OnKeyItem;
    public Func<ItemGroup, int> OnItemAmount;
    public Func<Costs, bool> OnEnoughCurrency;
    public Func<bool> OnHasReturn;
    
    public Action OnCutScene;
    public Action OnTimeChanged;
    public Action OnKill;
    public Action OnTeleport;

    public Action OnEffectUpdate;
    public Action OnLifeManaUpdate;
    public Action<string> OnSceneChanged;
    //public Action<string> OnOnlineSceneChanged;

    public Action<GameObject> OnStart;
    public Action OnDeviceChanged;

    public void DoEffectAdded(StatusEffect effect) => this.OnEffectAdded?.Invoke(effect);  
    public void DoChangeState(CharacterState state) => this.OnStateChanged?.Invoke(state);  
    public void DoMenuOverlay(bool value) => this.OnMenuOverlay?.Invoke(value);

    public void DoCurrencyChange(bool show) => this.OnCurrencyChanged?.Invoke(show);
    public void DoCollect(ItemStats stats) => this.OnCollect?.Invoke(stats);    
    public void DoReduce(Costs costs) => this.OnReduce?.Invoke(costs);    
    public void DoSubmit() => this.OnSubmit?.Invoke();  
    public void DoCancel() => this.OnCancel?.Invoke();
    public void DoPage(int page) => this.OnPage?.Invoke(page);
    public void DoWarning(WarningType type) => this.OnWarning?.Invoke(type);
    public void DoSleep(Vector2 position, Action before, Action after) => this.OnSleep?.Invoke(position, before, after);
    public void DoWakeUp(Vector2 position, Action before, Action after) => this.OnWakeUp?.Invoke(position, before, after);
    public void DoDirectionLock() => this.OnLockDirection?.Invoke();

    public void DoCutScene() => this.OnCutScene?.Invoke();
    public void DoKill() => this.OnKill?.Invoke();
    public void DoTimeChange() => this.OnTimeChanged?.Invoke();
    public void DoRangeTrigger(Character character, bool value) => this.OnRangeTriggered?.Invoke(character, value);

    public void DoAggroHit(Character character, Character target, float value) => this.OnAggroHit?.Invoke(character, target, value);
    public void DoAggroIncrease(Character character, Character target, float value) => this.OnAggroIncrease?.Invoke(character, target, value);
    public void DoAggroDecrease(Character character, Character target, float value) => this.OnAggroDecrease?.Invoke(character, target, value);
    public void DoAggroClear(Character character) => this.OnAggroClear(character);

    public void DoCameraShake(float strength, float duration, float speed) => this.OnCameraShake?.Invoke(strength, duration, speed);
    public void DoCameraStill(float speed) => this.OnCameraStill?.Invoke(speed);
    public void DoTeleport() => this.OnTeleport?.Invoke();
    public void DoChangeScene(string newScene) => this.OnSceneChanged?.Invoke(newScene);
    //public void DoOnlineChangeScene(string newScene) => this.OnOnlineSceneChanged?.Invoke(newScene);

    public void DoStatusEffectUpdate() => this.OnEffectUpdate?.Invoke();
    public void DoManaLifeUpdate() => this.OnLifeManaUpdate?.Invoke();
    public void DoStart(GameObject gameObject) => this.OnStart?.Invoke(gameObject);

    public void DoDeviceChanged() => this.OnDeviceChanged?.Invoke();



    public bool HasReturn()
    {
        if (this.OnHasReturn != null) return this.OnHasReturn.Invoke();
        return false;
    }

    public bool HasKeyItem(string name)
    {
        if(this.OnKeyItem != null) return this.OnKeyItem.Invoke(name);
        return false;
    }

    public int GetItemAmount(ItemGroup item)
    {
        if (this.OnItemAmount != null) return this.OnItemAmount.Invoke(item);
        return 0;
    }

    public bool HasEnoughCurrency(Costs costs)
    {
        if (this.OnEnoughCurrency != null) return this.OnEnoughCurrency.Invoke(costs);
        return false;
    }
}
