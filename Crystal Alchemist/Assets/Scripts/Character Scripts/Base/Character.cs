using DG.Tweening;
using Photon.Pun;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CrystalAlchemist
{
    [RequireComponent(typeof(PhotonView))]
    public class Character : NetworkBehaviour, IPunObservable
    {
        #region Basic Attributes

        [Required]
        [BoxGroup("Easy Access")]
        public Rigidbody2D myRigidbody;

        [Required]
        [BoxGroup("Easy Access")]
        public Animator animator;

        [Required]
        [BoxGroup("Easy Access")]
        public Collider2D boxCollider;

        [BoxGroup("Easy Access")]
        public RespawnAnimation respawnAnimation;

        [BoxGroup("Position")]
        [Required]
        [SerializeField]
        [Tooltip("Zur Erkennung wo der Charakter steht. Nicht Hauptsprite wählen!")]
        private GameObject groundPosition;

        [BoxGroup("Position")]
        [SerializeField]
        [Tooltip("Position des Skills")]
        private GameObject skillStartPosition;

        [BoxGroup("Position")]
        [SerializeField]
        [Tooltip("Position von Sprechblasen")]
        private GameObject headPosition;

        [BoxGroup("Parent")]
        [Required]
        public GameObject activeSkillParent;

        [BoxGroup("Parent")]
        [Required]
        public GameObject activeStatusEffectParent;

        #endregion

        #region Attributes

        private bool isVisible;
        private float selfDestructionElapsed;
        private float regenTimeElapsed;
        private float manaTime;
        private DeathAnimation activeDeathAnimation;
        private Vector3 spawnPosition;
        private List<StatusEffectGameObject> statusEffectVisuals = new List<StatusEffectGameObject>();

        [BoxGroup("Debug")]
        public CharacterStats stats;

        [BoxGroup("Debug")]
        public CharacterValues values;

        [HideInInspector]
        public bool IsSummoned = false;

        [HideInInspector]
        public string characterName;

        #endregion

        #region Start Functions (Spawn, Init)
        public virtual void Awake()
        {
            this.values = ScriptableObject.CreateInstance<CharacterValues>(); //create new Values when not already assigned (NPC)
            this.values.Initialize();
            this.characterName = this.stats.GetCharacterName();

            this.spawnPosition = this.transform.position;
            SetComponents();
        }

        public override void OnEnable()
        {
            base.OnEnable();
            this.transform.position = this.spawnPosition;
            ResetValues();
        }

        public virtual void Start() => GameEvents.current.OnEffectAdded += AddStatusEffectVisuals;

        public virtual void SetComponents()
        {
            if (this.myRigidbody == null) this.myRigidbody = this.GetComponent<Rigidbody2D>();
            if (this.skillStartPosition == null) this.skillStartPosition = this.gameObject;
            if (this.animator == null) this.animator = this.GetComponent<Animator>();
            if (this.boxCollider == null) this.boxCollider = GetComponent<Collider2D>();
            if (this.boxCollider != null) this.boxCollider.gameObject.tag = this.transform.gameObject.tag;
        }

        public virtual void ResetValues()
        {
            this.values.Clear(this.stats);

            this.SetDefaultDirection();
            this.animator.speed = 1;
            this.animator.enabled = true;
            this.updateTimeDistortion(0);
            this.updateSpellSpeed(0);            
            this.SetCharacterSprites(true);
            this.activeDeathAnimation = null;

            if (this.stats.isMassive) this.myRigidbody.bodyType = RigidbodyType2D.Kinematic;
            else this.myRigidbody.bodyType = RigidbodyType2D.Dynamic;

            if (this.GetComponent<CharacterRenderingHandler>() != null) this.GetComponent<CharacterRenderingHandler>().Reset();
            if (this.boxCollider != null) this.boxCollider.enabled = true;

            if (this.stats.hasSelfDestruction) this.selfDestructionElapsed = this.stats.selfDestructionTimer;
        }

        public void InitializeAddSpawn()
        {
            InitializeAddSpawn(false, 0);
        }

        public void InitializeAddSpawn(bool hasMaxDuration, float maxDuration)
        {
            this.IsSummoned = true;
            this.stats = Instantiate(this.stats);
            this.stats.hasRespawn = false;
            this.transform.SetParent(null);

            if (hasMaxDuration && maxDuration > 0)
            {
                this.stats.hasSelfDestruction = true;
                this.stats.selfDestructionTimer = maxDuration;
            }
        }

        public virtual void OnDestroy() => GameEvents.current.OnEffectAdded -= AddStatusEffectVisuals;

        #endregion

        #region Updates

        public virtual void Update()
        {
            UpdateSelfDestruction();

            if (this.values.currentState == CharacterState.dead
                || this.values.currentState == CharacterState.respawning) return;

            Regenerate();
            UpdateLifeAnimation();
            UpdateStatusEffects();

        }

        private void UpdateSelfDestruction()
        {
            if (this.stats.hasSelfDestruction)
            {
                if (this.selfDestructionElapsed > 0) this.selfDestructionElapsed -= (Time.deltaTime * this.values.timeDistortion);
                else KillIt();
            }
        }

        public void CheckDeath()
        {
            if (this.values.life <= 0
                && !this.values.cannotDie //Item
                && !this.values.isInvincible //Event
                && !this.values.cantBeHit) //after Hit
                KillIt();
        }

        private void UpdateStatusEffects()
        {
            UpdateStatusEffectGroup(this.values.buffs);
            UpdateStatusEffectGroup(this.values.debuffs);
        }

        private void UpdateLifeAnimation()
        {
            float percentage = this.values.life * 100 / this.values.maxLife;
            AnimatorUtil.SetAnimatorParameter(this.animator, "Life", percentage);
        }

        private void Regenerate()
        {
            if (this.values.currentState != CharacterState.dead
                && this.values.currentState != CharacterState.respawning
                && this.stats.canRegenerate)
            {
                if (this.regenTimeElapsed >= this.stats.regenerationInterval)
                {
                    this.regenTimeElapsed = 0;
                    if (this.values.lifeRegen != 0 && this.values.life < this.values.maxLife) UpdateResource(CostType.life, this.values.lifeRegen);
                    if (this.values.manaRegen != 0 && this.values.mana < this.values.maxMana) UpdateResource(CostType.mana, this.values.manaRegen, false);
                }
                else
                {
                    this.regenTimeElapsed += (Time.deltaTime * this.values.timeDistortion);
                }
            }
        }

        #endregion

        #region Item Functions (drop Item, Lootregeln)

        public void DropItem()
        {
            if (this.values.itemDrop != null && NetworkUtil.IsMaster())
                NetworkEvents.current.InstantiateItem(this.values.itemDrop, this.transform.position, false);
        }

        public void DropItem(GameObject position)
        {
            if (this.values.itemDrop != null && NetworkUtil.IsMaster())
                NetworkEvents.current.InstantiateItem(this.values.itemDrop, position.transform.position, true);
        }

        #endregion

        #region Animation and Direction

        public void SetDefaultDirection() => ChangeDirection(Vector2.down);

        public void ChangeDirection(Vector2 direction)
        {
            if (direction == Vector2.zero) direction = Vector2.down;

            this.values.direction = direction;
            AnimatorUtil.SetAnimDirection(direction, this.animator);
        }
        #endregion

        #region Color Changes

        public void removeColor(Color color)
        {
            if (this.GetComponent<CharacterRenderingHandler>() != null)
                this.GetComponent<CharacterRenderingHandler>().RemoveTint(color);
        }

        public void ChangeColor(Color color)
        {
            if (this.GetComponent<CharacterRenderingHandler>() != null)
                this.GetComponent<CharacterRenderingHandler>().ChangeTint(color);
        }

        public void InvertColor(bool value)
        {
            if (this.GetComponent<CharacterRenderingHandler>() != null)
                this.GetComponent<CharacterRenderingHandler>().Invert(value);
        }

        #endregion

        #region Update Resources

        public void updateSpeed(int addSpeed)
        {
            updateSpeed(addSpeed, true);
        }

        public void updateSpeed(int addSpeed, bool affectAnimation)
        {
            float startSpeedInPercent = this.stats.startSpeed / 100;
            float addNewSpeed = startSpeedInPercent * ((float)addSpeed / 100);
            float changeSpeed = startSpeedInPercent + addNewSpeed;

            this.values.speed = changeSpeed * this.values.timeDistortion * this.values.speedFactor;
            if (affectAnimation && this.stats.startSpeed > 0)
                this.animator.speed = this.values.speed / (this.stats.startSpeed * this.values.speedFactor / 100);
        }

        public void updateSpellSpeed(float addSpellSpeed)
        {
            this.values.spellspeed = ((this.stats.startSpellSpeed / 100) + (addSpellSpeed / 100)) * this.values.timeDistortion;
        }

        public void updateTimeDistortion(float distortion)
        {
            this.values.timeDistortion = 1 + (distortion / 100);
            updateAnimatorSpeed(this.values.timeDistortion);

            foreach (StatusEffect effect in this.values.buffs)
            {
                effect.UpdateTimeDistortion(distortion);
            }

            foreach (StatusEffect effect in this.values.debuffs)
            {
                effect.UpdateTimeDistortion(distortion);
            }
        }

        public void updateAnimatorSpeed(float value)
        {
            if (this.animator != null) this.animator.speed = value;
        }

        public void UpdateResource(CostType type, float addResource)
        {
            //Life Regeneration, Hole, Savepoint
            UpdateResource(type, addResource, true);
        }

        public void UpdateResource(CostType type, float addResource, bool showingDamageNumber)
        {
            //Mana Regeneration
            CharacterResource resource = new CharacterResource(type, addResource);
            UpdateResource(resource, showingDamageNumber);
        }

        public void UpdateResource(CharacterResource resource)
        {
            //Statuseffect, Skill Hit, Fountain
            UpdateResource(resource, true);
        }

        public virtual void ReduceResource(Costs price)
        {
            //No Costs for AI
        }

        public void UpdateResource(CharacterResource resource, bool showingDamageNumber)
        {
            CostType resourceType = resource.resourceType;
            float amount = resource.amount;

            if (resourceType == CostType.life) UpdateLife(amount, showingDamageNumber);
            else if (resourceType == CostType.mana) UpdateMana(amount, showingDamageNumber);
            else if (resourceType == CostType.item) UpdateItem(resource.item, (int)amount);
            else if (resourceType == CostType.keyItem) UpdateKeyItem(resource.keyItem);
            else if (resourceType == CostType.statusEffect) UpdateStatusEffect(resource.statusEffect, (int)amount);

            CheckDeath();
        }

        public virtual void UpdateLife(float value, bool showingDamageNumber)
        {
            this.values.life = GameUtil.setResource(this.values.life, this.values.maxLife, value);

            NumberColor color = NumberColor.red;
            if (value > 0) color = NumberColor.green;

            if (this.values.life > 0
                && this.values.currentState != CharacterState.dead
                && showingDamageNumber)
            {
                ShowDamageNumber(value, color);
            }
        }

        public virtual void UpdateMana(float value, bool showingDamageNumber)
        {
            this.values.mana = GameUtil.setResource(this.values.mana, this.values.maxMana, value);
            if (showingDamageNumber && value > 0) ShowDamageNumber(value, NumberColor.blue);
        }

        public virtual void UpdateItem(ItemGroup item, int value)
        {

        }

        public virtual void UpdateKeyItem(ItemDrop keyItem)
        {

        }

        public virtual void UpdateStatusEffect(StatusEffect effect, int value)
        {
            //TODO: Dispell with amount (value negativ)
            for (int i = 0; i < value; i++) StatusEffectUtil.AddStatusEffect(effect, this);
        }

        #endregion

        #region Damage Functions

        private void showDamageNumber(float value) => ShowDamageNumber(value, NumberColor.yellow);

        private void ShowDamageNumber(float value, NumberColor color)
        {
            if (this.stats.showDamageNumbers) RpcShowDamageNumber(value, (byte)color);  
        }

        [PunRPC]
        protected void RpcShowDamageNumber(float value, byte color)
        {
            DamageNumbers damageNumberClone = Instantiate(MasterManager.damageNumber, this.transform.position, Quaternion.identity, this.transform);
            damageNumberClone.Initialize(value, (NumberColor)color);
        }

        public void GotHit(Skill skill) => GotHit(skill, 100);

        public void GotHit(Skill skill, float percentage) => GotHit(skill, percentage, true);

        public virtual void GotHit(Skill skill, float percentage, bool knockback)
        {
            SkillTargetModule targetModule = skill.GetComponent<SkillTargetModule>();
            Character sender = skill.sender;

            if (!NetworkUtil.IsLocal(sender.GetComponent<Player>()) //no guestplayer hits
                || targetModule == null
                || this.values.currentState == CharacterState.dead
                || (!targetModule.affections.CanIgnoreInvinvibility() && this.values.cantBeHit)) return;

            Vector2 position = skill.transform.position;
            int ID = sender.photonView.ViewID;
            string[] resources = targetModule.GetAffectedResourcesArray(this);
            float thrust = targetModule.thrust;
            float duration = targetModule.knockbackTime;

            this.photonView.RPC("RpcGotHit", RpcTarget.All, ID, resources,
                            position, percentage, knockback, thrust, duration);
        }

        [PunRPC]
        protected void RpcGotHit(int senderID, string[] resourcesArray, Vector2 position,
                                 float percentage, bool knockback, float thrust, float duration)
        {
            List<CharacterResource> resources = new List<CharacterResource>();
            foreach (string elem in resourcesArray) resources.Add(new CharacterResource(elem));

            Character sender = NetworkUtil.GetCharacter(senderID);
            GetDamage(sender, resources, position, percentage, knockback, thrust, duration);
        }

        private void GetDamage(Character sender, List<CharacterResource> resources, Vector2 position,
                               float percentage, bool knockback, float thrust, float duration)
        {
            if (this.values.isInvincible)
            {
                showDamageNumber(0);
                SetCannotHit(false);
            }
            else
            {
                foreach (CharacterResource elem in resources)
                {
                    elem.amount *= percentage / 100;
                    UpdateResource(elem);

                    if (this.values.life > 0 && elem.resourceType == CostType.life && elem.amount < 0)
                    {
                        GameEvents.current.DoAggroHit(this, sender, elem.amount);

                        //Charakter-Treffer (Schaden) animieren
                        AudioUtil.playSoundEffect(this.gameObject, this.stats.hitSoundEffect);
                        SetCannotHit();
                    }
                }

                if (this.values.life > 0 && knockback)
                {
                    //Rückstoß ermitteln
                    float knockbackTrust = thrust - (this.stats.antiKnockback / 100 * thrust);
                    knockBack(duration, knockbackTrust, position);
                }
            }
        }

        [Button]
        public void KillIt() => this.KillCharacter(this); //KillIt(true);

        public void KillIt(bool showAnimation) => this.KillCharacter(this, showAnimation);//KillCharacter(showAnimation);

        public virtual void KillCharacter(bool animate)
        {
            for (int i = 0; i < this.values.activeSkills.Count; i++)
            {
                if (this.values.activeSkills[i].isAttachedToSender()) this.values.activeSkills[i].DeactivateIt();
            }

            RemoveAllStatusEffects();
            this.values.currentState = CharacterState.dead;

            if (this.myRigidbody != null && this.myRigidbody.bodyType != RigidbodyType2D.Static) this.myRigidbody.velocity = Vector2.zero;
            if (this.boxCollider != null) this.boxCollider.enabled = false;
            if (this.groundPosition != null) this.groundPosition.SetActive(false);

            //Play Death Effect
            if (animate)
            {
                if (this.stats.deathAnimation != null) PlayDeathAnimation();
                else AnimatorUtil.SetAnimatorParameter(this.animator, "Dead", true);
            }
            else DestroyItWithoutDrop();
        }

        public void DestroyIt()
        {
            DropItem();
            DestroyItWithoutDrop();
        }

        public void DestroyItWithoutDrop()
        {
            if (this.stats.hasRespawn) this.gameObject.SetActive(false);
            else PhotonNetwork.Destroy(this.gameObject);
        }

        #endregion

        #region Knockback and Invincibility   

        public void SetCannotHit() => SetCannotHit(this.stats.cannotBeHitTime, true);

        private void SetCannotHit(bool showHitColor) => SetCannotHit(this.stats.cannotBeHitTime, showHitColor);

        public void SetCannotHit(float delay, bool showHitcolor)
        {
            StopCoroutine(hitCo(delay, showHitcolor));
            StartCoroutine(hitCo(delay, showHitcolor));
        }

        public void SetInvincible(bool value)
        {
            this.values.isInvincible = value;
        }

        public void setCannotDie(bool value) => this.values.cannotDie = value;

        public void KnockBack(float knockTime, float thrust, Vector2 direction)
        {
            if (thrust != 0
                && this.myRigidbody != null
                && this.myRigidbody.bodyType != RigidbodyType2D.Kinematic
                && this.values.CanOpenMenu())
            {
                Vector2 difference = direction.normalized * thrust;
                //this.myRigidbody.velocity = Vector2.zero;
                //this.myRigidbody.AddForce(difference, ForceMode2D.Impulse);
                this.myRigidbody.DOMove(this.myRigidbody.position + difference, knockTime);
                StartCoroutine(knockCo(knockTime));
            }
        }

        public void knockBack(float knockTime, float thrust, Vector2 hitPosition)
        {
            if (this.myRigidbody != null)
            {
                Vector2 direction = (Vector2)this.myRigidbody.transform.position - hitPosition;
                KnockBack(knockTime, thrust, direction);
            }
        }

        #endregion

        #region Coroutines (Hit, Kill, Respawn, Knockback)

        private IEnumerator hitCo(float duration, bool showColor)
        {
            this.values.cantBeHit = true;
            if (this.stats.showHitcolor && showColor) this.ChangeColor(this.stats.hitColor);
            yield return new WaitForSeconds(duration);
            if (showColor) this.removeColor(this.stats.hitColor);
            this.values.cantBeHit = false;
        }

        private IEnumerator knockCo(float knockTime)
        {
            this.values.currentState = CharacterState.knockedback;
            yield return new WaitForSeconds(knockTime);

            //Rückstoß zurück setzten
            this.values.currentState = CharacterState.idle;
            this.myRigidbody.velocity = Vector2.zero;
        }

        #endregion

        #region Play

        public void PlaySoundEffect(AudioClip clip) => AudioUtil.playSoundEffect(this.gameObject, clip);

        public void PlayDeathAnimation()
        {
            if (this.activeDeathAnimation == null)
            {
                DeathAnimation deathObject = Instantiate(this.stats.deathAnimation, this.transform.position, Quaternion.identity);
                deathObject.setCharacter(this);
                this.activeDeathAnimation = deathObject;
            }
        }

        public void PlayCastingAnimation(bool value)
        {
            AnimatorUtil.SetAnimatorParameter(this.animator, "Casting", value);
        }

        public void PlayAnimation(string name) => AnimatorUtil.SetAnimatorParameter(this.animator, name);

        public void PlayAnimation(string name, bool value) => AnimatorUtil.SetAnimatorParameter(this.animator, name, value);

        #endregion

        #region Get and Set

        public Vector2 GetShootingPosition()
        {
            if (this.skillStartPosition != null) return this.skillStartPosition.transform.position;
            return this.transform.position;
        }

        public Vector2 GetHeadPosition()
        {
            if (this.headPosition != null) return this.headPosition.transform.position;
            else return GetShootingPosition();
        }

        public string GetCharacterName()
        {
            return this.characterName;
        }

        public Vector2 GetGroundPosition()
        {
            if (this.groundPosition != null) return this.groundPosition.transform.position;
            return this.transform.position;
        }

        public Vector2 GetSpawnPosition()
        {
            return this.spawnPosition;
        }

        public float GetSpeedFactor()
        {
            return this.values.speedFactor;
        }

        #endregion

        #region misc

        public virtual bool HasEnoughCurrency(Costs price)
        {
            return true; //Override by Player and used by Ability
        }

        public bool canUseIt(Costs price)
        {
            //Door, Shop, Treasure, Abilities
            if (this.values.CanMove() && HasEnoughCurrency(price)) return true;
            return false;
        }

        public virtual void EnableScripts(bool value)
        {
            if (this.GetComponent<AIAggroSystem>() != null) this.GetComponent<AIAggroSystem>().enabled = value;
            if (this.GetComponent<AICombat>() != null) this.GetComponent<AICombat>().enabled = value;
            if (this.GetComponent<AIMovement>() != null) this.GetComponent<AIMovement>().enabled = value;

            this.boxCollider.enabled = value;
        }

        public void startAttackAnimation(string parameter)
        {
            AnimatorUtil.SetAnimatorParameter(this.animator, parameter);
        }

        #endregion

        #region Respawn

        public void SetCharacterSprites(bool value)
        {
            this.isVisible = value;

            if (this.GetComponent<CharacterRenderingHandler>() != null)
                this.GetComponent<CharacterRenderingHandler>().enableSpriteRenderer(this.isVisible);

            if (this.groundPosition != null) this.groundPosition.SetActive(this.isVisible);
        }

        public virtual void SpawnOut()
        {
            this.myRigidbody.velocity = Vector2.zero;
            this.EnableScripts(false);
            this.values.currentState = CharacterState.respawning;
        }

        public void SpawnIn()
        {
            this.values.currentState = CharacterState.idle;
            this.EnableScripts(true);
        }

        public void PlayDespawnAnimation()
        {
            AnimatorUtil.SetAnimatorParameter(this.animator, "Despawn");
        }

        public float GetDespawnLength()
        {
            return AnimatorUtil.GetAnimationLength(this.animator, "Respawn");
        }

        public void PlayRespawnAnimation()
        {
            AnimatorUtil.SetAnimatorParameter(this.animator, "Respawn");
        }

        #endregion

        #region StatusEffect

        private void UpdateStatusEffectGroup(List<StatusEffect> effects)
        {
            effects.RemoveAll(item => item == null);
            for (int i = 0; i < effects.Count; i++) effects[i].Updating(this);
        }

        private void AddStatusEffectVisuals(List<StatusEffect> effects)
        {
            effects.RemoveAll(item => item == null);
            for (int i = 0; i < effects.Count; i++) AddStatusEffectVisuals(effects[i]);
        }

        private void AddStatusEffectVisuals(StatusEffect effect)
        {
            if (effect == null || effect.GetTarget() != this) return;
            if (effect.CanChangeColor()) ChangeColor(effect.GetColor());
            if (effect.CanInvertColor()) InvertColor(true);
            if (!ContainsEffect(effect)) this.statusEffectVisuals.Add(effect.Instantiate(this.activeStatusEffectParent));
        }

        private bool ContainsEffect(StatusEffect effect)
        {
            for (int i = 0; i < this.statusEffectVisuals.Count; i++)
            {
                if (this.statusEffectVisuals[i] != null && this.statusEffectVisuals[i].name == effect.GetVisuals().name) return true;
            }

            return false;
        }

        public void AddStatusEffectVisuals()
        {
            AddStatusEffectVisuals(this.values.buffs);
            AddStatusEffectVisuals(this.values.debuffs);
        }

        #endregion

        public void ShowMiniDialog(string text, float duration)
        {
            MiniDialogBox dialogBox = Instantiate(MasterManager.miniDialogBox, this.transform);
            dialogBox.setDialogBox(text, duration, GetHeadPosition());
        }

        public void RemoveAllStatusEffects()
        {
            StatusEffectUtil.RemoveAllStatusEffects(this.values.debuffs);
            StatusEffectUtil.RemoveAllStatusEffects(this.values.buffs);
        }



        #region Networking

        public void KillCharacter(Character target)
        {
            KillCharacter(target, true);
        }

        private void KillCharacter(Character target, bool value)
        {
            int targetID = target.gameObject.GetPhotonView().ViewID;
            this.photonView.RPC("RpcKillCharacterMaster", RpcTarget.MasterClient, targetID, value);
        }

        [PunRPC]
        protected void RpcKillCharacterMaster(int targetID, bool value, PhotonMessageInfo info)
        {
            this.photonView.RPC("RpcKillCharacter", RpcTarget.All, targetID, value);
        }

        [PunRPC]
        protected void RpcKillCharacter(int targetID, bool value, PhotonMessageInfo info)
        {
            Character target = PhotonView.Find(targetID).GetComponent<Character>();
            if (target != null) target.KillCharacter(value);
        }

        public virtual void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                stream.SendNext(this.values.direction);
                stream.SendNext(this.myRigidbody.velocity);

                stream.SendNext(this.values.life);
                stream.SendNext(this.values.mana);

                stream.SendNext(this.characterName);

                stream.SendNext(this.isVisible);
                stream.SendNext(this.values.cantBeHit);
                stream.SendNext(this.values.isInvincible);
                stream.SendNext(this.values.cannotDie);
            }
            else
            {
                this.values.direction = (Vector2)stream.ReceiveNext();
                this.myRigidbody.velocity = (Vector2)stream.ReceiveNext();

                this.values.life = (float)stream.ReceiveNext();
                this.values.mana = (float)stream.ReceiveNext();

                this.characterName = (string)stream.ReceiveNext();

                this.isVisible = (bool)stream.ReceiveNext();
                this.values.cantBeHit = (bool)stream.ReceiveNext();
                this.values.isInvincible = (bool)stream.ReceiveNext();
                this.values.cannotDie = (bool)stream.ReceiveNext();
            }
        }


        #endregion

    }
}
