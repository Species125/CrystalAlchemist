using Photon.Pun;
using Sirenix.OdinInspector;
using System.Collections.Generic;

namespace CrystalAlchemist
{
    public class CrystalNetworkTest : MonoBehaviourPunCallbacks
    {
        [BoxGroup("Testing")]
        public bool testing = true;

        [BoxGroup("Testing")]
        public PlayerSaveGame saveGame;

        [BoxGroup("Testing")]
        [ShowIf("testing")]
        public List<TestPlayer> players = new List<TestPlayer>();

        private void Awake()
        {
            if (this.testing)
            {
                int rnd = UnityEngine.Random.Range(0, this.players.Count);
                TestPlayer player = this.players[rnd];
                this.saveGame.playerPreset = player.preset;
                this.saveGame.characterName.SetValue(player.name);

                this.saveGame.stats.maxLife = player.life;
                this.saveGame.stats.startLife = player.life;
                this.saveGame.stats.maxMana = player.mana;
                this.saveGame.stats.startMana = player.mana;

                this.saveGame.playerValue.Clear(this.saveGame.stats);
            }
        }
    }
}