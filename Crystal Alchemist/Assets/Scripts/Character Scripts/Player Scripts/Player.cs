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

    public class Player : Character
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

        [BoxGroup("Player Objects")]
        [SerializeField]
        private StringValue currentScene;

        [BoxGroup("Debug")]
        [ReadOnly]
        public bool isLocalPlayer = true;        

        [BoxGroup("Debug")]
        [ReadOnly]
        public string uniqueID;

        [BoxGroup("Debug")]
        [ReadOnly]
        public bool isMaster;

        ///////////////////////////////////////////////////////////////

        public override void Awake()
        {
            SetComponents();
            SetScriptableObjects();
        }

        private void SetScriptableObjects()
        {
            this.gameObject.name = "Player (Guest)";

            if (!NetworkUtil.IsLocal(this.photonView))
            {
                this.isLocalPlayer = false;
                this.stats = ScriptableObject.CreateInstance<CharacterStats>();                
                this.stats.SetCharacterType(CharacterType.Friend);
                this.values = ScriptableObject.CreateInstance<CharacterValues>();    
                this.saveGame = null;
                this.values.Initialize();
                this.stats.SetCharacterName("New Player");
            }
            else
            {
                this.gameObject.name = "Player (Local)";
                this.stats = this.saveGame.stats;
                this.values = this.saveGame.playerValue;
                this.values.Initialize();
                this.saveGame.attributes.SetValues();
                this.stats.SetCharacterName(this.saveGame.characterName.GetValue());
            }
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
        }

        private void LoadPlayer()
        {
            this.photonView.Owner.TagObject = this;

            if (!this.isLocalPlayer)
            {
                if (this.GetComponent<PlayerMovement>() != null) Destroy(this.GetComponent<PlayerMovement>());
                if (this.GetComponent<PlayerControls>() != null) Destroy(this.GetComponent<PlayerControls>());
                if (this.GetComponent<PlayerItems>() != null) Destroy(this.GetComponent<PlayerItems>());

                if (this.GetComponent<PlayerAbilities>() != null) this.GetComponent<PlayerAbilities>().Initialize();
                if (this.GetComponent<PlayerTeleport>() != null) this.GetComponent<PlayerTeleport>().Initialize();
                GameEvents.current.DoOtherLocalPlayerSpawned(this.photonView.ViewID);
                return;
            }

            this.saveGame.progress.Updating();

            SceneManager.LoadScene("UI", LoadSceneMode.Additive);

            GameEvents.current.OnCollect += this.CollectIt;
            GameEvents.current.OnReduce += this.ReduceResource;
            GameEvents.current.OnStateChanged += this.SetState;
            GameEvents.current.OnSleep += this.GoToSleep;
            GameEvents.current.OnWakeUp += this.WakeUp;
            GameEvents.current.OnCutScene += this.SetCutScene;
            GameEvents.current.OnEnoughCurrency += this.HasEnoughCurrency;
            GameEvents.current.OnLifeManaUpdateLocal += UpdateLifeManaUI;

            if (this.GetComponent<PlayerAbilities>() != null) this.GetComponent<PlayerAbilities>().Initialize();
            if (this.GetComponent<PlayerMovement>() != null) this.GetComponent<PlayerMovement>().Initialize();
            if (this.GetComponent<PlayerControls>() != null) this.GetComponent<PlayerControls>().Initialize();
            if (this.GetComponent<PlayerItems>() != null) this.GetComponent<PlayerItems>().Initialize();
            if (this.GetComponent<PlayerTeleport>() != null) this.GetComponent<PlayerTeleport>().Initialize();

            GameEvents.current.DoManaLifeUpdate(this.photonView.ViewID);
            GameEvents.current.DoLocalPlayerSpawned(this.photonView.ViewID);

            UpdateLifeManaForOthers();
            SetMasterFlag();

            this.saveGame.skillSet.TestInitialize(this);
            this.currentScene.SetValue(this.gameObject.scene.name);
        }

        public override void Update()
        {
            if (!isLocalPlayer) return;

            base.Update();
            if (this.GetComponent<PlayerAbilities>() != null) this.GetComponent<PlayerAbilities>().Updating();
        }

        public override void OnDestroy()
        {
            if (!isLocalPlayer)
            {
                //Destroy(this.values);
                return;
            }

            base.OnDestroy();
            GameEvents.current.OnCollect -= this.CollectIt;
            GameEvents.current.OnReduce -= this.ReduceResource;
            GameEvents.current.OnStateChanged -= this.SetState;
            GameEvents.current.OnSleep -= this.GoToSleep;
            GameEvents.current.OnWakeUp -= this.WakeUp;
            GameEvents.current.OnCutScene -= this.SetCutScene;
            GameEvents.current.OnEnoughCurrency -= this.HasEnoughCurrency;
            GameEvents.current.OnLifeManaUpdateLocal -= UpdateLifeManaUI;
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

        public override void SpawnIn()
        {
            base.SpawnIn();
            AddStatusEffectVisuals();
            if (this.isLocalPlayer) GameEvents.current.DoPlayerSpawnCompleted();
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

                GameEvents.current.DoDeath();
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

        private void UpdateLifeManaUI() => UpdateLifeManaUI(-1);

        public void UpdateLifeManaUI(float addResource)
        {
            if (addResource != 0) GameEvents.current.DoManaLifeUpdate(this.photonView.ViewID);
        }

        public override bool IsGuestPlayer()
        {
            return !this.isLocalPlayer;
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

        private void CollectIt(ItemDrop drop)
        {            
            //Collectable, Load, MiniGame, Shop und Treasure
            if (!isLocalPlayer) return;

            ItemStats stats = drop.stats;
            GameEvents.current.DoProgress(drop.progress);

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

        public override void Regenerate()
        {
            if (!this.isLocalPlayer) return;
            RegenerateLifeMana();
        }

        public override void UpdateLife(float value, bool showingDamageNumber)
        {
            base.UpdateLife(value, showingDamageNumber);
            UpdateLifeManaUI(value);

            NumberColor color = NumberColor.red;
            if (value > 0) color = NumberColor.green;
            this.photonView.RPC("RpcShowDamageNumber", RpcTarget.Others, value, (byte)color);
        }

        public override void UpdateMana(float value, bool showingDamageNumber)
        {
            base.UpdateMana(value, showingDamageNumber);
            UpdateLifeManaUI(value);
        }

        public override void UpdateItem(ItemGroup item, int value)
        {
            if(this.GetComponent<PlayerItems>() != null) this.GetComponent<PlayerItems>().UpdateInventory(item, value);
        }

        public override void UpdateKeyItem(ItemDrop keyItem)
        {
            if (this.GetComponent<PlayerItems>() != null) this.GetComponent<PlayerItems>().CollectItem(keyItem.stats);
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
            this.values.currentState = CharacterState.sleeping;
            this.myRigidbody.velocity = Vector2.zero;
            this.transform.position = position;
            yield return new WaitForEndOfFrame(); //Wait for Camera

            EnablePlayer(false); //Disable Movement and Collision

            before?.Invoke(); //Decke

            AnimatorUtil.SetAnimatorParameter(this.animator, "GoSleep");
            float animDuration = AnimatorUtil.GetAnimationLength(this.animator, "GoSleep");
            yield return new WaitForSeconds(animDuration + duration);

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
            this.myRigidbody.velocity = Vector2.zero;
            this.boxCollider.enabled = false;
            before?.Invoke(); //Zeit    

            AnimatorUtil.SetAnimatorParameter(this.animator, "WakeUp");
            float animDuration = AnimatorUtil.GetAnimationLength(this.animator, "WakeUp");
            yield return new WaitForSeconds(animDuration + duration);

            after?.Invoke(); //Decke

            EnablePlayer(true);

            this.transform.position = position;
            this.values.currentState = CharacterState.idle;
        }


        ///////////////////////////////////////////////////////////////////////////////

        private void AddStatusEffectVisuals()
        {
            AddStatusEffectVisualsOnStart(this.values.buffs);
            AddStatusEffectVisualsOnStart(this.values.debuffs);
        }

        public void AddStatusEffectVisualsOnStart(List<StatusEffect> effects)
        {
            effects.RemoveAll(item => item == null);
            for (int i = 0; i < effects.Count; i++)
            {
                effects[i].SetTarget(this);
                AddStatusEffectVisual(effects[i]);
            }
        }

        public override void AddStatusEffectVisual(StatusEffect effect)
        {
            base.AddStatusEffectVisual(effect);
            AddStatusEffectVisualOthers(effect);
        }

        public override void RemoveStatusEffectVisual(StatusEffect effect)
        {
            base.RemoveStatusEffectVisual(effect);
            RemoveStatusEffectVisualOthers(effect);
        }

        /////////////////////////////////////////////////////////////////////////////////       

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

        public override void UpdateLifeManaForOthers()
        {
            if (!this.isLocalPlayer) return;
            this.photonView.RPC("RpcUpdateLifeMana", RpcTarget.Others, this.values.life, this.values.mana, this.values.maxLife, this.values.maxMana);
            this.photonView.RPC("RpcShowDataOnClient", RpcTarget.Others);
        }
        
        [PunRPC]
        protected void RpcShowDataOnClient(PhotonMessageInfo info)
        {
            int ID = info.photonView.ViewID;
            GameEvents.current.DoManaLifeUpdate(ID);
        }

        /////////////////////////////////////////////////////////////////////////////////////////////

        public void AddStatusEffectVisualOthers(StatusEffect effect)
        {
            if (!this.isLocalPlayer) return;            
            this.photonView.RPC("RpcAddStatusEffectVisualOthers", RpcTarget.Others, effect.path);
            this.photonView.RPC("RpcShowDataOnClient", RpcTarget.Others);
        }

        [PunRPC]
        protected void RpcAddStatusEffectVisualOthers(string path, PhotonMessageInfo info)
        {
            StatusEffect effect = Resources.Load<StatusEffect>(path);
            effect.SetTarget(this);
            base.AddStatusEffectVisual(effect);
        }

        public void RemoveStatusEffectVisualOthers(StatusEffect effect)
        {
            if (!this.isLocalPlayer) return;
            this.photonView.RPC("RpcRemoveStatusEffectVisualOthers", RpcTarget.Others, effect.path);
            this.photonView.RPC("RpcShowDataOnClient", RpcTarget.Others);
        }

        [PunRPC]
        protected void RpcRemoveStatusEffectVisualOthers(string path, PhotonMessageInfo info)
        {
            StatusEffect effect = Resources.Load<StatusEffect>(path);
            base.RemoveStatusEffectVisual(effect);
        }


        public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
        {
            this.photonView.RPC("RpcSetPlayerPosition", RpcTarget.Others, (Vector2)this.transform.position);            
        }

        [PunRPC]
        protected void RpcSetPlayerPosition(Vector2 position) => this.transform.position = position;        

        private void SetMasterFlag()
        {
            if (!this.isLocalPlayer) return;
            this.isMaster = NetworkUtil.IsMaster();
            this.photonView.RPC("RpcSetMasterFlag", RpcTarget.OthersBuffered, this.isMaster);
        }

        public override void OnPlayerLeftRoom(Photon.Realtime.Player newPlayer)
        {            
            SetMasterFlag();
        }

        [PunRPC]
        protected void RpcSetMasterFlag(bool value)
        {
            this.isMaster = value;
        }
    }
}