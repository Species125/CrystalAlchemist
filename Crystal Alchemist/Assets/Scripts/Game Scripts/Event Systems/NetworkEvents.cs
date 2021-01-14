using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;

namespace CrystalAlchemist
{
    public class NetworkEvents : MonoBehaviourPun
    {
        public static NetworkEvents current;

        [BoxGroup("Required")]
        [Required]
        [SerializeField]
        private StringValue teleportPath;

        private void Awake()
        {
            current = this;            
        }


        #region Skills

        public void InstantiateSkillOverNetwork(Ability ability, Character sender, Character target, RpcTarget rpcTarget)
        {
            //Player only!
            int senderID = sender.gameObject.GetPhotonView().ViewID;
            int targetID = -1;
            if (target != null) targetID = target.gameObject.GetPhotonView().ViewID;

            this.photonView.RPC("RpcInstantiateSkill", rpcTarget, ability.path, senderID, targetID);
        }


        [PunRPC]
        public void RpcInstantiateSkill(string path, int senderID, int targetID)
        {
            Ability ability = Resources.Load<Ability>(path);
            Character sender = PhotonView.Find(senderID).GetComponent<Character>();

            Character target = null;
            if (targetID >= 0) target = PhotonView.Find(targetID).GetComponent<Character>();
            ability.SetSender(sender);

            AbilityUtil.InstantiateSkill(ability, target);
        }

        #endregion


        #region Items

        public void InstantiateItem(ItemDrop drop, Vector2 position)
        {
            InstantiateItem(drop, position, false);
        }

        public void InstantiateItem(ItemDrop drop, Vector2 position, bool bounce)
        {
            InstantiateItem(drop, position, bounce, Vector2.zero);
        }

        public void InstantiateTreasureItem(ItemDrop drop, Vector2 position, bool bounce, Vector2 playerPosition)
        {
            Vector2 direction = position - playerPosition;
            InstantiateItem(drop, position, bounce, direction);
        }

        private void InstantiateItem(ItemDrop drop, Vector2 position, bool bounce, Vector2 direction)
        {            
            this.photonView.RPC("RpcInstatiateItemMaster", RpcTarget.MasterClient, drop.path, position, bounce, direction);
        }

        [PunRPC]
        public void RpcInstatiateItemMaster(string path, Vector2 position, bool bounce, Vector2 direction)
        {
            this.photonView.RPC("RpcInstantiateItem", RpcTarget.All, path, position, bounce, direction);
        }

        [PunRPC]
        public void RpcInstantiateItem(string path, Vector2 position, bool bounce, Vector2 direction)
        {
            ItemDrop drop = Resources.Load<ItemDrop>(path);
            DropItem(drop, position, bounce, direction);
        }

        private void DropItem(ItemDrop drop, Vector2 position, bool bounce, Vector2 direction)
        {
            Collectable temp = Instantiate(drop.collectable, position, Quaternion.identity);
            temp.SetBounce(bounce, direction);
            temp.name = drop.name;
            temp.SetItem(drop);
            temp.SetSelfDestruction(drop.duration, drop.hasSelfDestruction);
        }

        #endregion


        #region Character

        public void KillCharacter(Character target)
        {
            KillCharacter(target, true);
        }

        public void KillCharacter(Character target, bool value)
        {
            int targetID = target.gameObject.GetPhotonView().ViewID;
            this.photonView.RPC("RpcKillCharacterMaster", RpcTarget.MasterClient, targetID, value);
        }

        [PunRPC]
        public void RpcKillCharacterMaster(int targetID, bool value)
        {
            this.photonView.RPC("RpcKillCharacter", RpcTarget.All, targetID, value);
        }

        [PunRPC]
        public void RpcKillCharacter(int targetID, bool value)
        {
            Character target = PhotonView.Find(targetID).GetComponent<Character>();
            if (target != null) target.KillCharacter(value);
        }

        #endregion


        #region Player

        public void GetPresetFromOtherClients()
        {
            Player player = NetworkUtil.GetLocalPlayer();
            int receiverID = player.gameObject.GetPhotonView().ViewID;

            this.photonView.RPC("RpcGetPreset", RpcTarget.Others, receiverID);
        }

        [PunRPC]
        public void RpcGetPreset(int receiverID)
        {
            Player localPlayer = NetworkUtil.GetLocalPlayer();
            SetPresetOnOtherClients(localPlayer, receiverID);
        }

        public void SetPresetForOtherClients(Player player)
        {
            SetPresetOnOtherClients(player, -1);
        }

        public void SetPresetOnOtherClients(Player player, int receiver)
        {
            CharacterPreset preset = player.GetPreset();
            int targetID = player.gameObject.GetPhotonView().ViewID;
            string race = "";
            string[] colorGroups;
            string[] characterParts;

            SerializationUtil.GetPreset(preset, out race, out colorGroups, out characterParts);
            this.photonView.RPC("RpcSetPreset", RpcTarget.Others, receiver, targetID, race, colorGroups, characterParts);
        }

