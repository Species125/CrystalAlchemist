using System;

using UnityEngine;

namespace CrystalAlchemist
{
    public class MiniGameEvents : MonoBehaviour
    {
        public static MiniGameEvents current;

        private void Awake() => current = this;

        public Action<MiniGameMatch> OnStartRound;
        public Action OnDifficultyChanged;
        public Action<bool> OnEndRound;

        public void SetDifficulty() => this.OnDifficultyChanged?.Invoke();
        public void StartMiniGameRound(MiniGameMatch match) => this.OnStartRound?.Invoke(match);
        public void EndMiniGameRound(bool value) => this.OnEndRound?.Invoke(value);
    }
}
