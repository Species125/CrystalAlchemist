using System;
using UnityEngine;
using Photon.Realtime;
using ExitGames.Client.Photon;
using Photon.Pun;

namespace CrystalAlchemist
{
    public class GameEvents : MonoBehaviour
    {
        public static GameEvents current;

        private void Awake() => Initialize();

        private void Initialize()
        {
            current = this;
            SaveSystem.LoadOptions();
        }

        public Action OnSubmit;
        public Action OnCancel;
        public Action OnPresetChange;
        public Action OnPresetChangeToOthers;
        public Action<bool> OnSaveGame;
        public Action OnRoomStatusChange;

        public Action<bool> OnCurrencyChanged;
        public Action<ItemDrop,int> OnCollect;
        public Action<ProgressValue> OnProgress;
        
        public Action<Costs> OnReduce;
        public Action<int> OnPage;
        public Action<CharacterState> OnStateChanged;
        public Action<bool> OnMenuOverlay;
        public Action<StatusEffect> OnEffectAdded;
        public Action<Action> OnSleep;
        public Action<Vector2, Action> OnWakeUp;
        public Action<WarningType> OnWarning;
        public Action<float, float, float> OnCameraShake;
        public Action<float> OnCameraStill;
        public Action<Character, bool> OnRangeTriggered;
        public Action<TeleportStats> OnSetTeleportStat;

        public Func<ProgressValue, bool> OnProgressExists;
        public Func<ScriptableObject, bool> OnKeyItemExists;

        public Func<InventoryItem, int> OnItemAmount;
        public Func<Costs, bool> OnEnoughCurrency;
        public Func<bool> OnHasReturn;

        public Action OnCutScene;
        public Action OnNightChange;
        public Action OnKill;
        public Action OnInterrupt;
        public Action OnTeleport;
        public Action OnTitleScreen;
        public Action OnMenuOpened;
        public Action OnMenuClosed;
        public Action<string> OnIngameMessage;

        public Action OnEffectUpdate;
        public Action<int> OnLifeManaUpdate;
        public Action OnLifeManaUpdateLocal;
        public Action<string> OnSceneChanged;

        public Action<int> OnOtherPlayerSpawned;
        public Action OnDeviceChanged;
        public Action OnDeath;

        public Action<float> OnTimeChange;
        public Action OnTimeReset;

        public void DoRoomStatusChange() => this.OnRoomStatusChange?.Invoke();
        public void DoSaveGame(bool buffered = true) => this.OnSaveGame?.Invoke(buffered);
        public void DoPresetChange() => this.OnPresetChange?.Invoke();
        public void DoPresetChangeToOthers() => this.OnPresetChangeToOthers?.Invoke();
        public void DoEffectAdded(StatusEffect effect) => this.OnEffectAdded?.Invoke(effect);
        public void DoChangeState(CharacterState state) => this.OnStateChanged?.Invoke(state);
        public void DoMenuOverlay(bool value) => this.OnMenuOverlay?.Invoke(value);
        public void DoCurrencyChange(bool show) => this.OnCurrencyChanged?.Invoke(show);
        public void DoCollect(ItemDrop drop, int amount = 1) => this.OnCollect?.Invoke(drop, amount);
        public void DoProgress(ProgressValue value) => this.OnProgress?.Invoke(value);
        public void DoReduce(Costs costs) => this.OnReduce?.Invoke(costs);
        public void DoSubmit() => this.OnSubmit?.Invoke();
        public void DoCancel() => this.OnCancel?.Invoke();
        public void DoMenuOpen() => this.OnMenuOpened?.Invoke();
        public void DoMenuClose() => this.OnMenuClosed?.Invoke();
        public void DoIngameMessage(string text) => this.OnIngameMessage?.Invoke(text);
        public void DoPage(int page) => this.OnPage?.Invoke(page);
        public void DoWarning(WarningType type) => this.OnWarning?.Invoke(type);
        public void DoSleep(Action action) => this.OnSleep?.Invoke(action);
        public void DoWakeUp(Vector2 position, Action action) => this.OnWakeUp?.Invoke(position, action);
        public void DoCutScene() => this.OnCutScene?.Invoke();
        public void DoKill() => this.OnKill?.Invoke();
        public void DoInterrupt() => this.OnInterrupt?.Invoke();
        public void DoTitleScreen() => this.OnTitleScreen?.Invoke();
        public void DoNightChange() => this.OnNightChange?.Invoke();
        public void DoRangeTrigger(Character character, bool value) => this.OnRangeTriggered?.Invoke(character, value);
        public void DoTeleportStat(TeleportStats stats) => this.OnSetTeleportStat?.Invoke(stats);

        public void DoCameraShake(float strength, float duration, float speed) => this.OnCameraShake?.Invoke(strength, duration, speed);
        public void DoCameraStill(float speed) => this.OnCameraStill?.Invoke(speed);
        public void DoTeleport() => this.OnTeleport?.Invoke();
        public void DoChangeScene(string newScene) => this.OnSceneChanged?.Invoke(newScene);

        public void DoStatusEffectUpdate() => this.OnEffectUpdate?.Invoke();
        public void DoManaLifeUpdate(int ID) => this.OnLifeManaUpdate?.Invoke(ID);
        public void DoManaLifeUpdateLocal() => this.OnLifeManaUpdateLocal?.Invoke();

        public void DoDeviceChanged() => this.OnDeviceChanged?.Invoke();
        public void DoDeath() => this.OnDeath?.Invoke();
        public void DoTimeChange(float value) => this.OnTimeChange?.Invoke(value);
        public void DoTimeReset() => this.OnTimeReset?.Invoke();

        public void LoadLevel(string scene)
        {
            if (NetworkUtil.IsMaster())
            {
                //PhotonNetwork.LoadLevel(scene);
                object[] datas = new object[] { scene };
                RaiseEventOptions options = NetworkUtil.TargetAll();

                PhotonNetwork.RaiseEvent(NetworkUtil.SCENE_CHANGE, datas, options, SendOptions.SendUnreliable);
            }
        }

        public bool HasReturn()
        {
            if (this.OnHasReturn != null) return this.OnHasReturn.Invoke();
            return false;
        }

        public bool HasItemAlready(ScriptableObject scriptableObject)
        {
            if (this.OnKeyItemExists != null) return this.OnKeyItemExists.Invoke(scriptableObject);
            return false;
        }

        public bool HasProgress(ProgressValue value)
        {
            if (this.OnProgressExists != null) return this.OnProgressExists.Invoke(value);
            return false;
        }

        public int GetItemAmount(InventoryItem item)
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
