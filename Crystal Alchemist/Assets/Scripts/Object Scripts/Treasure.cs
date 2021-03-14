using Sirenix.OdinInspector;
using UnityEngine;
using Photon.Pun;
using System.Collections.Generic;

namespace CrystalAlchemist
{
    public enum TreasureType
    {
        normal,
        lootbox
    }

    public enum BounceDirection
    {
        player,
        up,
        down,
        left,
        right,
        position
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

        [BoxGroup("Treasure Options")]
        [SerializeField]
        private BounceDirection bounceDirection = BounceDirection.player;

        [BoxGroup("Treasure Options")]
        [SerializeField]
        private Vector2 itemSpawnPosition;

        [BoxGroup("Treasure Options")]
        [SerializeField]
        [Tooltip("Spawn Items for all players?")]
        private bool isShared = false;       

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

        private Vector2 playerPosition;
        #endregion

        private bool canLoot = true;

        #region Start Funktionen

        public override void Start()
        {
            base.Start();
            this.SetLoot();
            this.shopPrice.Initialize(this.costs);

            ClearTreasure();

            AnalyseInfo analyse = Instantiate(MasterManager.analyseInfo, this.transform.position, Quaternion.identity, this.transform);
            analyse.SetTarget(this.gameObject);
        }

        

        private void ClearTreasure()
        {
            if (this.itemDrops.Count <= 0
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

            if (this.player.CanUseInteraction(this.costs))
            {
                this.playerPosition = this.player.GetGroundPosition();
                this.player.ReduceResource(this.costs);
                AnimatorUtil.SetAnimatorParameter(this.anim, "isOpened", true);
                if (isShared) SharedSubmit();
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

        public override bool PlayerCanInteract()
        {
            return base.PlayerCanInteract() && this.canLoot;
        }

        public void PlayTreasureSoundEffect()
        {
            if (this.soundEffect != null) AudioUtil.playSoundEffect(this.gameObject, this.soundEffect);
        }

        public void PlayTreasureMusic()
        {
            if (this.treasureMusic != null && this.itemDrops.Count > 0)
                MusicEvents.current.PlayMusicAndResume(this.treasureMusic, true, this.fadeOld, this.fadeNew);
        }

        public void ShowTreasureItem()
        {
            if (this.itemDrops.Count > 0)
            {
                Vector2 position = this.transform.position;
                Vector2 direction = position - this.playerPosition;

                switch (this.bounceDirection)
                {
                    case BounceDirection.down: direction = Vector2.down; break;
                    case BounceDirection.up: direction = Vector2.up; break;
                    case BounceDirection.left: direction = Vector2.left; break;
                    case BounceDirection.right: direction = Vector2.right; break;
                    case BounceDirection.position: direction = Vector2.zero; position = this.itemSpawnPosition; break;
                }

                foreach(ItemDrop drop in this.itemDrops)
                {
                    NetworkEvents.current.InstantiateTreasureItem(drop, position, true, direction);
                }
            }

            if (this.treasureType == TreasureType.lootbox)
            {
                AnimatorUtil.SetAnimatorParameter(this.anim, "isOpened", false);
                this.SetLoot();
            }
        }

        private void SharedSubmit()
        {
            List<string> paths = new List<string>();
            foreach(ItemDrop drop in this.itemDrops) paths.Add(drop.path);            

            this.photonView.RPC("RpcSetAnimation", RpcTarget.Others, paths.ToArray());
        }

        [PunRPC]
        protected void RpcSetAnimation(string[] paths)
        {
            this.itemDrops.Clear();
            foreach(string path in paths) this.itemDrops.Add(Resources.Load<ItemDrop>(path));

            AnimatorUtil.SetAnimatorParameter(this.anim, "isOpened", true);
        }

        #endregion
    }
}