        [PunRPC]
        public void RpcSetPreset(int receiver, int targetID, string race, string[] colorgroups, string[] parts)
        {
            Player localPlayer = NetworkUtil.GetLocalPlayer();
            if (receiver >= 0 && localPlayer.gameObject.GetPhotonView().ViewID != receiver) return;

            PhotonView view = PhotonView.Find(targetID);
            if (view == null) return;

            Player player = view.GetComponent<Player>();
            if (player == null) return;

            CharacterPreset preset = player.GetPreset();
            SerializationUtil.SetPreset(preset, race, colorgroups, parts);
            player.UpdateCharacterParts();
        }

        #endregion


        #region Respawn

        public void HideGameObject(GameObject gameObject, bool isInit)
        {
            if (!NetworkUtil.IsMaster() || gameObject == null) return;

            int ID = gameObject.GetPhotonView().ViewID;
            this.photonView.RPC("RpcHideCharacter", RpcTarget.All, ID, isInit);
        }

        public void ShowGameObject(GameObject gameObject, bool isInit)
        {
            if (!NetworkUtil.IsMaster() || gameObject == null) return;

            int ID = gameObject.GetPhotonView().ViewID;
            this.photonView.RPC("RpcShowCharacter", RpcTarget.All, ID, isInit);
        }


        [PunRPC]
        public void RpcShowCharacter(int ID, bool isInit)
        {
            GameObject gameObject = PhotonView.Find(ID).gameObject;
            if (gameObject != null) _ShowGameObject(gameObject, isInit);
        }

        [PunRPC]
        public void RpcHideCharacter(int ID, bool isInit)
        {
            GameObject gameObject = PhotonView.Find(ID).gameObject;
            if (gameObject != null) _HideGameObject(gameObject, isInit);
        }

        private void _HideGameObject(GameObject gameObject, bool isInit)
        {
            Character character = gameObject.GetComponent<Character>();

            if (character != null)
            {
                if (character.respawnAnimation != null)
                {
                    RespawnAnimation respawnObject = Instantiate(character.respawnAnimation, character.GetSpawnPosition(), Quaternion.identity);
                    respawnObject.Reverse(character);
                    character.SetCharacterSprites(true);

                    StartCoroutine(InactiveCo(respawnObject.getAnimationLength(), character.gameObject));
                    StartCoroutine(InactiveCo(respawnObject.getAnimationLength(), respawnObject.gameObject));
                }
                else
                {
                    if (!isInit) character.PlayDespawnAnimation();
                    character.SpawnOut();
                    StartCoroutine(InactiveCo(character.GetDespawnLength(), character.gameObject));
                }

                character.values.currentState = CharacterState.respawning;
            }
            else
            {
                SetGameObjectActive(gameObject, false, isInit);
            }
        }

        private IEnumerator InactiveCo(float seconds, GameObject gameObject)
        {
            yield return new WaitForSeconds(seconds);
            gameObject.SetActive(false);
        }

        private void _ShowGameObject(GameObject gameObject, bool isInit)
        {
            Character character = gameObject.GetComponent<Character>();

            if (character != null)
            {
                character.gameObject.SetActive(true);
                character.values.currentState = CharacterState.respawning;

                if (character.respawnAnimation != null)
                {
                    //spawn character after animation
                    RespawnAnimation respawnObject = Instantiate(character.respawnAnimation, character.GetSpawnPosition(), Quaternion.identity);
                    respawnObject.Initialize(character);
                    character.SetCharacterSprites(false);
                }
                else
                {
                    //spawn character immediately
                    character.SetCharacterSprites(true);
                    if (!isInit) character.PlayRespawnAnimation();
                    character.SpawnIn();
                }
            }
            else
            {
                SetGameObjectActive(gameObject, true, isInit);
            }
        }

        private void SetGameObjectActive(GameObject gameObject, bool value, bool isInit)
        {
            //Prevent Effect on Initialize
            if (gameObject.GetComponent<Collectable>() != null)
            {
                gameObject.GetComponent<Collectable>().SetBounce(!isInit, Vector2.zero);
                gameObject.GetComponent<Collectable>().SetSmoke(!isInit);
            }
            else if (gameObject.GetComponent<Interactable>() != null)
                gameObject.GetComponent<Interactable>().SetSmoke(!isInit);

            gameObject.SetActive(value);
        }

        #endregion


        public void SetReadyWindow(bool value)
        {
            int ID = PhotonNetwork.LocalPlayer.GetPlayerNumber();
            this.photonView.RPC("RpcSetReadyWindow", RpcTarget.All, ID, value);
        }

        [PunRPC]
        public void RpcSetReadyWindow(int targetID, bool value)
        {
            MenuEvents.current.SetTeleportStatus(targetID, value);
        }

        public void ShowReadywindow(TeleportStats stats)
        {       
            this.photonView.RPC("RpcShowReadyWindow", RpcTarget.All, stats.path);
        }

        [PunRPC]
        public void RpcShowReadyWindow(string path)
        {
            this.teleportPath.SetValue(path);
            MenuEvents.current.OpenTeleport();
        }
    }
}