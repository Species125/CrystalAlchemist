using Sirenix.OdinInspector;
using UnityEngine;
using Photon.Pun;
using System.Collections.Generic;
using System.Collections;

namespace CrystalAlchemist
{
    public enum BounceDirection
    {
        awayFromPlayer,
        up,
        down,
        left,
        right,
        position,
        area,
        toPlayer
    }

    public class Treasure : Rewardable
    {
        #region Attribute   

        [BoxGroup("Treasure Options")]
        [SerializeField]
        private Animator anim;

        [BoxGroup("Treasure Options")]
        [SerializeField]
        private AudioClip treasureMusic;

        [BoxGroup("Treasure Options")]
        [SerializeField]
        private BounceDirection bounceDirection = BounceDirection.awayFromPlayer;

        [BoxGroup("Treasure Options")]
        [ShowIf("bounceDirection", BounceDirection.area)]
        [SerializeField]
        private Collider2D spawnArea;

        [BoxGroup("Treasure Options")]
        [SerializeField]
        [ShowIf("bounceDirection", BounceDirection.position)]
        private Vector2 itemSpawnPosition;

        [BoxGroup("Treasure Options")]
        [SerializeField]
        [Tooltip("Spawn Items for all players?")]
        private bool isShared = false;

        [BoxGroup("Treasure Options")]
        [SerializeField]
        private bool isLootbox = false;

        [BoxGroup("Treasure Options")]
        [SerializeField]
        private bool canReset = true;

        [BoxGroup("Treasure Options")]
        [SerializeField]
        [MinValue(0)]
        [ShowIf("canReset")]
        private float resetInterval = 30f;

        [SerializeField]
        [BoxGroup("Mandatory")]
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

        private bool resetIsRunning = false;
        private Vector2 playerPosition;
        #endregion

        private bool canLoot = true;

        #region Start Funktionen

        public override void Start()
        {
            base.Start();
            
            if(this.shopPrice != null) this.shopPrice.Initialize(this.costs);

            InitTreasure();
        }

        private void InitTreasure()
        {
            this.SetLoot();

            if (this.itemDrops.Count <= 0 && !this.isLootbox)
            {
                SetEnabled(false);
                AnimatorUtil.SetAnimatorParameter(this.anim, "Empty");
            }

            AnalyseInfo analyse = Instantiate(MasterManager.analyseInfo, this.transform.position, Quaternion.identity, this.transform);
            analyse.SetTarget(this.gameObject);
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

                OpenIt();

                if (isShared) SharedSubmit();
            }
            else ShowDialog(DialogTextTrigger.failed);
        }

        private void OpenIt()
        {
            if (this.anim != null) AnimatorUtil.SetAnimatorParameter(this.anim, "isOpened", true);
            else
            {
                SetEnabled(false);
                ShowTreasureItem();                
            }
        }

        private void CloseIt()
        {
            if (this.anim != null) AnimatorUtil.SetAnimatorParameter(this.anim, "isOpened", false);
        }

        #endregion


        #region Treasure Chest Funktionen (open, show Item)


        public virtual void SetEnabled(bool enable)
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
                Vector2 direction = Vector2.zero;

                switch (this.bounceDirection)
                {
                    case BounceDirection.awayFromPlayer: direction = position - this.playerPosition; break;
                    case BounceDirection.toPlayer: direction = this.playerPosition-position; break;
                    case BounceDirection.down: direction = Vector2.down; break;
                    case BounceDirection.up: direction = Vector2.up; break;
                    case BounceDirection.left: direction = Vector2.left; break;
                    case BounceDirection.right: direction = Vector2.right; break;
                    case BounceDirection.position: direction = Vector2.zero; position = this.itemSpawnPosition; break;
                    case BounceDirection.area: direction = Vector2.zero; break;
                }

                foreach(ItemDrop drop in this.itemDrops)
                {
                    if(this.bounceDirection == BounceDirection.area) 
                        position = UnityUtil.GetRandomVector(this.spawnArea);                   

                    NetworkEvents.current.InstantiateTreasureItem(drop, position, direction);
                }
            }

            if (this.canReset && !this.resetIsRunning) StartCoroutine(ResetLootCo());
        }

        private IEnumerator ResetLootCo()
        {
            this.resetIsRunning = true;
            yield return new WaitForSeconds(this.resetInterval);
            CloseIt();
            this.SetLoot();

            this.resetIsRunning = false;
        }

        private void SharedSubmit()
        {
            List<string> paths = new List<string>();
            foreach(ItemDrop drop in this.itemDrops) paths.Add(drop.path);            

            if(this.photonView != null) this.photonView.RPC("RpcSetAnimation", RpcTarget.Others, paths.ToArray());
        }

        [PunRPC]
        protected void RpcSetAnimation(string[] paths)
        {
            this.itemDrops.Clear();
            foreach(string path in paths) this.itemDrops.Add(Resources.Load<ItemDrop>(path));

            OpenIt();
        }

        #endregion
    }
}