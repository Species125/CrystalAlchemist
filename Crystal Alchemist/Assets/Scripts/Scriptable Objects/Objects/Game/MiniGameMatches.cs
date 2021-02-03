using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace CrystalAlchemist
{
    [System.Serializable]
    public class MiniGameMatch
    {
        [BoxGroup("$difficulty")]
        [Range(1, 6)]
        public int maxRounds;

        [BoxGroup("$difficulty")]
        [Range(1, 6)]
        public int winsNeeded;

        [BoxGroup("$difficulty")]
        [Range(1, 5)]
        public int difficulty;

        [BoxGroup("$difficulty")]
        [Range(1, 120)]
        public float maxDuration;

        [BoxGroup("$difficulty")]
        public Costs price;

        [BoxGroup("$difficulty")]
        [SerializeField]
        private Reward reward;

        private ItemDrop item;

        public void SetItem() => this.item = reward.GetItemDrop();

        public ItemDrop GetItem()
        {
            return this.item;
        }
    }

    [CreateAssetMenu(menuName = "Game/Menu/Mini Game Matches")]
    public class MiniGameMatches : ScriptableObject
    {
        [SerializeField]
        private List<MiniGameMatch> matches = new List<MiniGameMatch>();

        public void Initialize()
        {
            foreach (MiniGameMatch match in this.matches) match.SetItem();
        }

        public List<MiniGameMatch> GetMatches()
        {
            return this.matches;
        }

        public MiniGameMatch GetMatch(int difficulty)
        {
            return this.matches[difficulty - 1];
        }

        public int GetCount()
        {
            return this.matches.Count;
        }
    }
}