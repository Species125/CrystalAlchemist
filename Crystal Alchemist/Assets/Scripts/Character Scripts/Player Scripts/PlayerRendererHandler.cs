using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;

namespace CrystalAlchemist
{
    [RequireComponent(typeof(Player))]
    public class PlayerRendererHandler : CharacterRenderingHandler
    {
        private Player player;

        [SerializeField]
        private CharacterPreset preset;

        private List<PlayerRenderer> parts = new List<PlayerRenderer>();

        public override void Awake()
        {
            base.Awake();

            this.player = this.GetComponent<Player>();

            if (player.isLocalPlayer)
            {
                GameEvents.current.OnPresetChange += UpdatePlayerRenderer;
                GameEvents.current.OnPresetChangeToOthers += UpdateCharacterPartsOthers;
            }
            else
            {
                this.preset = ScriptableObject.CreateInstance<CharacterPreset>();
            }
        }

        private void Start()
        {            
            UpdatePlayerRenderer();
            UpdateCharacterPartsOthers();
        }

        private void OnDestroy()
        {
            if (!player.isLocalPlayer) return;
            GameEvents.current.OnPresetChange -= UpdatePlayerRenderer;
            GameEvents.current.OnPresetChangeToOthers -= UpdateCharacterPartsOthers;
        }

        public void UpdatePlayerRenderer()
        {
            this.parts.Clear();
            UnityUtil.GetChildObjects<PlayerRenderer>(this.characterSprite.transform, this.parts);
            
            foreach (PlayerRenderer part in this.parts)
            {
                CharacterCreatorProperty property = this.preset.GetProperty(part.type);
                List<Color> colors = new List<Color>();
                if(property) colors = this.preset.getColors(property.GetColorTable());

                part.SetRenderer(property, colors);                
            }
        }

        public void UpdateCharacterPartsOthers()
        {
            if (this.player.isLocalPlayer) this.SetPresetOnOtherClients();
        }

        public void SetPresetOnOtherClients()
        {
            string race = "";
            string characterName = this.player.GetCharacterName();
            string[] colorGroups;
            string[] characterParts;

            SerializationUtil.GetPreset(this.preset, out race, out colorGroups, out characterParts);
            this.player.photonView.RPC("RpcSetPreset", RpcTarget.OthersBuffered, race, colorGroups, characterParts, characterName);
        }

        [PunRPC]
        public void RpcSetPreset(string race, string[] colorgroups, string[] parts, string characterName, PhotonMessageInfo info)
        {
            SerializationUtil.SetPreset(this.preset, race, colorgroups, parts);
            this.player.characterName = characterName;
            UpdatePlayerRenderer();
        }
    }
}
