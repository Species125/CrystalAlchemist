using UnityEngine;
using Sirenix.OdinInspector;
using System.Collections;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class Player : Character
{
    [Required]
    [BoxGroup("Player Objects")]
    [SerializeField]
    private SimpleSignal presetSignal;

    [BoxGroup("Player Objects")]
    [SerializeField]
    private float goToBedDuration = 1f;

    [BoxGroup("Player Objects")]
    [SerializeField]
    private BoolValue CutSceneValue;

    [BoxGroup("Player Objects")]
    [SerializeField]
    private PlayerSaveGame saveGame;

    ///////////////////////////////////////////////////////////////

    public override void Awake()
    {
        this.stats = this.saveGame.stats;
        this.values = this.saveGame.playerValue;

        this.values.Initialize();    
        SetComponents();
        this.saveGame.attributes.SetValues();
    }

    public override void OnEnable()
    {
        if (this.values.life <= 0) ResetValues();        
    }

    public override void ResetValues()
    {
        base.ResetValues();
        this.saveGame.attributes.SetValues();
        this.values.life = this.values.maxLife;
        this.values.mana = this.values.maxMana;
    }

    public override void Start()
    {
        base.Start();
        this.presetSignal.Raise();

        if (this.hasAuthority) this.gameObject.name = "Player (Local)";
        else this.gameObject.name = "Player (Other)";

        if (!this.hasAuthority)
        {
            this.GetComponent<PlayerInput>().enabled = false;
            return;
        }

        SceneManager.LoadScene("UI", LoadSceneMode.Additive);

        GameEvents.current.OnCollect += this.CollectIt;
        GameEvents.current.OnReduce += this.reduceResource;
        GameEvents.current.OnStateChanged += this.SetState;
        GameEvents.current.OnSleep += this.GoToSleep;
        GameEvents.current.OnWakeUp += this.WakeUp;
        GameEvents.current.OnCutScene += this.SetCutScene;
        GameEvents.current.OnEnoughCurrency += this.HasEnoughCurrency;

        if( this.GetComponent<PlayerAbilities>() != null) this.GetComponent<PlayerAbilities>().Initialize();
        PlayerComponent[] components = this.GetComponents<PlayerComponent>();
        for (int i = 0; i < components.Length; i++) components[i].Initialize();

        GameEvents.current.DoManaLifeUpdate();  
        this.ChangeDirection(this.values.direction);

        this.animator.speed = 1;
        this.updateTimeDistortion(0);
        this.AddStatusEffectVisuals();

        GameEvents.current.DoStart(this.gameObject);
    }       

    public override void Update()
    {
        if (!this.hasAuthority) return;

        base.Update();        
        if(this.GetComponent<PlayerAbilities>() != null) this.GetComponent<PlayerAbilities>().Updating();
        PlayerComponent[] components = this.GetComponents<PlayerComponent>();
        for (int i = 0; i < components.Length; i++) components[i].Updating();
    }

    public override void OnDestroy()
    {
        if (!this.hasAuthority) return;

        base.OnDestroy();
        GameEvents.current.OnCollect -= this.CollectIt;
        GameEvents.current.OnReduce -= this.reduceResource;
        GameEvents.current.OnStateChanged -= this.SetState;
        GameEvents.current.OnSleep -= this.GoToSleep;
        GameEvents.current.OnWakeUp -= this.WakeUp;
        GameEvents.current.OnCutScene -= this.SetCutScene;
        GameEvents.current.OnEnoughCurrency -= this.HasEnoughCurrency;
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


    public override void reduceResource(Costs price)
    {
        //Shop, Door, Treasure, MiniGame, Abilities, etc
        if (price != null
            && ((price.item != null && !price.item.isKeyItem())
              || price.item == null))
            this.updateResource(price.resourceType, price.item, -price.amount);
    }

    ///////////////////////////////////////////////////////////////


    public override void updateResource(CostType type, ItemGroup item, float value, bool showingDamageNumber)
    {
        UpdateLifeMana(type, null, value, showingDamageNumber);

        switch (type)
        {
            case CostType.life: callSignal(value); break;
            case CostType.mana: callSignal(value); break;
            case CostType.item: this.GetComponent<PlayerItems>().UpdateInventory(item, Mathf.RoundToInt(value)); break;
        }
        CheckDeath();
    }

    private void SetCutScene()
    {
        if (this.CutSceneValue.GetValue()) this.values.currentState = CharacterState.respawning;
        else this.values.currentState = CharacterState.idle;
    }

    public void callSignal(float addResource)
    {
        if (addResource != 0) GameEvents.current.DoManaLifeUpdate();
    }


    public override void gotHit(Skill skill, float percentage, bool knockback)
    {
        GameEvents.current.DoCancel();
        base.gotHit(skill, percentage, knockback);
    }

    private void SetState(CharacterState state)
    {
        if (this.values.currentState == CharacterState.dead) return;
        this.values.currentState = state;
    } 

    private void CollectIt(ItemStats stats)
    {
        //Collectable, Load, MiniGame, Shop und Treasure

        if (stats.resourceType == CostType.life || stats.resourceType == CostType.mana) updateResource(stats.resourceType, stats.amount, true);
        else if (stats.resourceType == CostType.item) GetComponent<PlayerItems>().CollectInventoryItem(stats);
        else if (stats.resourceType == CostType.none)
        {
            //if(this.ability != null)
            foreach (StatusEffect effect in stats.statusEffects)
            {
                StatusEffectUtil.AddStatusEffect(effect, this);
            }
        }
    }

    public override string GetCharacterName()
    {
        return this.saveGame.characterName.GetValue();
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

        AnimatorUtil.SetAnimDirection(Vector2.down, this.animator);
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
}