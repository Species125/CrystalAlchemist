using System.Collections.Generic;
using UnityEngine;

namespace CrystalAlchemist
{
    [CreateAssetMenu(menuName = "Game/Player/Player List")]
    public class PlayerList : ScriptableObject
    {
        [SerializeField]
        private List<int> players = new List<int>();

        public void AddPlayer(int ID)
        {
            if (!this.players.Contains(ID)) this.players.Add(ID);
        }

        public void RemovePlayer(int ID)
        {
            if (this.players.Contains(ID)) this.players.Remove(ID);
        }

        public List<int> GetPlayers()
        {
            return this.players;
        }
    }
}
