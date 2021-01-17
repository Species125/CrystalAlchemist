using Photon.Pun;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CrystalAlchemist
{
    [System.Serializable]
    public class TestPlayer
        {
        public string name = "";
        public CharacterPreset preset;
        public float life;
        public float mana;
        }

    public class Player : Character, IPunInstantiateMagicCallback
    {
        [BoxGroup("Player Objects")]
        [SerializeField]
        private float goToBedDuration = 1f;

        [BoxGroup("Player Objects")]
        [SerializeField]
        private BoolValue CutSceneValue;

        [BoxGroup("Player Objects")]
        [SerializeField]
        private PlayerSaveGame saveGame;

        [BoxGroup("Debug")]
        [SerializeField]
        private CharacterCreatorPartHandler handler;

        [BoxGroup("Debug")]
        [SerializeField]
        private CharacterPreset preset;

        [BoxGroup("Debug")]
        public bool isLocalPlayer = true;

        [BoxGroup("Debug")]
        public bool isMaster = false;

        [BoxGroup("Debug")]
        public string uniqueID;

        ///////////////////////////////////////////////////////////////

        public override void Awake()
        {
            SetComponents();
            SetScriptableObjects();
        }

        public override void SetComponents()
        {
            base.SetComponents();
            this.handler = this.GetComponent<CharacterCreatorPartHandler>();
        }

        private void SetScriptableObjects()
        {
            this.gameObject.name = "Player (Guest)";

            if (NetworkUtil.IsMaster()) this.isMaster = true;

            if (!NetworkUtil.IsLocal(this.photonView))
            {
                this.isLocalPlayer = false;
                this.stats = ScriptableObject.CreateInstance<CharacterStats>();
                this.stats.SetCharacterType(CharacterType.Friend);
                this.values = ScriptableObject.CreateInstance<CharacterValues>();
                this.saveGame = null;
                this.values.Initialize();
                this.preset = ScriptableObject.CreateInstance<CharacterPreset>();
            }
            else
            {
                this.gameObject.name = "Player (Local)";
                this.stats = this.saveGame.stats;
                this.values = this.saveGame.playerValue;
                this.values.Initialize();
                this.saveGame.attributes.SetValues();
                this.preset = this.saveGame.playerPreset;
                this.characterName = this.saveGame.characterName.GetValue();
            }

            this.handler.SetPreset(this.preset);
        }

        public override void OnEnable()
        {
            if (this.values.life <= 0) ResetValues();
        }

        public override void ResetValues()
        {
            base.ResetValues();
            if (this.saveGame != null) this.saveGame.attributes.SetValues();
            this.values.life = this.values.maxLife;
            this.values.mana = this.values.maxMana;
        }

        public override void Start()
        {
            base.Start();

            LoadPlayer();

            this.ChangeDirection(this.values.direction);
            this.animator.speed = 1;
            this.updateTimeDistortion(0);
            this.AddStatusEffectVisuals();
        }

        private void LoadPlayer()
        {
            if (!this.isLocalPlayer)
            {
                if (this.GetComponent<PlayerAbilities>() != null) Destroy(this.GetComponent<PlayerAbilities>());
                if (this.GetComponent<PlayerMovement>() != null) Destroy(this.GetComponent<PlayerMovement>());
                if (this.GetComponent<PlayerControls>() != null) Destroy(this.GetComponent<PlayerControls>());
                if (this.GetComponent<PlayerItems>() != null) Destroy(this.GetComponent<PlayerItems>());
                if (this.GetComponent<PlayerTeleport>() != null) this.GetComponent<PlayerTeleport>().Initialize();
                return;
            }

            this.saveGame.progress.Initialize();

            SceneManager.LoadScene("UI", LoadSceneMode.Additive);

            GameEvents.current.OnCollect += this.CollectIt;
            GameEvents.current.OnReduce += this.ReduceResource;
            GameEvents.current.OnStateChanged += this.SetState;
            GameEvents.current.OnSleep += this.GoToSleep;
            GameEvents.current.OnWakeUp += this.WakeUp;
            GameEvents.current.OnCutScene += this.SetCutScene;
            GameEvents.current.OnEnoughCurrency += this.HasEnoughCurrency;
            GameEvents.current.OnPresetChange += UpdateCharacterParts;

            if (this.GetComponent<PlayerAbilities>() != null) this.GetComponent<PlayerAbilities>().Initialize();
            if (this.GetComponent<PlayerMovement>() != null) this.GetComponent<PlayerMovement>().Initialize();
            if (this.GetComponent<PlayerControls>() != null) this.GetComponent<PlayerControls>().Initialize();
            if (this.GetComponent<PlayerItems>() != null) this.GetComponent<PlayerItems>().Initialize();
            if (this.GetComponent<PlayerTeleport>() != null) this.GetComponent<PlayerTeleport>().Initialize();

            GameEvents.current.DoManaLifeUpdate();
            GameEvents.current.DoPlayerSpawned(this.gameObject);

            UpdateCharacterParts();
            GetPresetFromOtherClients();

            this.saveGame.skillSet.TestInitialize(this);
        }

        public override void Update()
        {
            if (!isLocalPlayer) return;

            base.Update();
            if (this.GetComponent<PlayerAbilities>() != null) this.GetComponent<PlayerAbilities>().Updating();
        }

        public override void OnDestroy()
        {
            if (!isLocalPlayer) return;

            base.OnDestroy();
            GameEvents.current.OnPresetChange -= UpdateCharacterParts;
            GameEvents.current.OnCollect -= this.CollectIt;
            GameEvents.current.OnReduce -= this.ReduceResource;
            GameEvents.current.OnStateChanged -= this.SetState;
            GameEvents.current.OnSleep -= this.GoToSleep;
            GameEvents.current.OnWakeUp -= this.WakeUp;
            GameEvents.current.OnCutScene -= this.SetCutScene;
            GameEvents.current.OnEnoughCurrency -= this.HasEnoughCurrency;
        }

        public void GodMode(bool active)
        {
            if (active)
            {
                this.values.life = this.values.maxLife;
                this.values.mana = this.values.maxMana;
                RemoveAllStatusEffects();
            }

            SetInvincible(active);
            setCannotDie(active);
        }

        public override void SpawnOut()
        {
            base.SpawnOut();
            this.deactivateAllSkills();
        }

        public override void EnableScripts(bool value)
        {
            if (this.GetComponent<PlayerAbilities>() != null) this.GetComponent<PlayerAbilities>().enabled = value;
            //if (this.GetComponent<PlayerControls>() != null) this.GetComponent<PlayerControls>().enabled = value;
            if (this.GetComponent<PlayerMovement>() != null) this.GetComponent<PlayerMovement>().enabled = value;
            //if (this.GetComponent<PlayerInput>() != null) this.GetComponent<PlayerInput>().enabled = value;        
        }

        private void deactivateAllSkills()
        {
            for (int i = 0; i < this.values.activeSkills.Count; i++)
            {
                Skill activeSkill = this.values.activeSkills[i];
                activeSkill.DeactivateIt();
            }
        }

        public override void KillCharacter(bool animate)
        {
            if (this.values.currentState != CharacterState.dead)
            {
                this.SetDefaultDirection();

                StatusEffectUtil.RemoveAllStatusEffects(this.values.debuffs);
                StatusEffectUtil.RemoveAllStatusEffects(this.values.buffs);
                AnimatorUtil.SetAnimatorParameter(this.animator, "Dead", true);

                this.values.currentState = CharacterState.dead;
                this.myRigidbody.bodyType = RigidbodyType2D.Kinematic; //Static causes Room to empty
                MenuEvents.current.OpenDeath();
            }
        }

        ///////////////////////////////////////////////////////////////

        public override bool HasEnoughCurrency(Costs price)
        {
            if (price.resourceType == CostType.none) return true;
            else if (price.resourceType == CostType.life && this.values.life - price.amount >= 0) return true;
            else if (price.resourceType == CostType.mana && this.values.mana - price.amount >= 0) return true;
            else if (price.resourceType == CostType.keyItem && price.keyItem != null && GameEvents.current.HasKeyItem(price.keyItem.name)) return true;
            else if (price.resourceType == CostType.item && price.item != null && GameEvents.current.GetItemAmount(price.item) - price.amount >= 0) return true;

            return false;
        }


        public override void ReduceResource(Costs price)
        {
            //Shop, Door, Treasure, MiniGame, Abilities, etc
            if (price != null
                && ((price.item != null && price.item.canConsume)
                  || price.item == null))
                this.UpdateResource(price.Convert(), false);
        }

        ///////////////////////////////////////////////////////////////

        private void SetCutScene()
        {
            if (this.CutSceneValue.GetValue()) this.values.currentState = CharacterState.respawning;
            else this.values.currentState = CharacterState.idle;
        }

        public void callSignal(float addResource)
        {
            if (addResource != 0) GameEvents.current.DoManaLifeUpdate();
        }

        public override void GotHit(Skill skill, float percentage, bool knockback)
        {
            if (!this.isLocalPlayer) return; //no damage to guest

            GameEvents.current.DoCancel();
            base.GotHit(skill, percentage, knockback);
        }

        private void SetState(CharacterState state)
        {
            if (this.values.currentState == CharacterState.dead) return;
            this.values.currentState = state;
        }

        private void CollectIt(ItemStats stats)
        {
            //Collectable, Load, MiniGame, Shop und Treasure
            if (!isLocalPlayer) return;

            if (stats.itemType == ItemType.consumable)
            {
                foreach (CharacterResource resource in stats.resources) UpdateResource(resource, true);
            }
            else if (stats.itemType == ItemType.item || stats.itemType == ItemType.keyItem)
            {
                GetComponent<PlayerItems>().CollectItem(stats);
            }
            else if (stats.itemType == ItemType.ability)
            {
                //add ability to skillset
            }
            else if (stats.itemType == ItemType.outfit)
            {
                //add outfit to glamour
            }
        }

        public override void UpdateLife(float value, bool showingDamageNumber)
        {
            base.UpdateLife(value, showingDamageNumber);
            callSignal(value);

            NumberColor color = NumberColor.red;
            if (value > 0) color = NumberColor.green;
            this.photonView.RPC("RpcShowDamageNumber", RpcTarget.Others, value, (byte)color);
        }

        public override void UpdateMana(float value, bool showingDamageNumber)
        {
            base.UpdateMana(value, showingDamageNumber);
            callSignal(value);
        }

        public override void UpdateItem(ItemGroup item, int value)
        {
            GetComponent<PlayerItems>().UpdateInventory(item, value);
        }

        public override void UpdateKeyItem(ItemDrop keyItem)
        {
            GetComponent<PlayerItems>().CollectItem(keyItem.stats);
        }


        /////////////////////////////////////////////////////////////////////////////////


        public void GoToSleep(Vector2 position, Action before, Action after)
        {
            StartCoroutine(GoToBed(goToBedDuration, position, before, after));
        }

        public void WakeUp(Vector2 position, Action before, Action after)
        {
            StartCoroutine(GetUp(goToBedDuration, position, before, after));
        }

        private IEnumerator GoToBed(float duration, Vector2 position, Action before, Action after)
        {
            this.transform.position = position;
            yield return new WaitForEndOfFrame(); //Wait for Camera

            EnablePlayer(false); //Disable Movement and Collision

            before?.Invoke(); //Decke

            AnimatorUtil.SetAnimatorParameter(this.animator, "GoSleep");
            float animDuration = AnimatorUtil.GetAnimationLength(this.animator, "GoSleep");
            yield return new WaitForSeconds(animDuration);

            after?.Invoke(); //Zeit    
            this.boxCollider.enabled = true;
        }

        private void EnablePlayer(bool value)
        {
            this.EnableScripts(value); //prevent movement        
            this.boxCollider.enabled = value; //prevent input

            this.SetDefaultDirection();
        }

        private IEnumerator GetUp(float duration, Vector2 position, Action before, Action after)
        {
            this.boxCollider.enabled = false;
            before?.Invoke(); //Zeit    

            AnimatorUtil.SetAnimatorParameter(this.animator, "WakeUp");
            float animDuration = AnimatorUtil.GetAnimationLength(this.animator, "WakeUp");
            yield return new WaitForSeconds(animDuration);

            after?.Invoke(); //Decke

            EnablePlayer(true);

            this.transform.position = position;
        }


        /////////////////////////////////////////////////////////////////////////////////


        public CharacterPreset GetPreset()
        {
            return this.preset;
        }

        [Button]
        public void UpdateCharacterParts()
        {
            this.handler.UpdateCharacterParts();
            this.SetPresetForOtherClients();
        }


        public void GetPresetFromOtherClients()
        {
            Player player = NetworkUtil.GetLocalPlayer();
            int receiverID = player.gameObject.GetPhotonView().ViewID;

            this.photonView.RPC("RpcGetPreset", RpcTarget.Others, receiverID);
        }

        [PunRPC]
        public void RpcGetPreset(int receiverID, PhotonMessageInfo info)
        {
            Player localPlayer = NetworkUtil.GetLocalPlayer();
            SetPresetOnOtherClients(localPlayer, receiverID);
        }

        public void SetPresetForOtherClients()
        {
            SetPresetOnOtherClients(this, -1);
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
        public void RpcSetPreset(int receiver, int targetID, string race, string[] colorgroups, string[] parts, PhotonMessageInfo info)
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







        public void OnPhotonInstantiate(PhotonMessageInfo info)
        {
            //if(this.isLocalPlayer) PhotonNetwork.LocalPlayer.TagObject = this;
            this.photonView.Owner.TagObject = this;
        }

        public override void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            base.OnPhotonSerializeView(stream, info);

            if (stream.IsWriting)
            {
                stream.SendNext(this.uniqueID);
            }
            else
            {
                this.uniqueID = (string)stream.ReceiveNext();
            }
        }

        
    }
}