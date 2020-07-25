using UnityEngine;
using Sirenix.OdinInspector;

public enum TreasureType
{
    normal,
    lootbox
}

public class Treasure : Rewardable
{
    #region Attribute   

    [BoxGroup("Treasure Options")]
    [Required]
    [SerializeField]
    private Animator anim;

    [BoxGroup("Mandatory")]
    [Required]
    [SerializeField]
    private GameObject showItem;

    [BoxGroup("Treasure Options")]
    [SerializeField]
    private AudioClip treasureMusic;

    [BoxGroup("Treasure Options")]
    [SerializeField]
    private TreasureType treasureType = TreasureType.normal;

    [BoxGroup("Loot")]
    [SerializeField]
    private bool useLootTable = false;

    [BoxGroup("Loot")]
    [HideIf("useLootTable")]
    [SerializeField]
    [HideLabel]
    private Reward reward;

    [BoxGroup("Loot")]
    [ShowIf("useLootTable")]
    [SerializeField]
    private LootTable lootTable;

    [SerializeField]
    [BoxGroup("Mandatory")]
    [Required]
    private ShopPrice shopPrice;

    [BoxGroup("Sound")]
    [Tooltip("Standard-Soundeffekt")]
    public AudioClip soundEffect;

    [BoxGroup("Sound")]
    [SerializeField]
    private float fadeOld = 0.5f;

    [BoxGroup("Sound")]
    [SerializeField]
    private float fadeNew = 0.5f;

    #endregion

    private bool treasureEnabled = true;

    #region Start Funktionen

    public override void Start()
    {
        base.Start();
        this.setLoot();
        this.shopPrice.Initialize(this.costs);

        if (this.itemDrop == null 
        && this.treasureType == TreasureType.normal) ChangeTreasureState(true);

        AnalyseInfo analyse = Instantiate(MasterManager.analyseInfo, this.transform.position, Quaternion.identity, this.transform);
        analyse.SetTarget(this.gameObject);       
    }

    private void setLoot()
    {
        if (this.useLootTable) this.itemDrop = this.lootTable.GetItemDrop();
        else this.itemDrop = this.reward.GetItemDrop();
    }

    #endregion


    #region Update Funktion

    public override void DoOnUpdate()
    {
        if (!this.treasureEnabled
            && ((this.player != null && this.player.values.currentState == CharacterState.interact)
              || this.player == null))
        {
            closeChest(); //close Chest
        }
    }

    public override void DoOnSubmit()
    {
        if (this.treasureEnabled)
        {
            if (this.player.canUseIt(this.costs)) openChest(); //open Chest
            else ShowDialog(DialogTextTrigger.failed);
        }
    }

    #endregion


    #region Treasure Chest Funktionen (open, show Item)

    private void openChest()
    {
        this.player.reduceResource(this.costs);
        ChangeTreasureState(true);

        if (this.soundEffect != null && this.itemDrop != null)
        {
            //Zeige Item
            this.showTreasureItem();
            ShowDialog(DialogTextTrigger.success, this.itemDrop.stats);
        }
        else
        {
            //Kein Item drin
            ShowDialog(DialogTextTrigger.empty);
        }
    }

    private void closeChest()
    {
        //Close when opened
        Destroy(this.itemDrop);        

        if (this.treasureType == TreasureType.lootbox)
        {
            AnimatorUtil.SetAnimatorParameter(this.anim, "isOpened", false);
            this.setLoot();
        }

        //Verstecke gezeigtes Item wieder
        for (int i = 0; i < this.showItem.transform.childCount; i++)
        {
            Destroy(this.showItem.transform.GetChild(i).gameObject);
        }

        this.showItem.SetActive(false);
    }

    private void ChangeTreasureState(bool openChest)
    {
        AudioUtil.playSoundEffect(this.gameObject, this.soundEffect);
        AnimatorUtil.SetAnimatorParameter(this.anim, "isOpened", openChest);       
    }

    public void SetEnabled(bool enable)
    {
        //Animator Events
        this.treasureEnabled = enable;

        if (PlayerCanInteract() && enable) ShowContextClue(true);
        else ShowContextClue(false);
    }

    public void showTreasureItem()
    {
        if(this.treasureMusic != null) MusicEvents.current.PlayMusicAndResume(this.treasureMusic, true, this.fadeOld, this.fadeNew);

        //Item instanziieren und der Liste zurück geben und das Item anzeigen            
        this.showItem.SetActive(true);

        Collectable collectable = this.itemDrop.Instantiate(this.showItem.transform.position);
        collectable.SetAsTreasureItem(this.showItem.transform);

        GameEvents.current.DoCollect(this.itemDrop.stats);
    }

    #endregion
}