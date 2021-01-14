







using Sirenix.OdinInspector;
using UnityEngine;

namespace CrystalAlchemist
{
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

        private bool canLoot = true;

        #region Start Funktionen

        public override void Start()
        {
            base.Start();
            this.setLoot();
            this.shopPrice.Initialize(this.costs);

            ClearTreasure();

            AnalyseInfo analyse = Instantiate(MasterManager.analyseInfo, this.transform.position, Quaternion.identity, this.transform);
            analyse.SetTarget(this.gameObject);
        }

        private void setLoot()
        {
            if (this.useLootTable) this.itemDrop = this.lootTable.GetItemDrop();
            else this.itemDrop = this.reward.GetItemDrop();
        }

        private void ClearTreasure()
        {
            if (this.itemDrop == null
                && this.treasureType == TreasureType.normal)
            {
                SetEnabled(false);
                AnimatorUtil.SetAnimatorParameter(this.anim, "Empty");
            }
        }

        #endregion


        #region Update Funktion

        public override void DoOnSubmit()
        {
            if (!canLoot) return;

            if (this.player.canUseIt(this.costs))
            {
                this.player.reduceResource(this.costs);
                AnimatorUtil.SetAnimatorParameter(this.anim, "isOpened", true);
            }
            else ShowDialog(DialogTextTrigger.failed);
        }

        #endregion


        #region Treasure Chest Funktionen (open, show Item)


        public void SetEnabled(bool enable)
        {
            //Animator Events
            this.canLoot = enable;

            if (PlayerCanInteract() && enable) ShowContextClue(true);
            else ShowContextClue(false);
        }

        public void PlayTreasureSoundEffect()
        {
            if (this.soundEffect != null) AudioUtil.playSoundEffect(this.gameObject, this.soundEffect);
        }

        public void PlayTreasureMusic()
        {
            if (this.treasureMusic != null && this.itemDrop != null)
                MusicEvents.current.PlayMusicAndResume(this.treasureMusic, true, this.fadeOld, this.fadeNew);
        }

        public void ShowTreasureItem()
        {
            Debug.Log(this.itemDrop);

            if (this.itemDrop != null)
            {
                NetworkEvents.current.InstantiateTreasureItem(this.itemDrop, this.transform.position, true, this.player.GetGroundPosition());
            }
            if (this.treasureType == TreasureType.lootbox)
            {
                AnimatorUtil.SetAnimatorParameter(this.anim, "isOpened", false);
                this.setLoot();
            }
        }

        #endregion
    }
}