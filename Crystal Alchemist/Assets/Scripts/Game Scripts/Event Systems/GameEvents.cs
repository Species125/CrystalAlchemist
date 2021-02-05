using System;
using UnityEngine;

namespace CrystalAlchemist
{
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
        public Action OnPresetChange;
        public Action OnPresetChangeToOthers;

        public Action<bool> OnCurrencyChanged;
        public Action<ItemDrop> OnCollect;
        public Action<ProgressValue> OnProgress;
        
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

        public Func<ProgressValue, bool> OnProgressExists;
        public Func<string, bool> OnKeyItem;
        public Func<ItemGroup, int> OnItemAmount;
        public Func<Costs, bool> OnEnoughCurrency;
        public Func<bool> OnHasReturn;

        public Action OnCutScene;
        public Action OnNightChange;
        public Action OnKill;
        public Action OnInterrupt;
        public Action OnTeleport;
        public Action OnTitleScreen;
        public Action OnMenuOpened;
        public Action<string> OnIngameMessage;

        public Action OnEffectUpdate;
        public Action<int> OnLifeManaUpdate;
        public Action OnLifeManaUpdateLocal;
        public Action<string> OnSceneChanged;

        public Action<int> OnPlayerSpawned;
        public Action OnPlayerSpawnCompleted;
        public Action<int> OnOtherPlayerSpawned;
        public Action OnDeviceChanged;
        public Action OnDeath;

        public Action<float> OnTimeChange;
        public Action OnTimeReset;

        public void DoPresetChange() => this.OnPresetChange?.Invoke();
        public void DoPresetChangeToOthers() => this.OnPresetChangeToOthers?.Invoke();
        public void DoEffectAdded(StatusEffect effect) => this.OnEffectAdded?.Invoke(effect);
        public void DoChangeState(CharacterState state) => this.OnStateChanged?.Invoke(state);
        public void DoMenuOverlay(bool value) => this.OnMenuOverlay?.Invoke(value);
        public void DoCurrencyChange(bool show) => this.OnCurrencyChanged?.Invoke(show);
        public void DoCollect(ItemDrop drop) => this.OnCollect?.Invoke(drop);
        public void DoProgress(ProgressValue value) => this.OnProgress?.Invoke(value);
        public void DoReduce(Costs costs) => this.OnReduce?.Invoke(costs);
        public void DoSubmit() => this.OnSubmit?.Invoke();
        public void DoCancel() => this.OnCancel?.Invoke();
        public void DoMenuOpen() => this.OnMenuOpened?.Invoke();
        public void DoIngameMessage(string text) => this.OnIngameMessage?.Invoke(text);
        public void DoPage(int page) => this.OnPage?.Invoke(page);
        public void DoWarning(WarningType type) => this.OnWarning?.Invoke(type);
        public void DoSleep(Vector2 position, Action before, Action after) => this.OnSleep?.Invoke(position, before, after);
        public void DoWakeUp(Vector2 position, Action before, Action after) => this.OnWakeUp?.Invoke(position, before, after);
        public void DoDirectionLock() => this.OnLockDirection?.Invoke();
        public void DoCutScene() => this.OnCutScene?.Invoke();
        public void DoKill() => this.OnKill?.Invoke();
        public void DoInterrupt() => this.OnInterrupt?.Invoke();
        public void DoTitleScreen() => this.OnTitleScreen?.Invoke();
        public void DoNightChange() => this.OnNightChange?.Invoke();
        public void DoRangeTrigger(Character character, bool value) => this.OnRangeTriggered?.Invoke(character, value);

        public void DoAggroHit(Character character, Character target, float value) => this.OnAggroHit?.Invoke(character, target, value);
        public void DoAggroIncrease(Character character, Character target, float value) => this.OnAggroIncrease?.Invoke(character, target, value);
        public void DoAggroDecrease(Character character, Character target, float value) => this.OnAggroDecrease?.Invoke(character, target, value);
        public void DoAggroClear(Character character) => this.OnAggroClear(character);

        public void DoCameraShake(float strength, float duration, float speed) => this.OnCameraShake?.Invoke(strength, duration, speed);
        public void DoCameraStill(float speed) => this.OnCameraStill?.Invoke(speed);
        public void DoTeleport() => this.OnTeleport?.Invoke();
        public void DoChangeScene(string newScene) => this.OnSceneChanged?.Invoke(newScene);

        public void DoStatusEffectUpdate() => this.OnEffectUpdate?.Invoke();
        public void DoManaLifeUpdate(int ID) => this.OnLifeManaUpdate?.Invoke(ID);
        public void DoManaLifeUpdateLocal() => this.OnLifeManaUpdateLocal?.Invoke();

        public void DoLocalPlayerSpawned(int ID) => this.OnPlayerSpawned?.Invoke(ID);
        public void DoPlayerSpawnCompleted() => this.OnPlayerSpawnCompleted?.Invoke();
        public void DoOtherLocalPlayerSpawned(int ID) => this.OnOtherPlayerSpawned?.Invoke(ID);

        public void DoDeviceChanged() => this.OnDeviceChanged?.Invoke();
        public void DoDeath() => this.OnDeath?.Invoke();
        public void DoTimeChange(float value) => this.OnTimeChange?.Invoke(value);
        public void DoTimeReset() => this.OnTimeReset?.Invoke();


        public bool HasReturn()
        {
            if (this.OnHasReturn != null) return this.OnHasReturn.Invoke();
            return false;
        }

        public bool HasKeyItem(string name)
        {
            if (this.OnKeyItem != null) return this.OnKeyItem.Invoke(name);
            return false;
        }

        public bool HasProgress(ProgressValue value)
        {
            if (this.OnProgressExists != null) return this.OnProgressExists.Invoke(value);
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
}
