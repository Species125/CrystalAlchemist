using AssetIcons;
using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;

namespace CrystalAlchemist
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Collectable : NetworkBehaviour
    {
        [Required]
        [BoxGroup("Pflichtfeld")]
        [SerializeField]
        private SpriteRenderer shadowRenderer;

        [BoxGroup("Pflichtfeld")]
        [SerializeField]
        [Tooltip("Used for blinking")]
        private SpriteRenderer itemSprite;

        [Required]
        [BoxGroup("Pflichtfeld")]
        [SerializeField]
        private ItemDrop itemDrop;

        [Required]
        [BoxGroup("Pflichtfeld")]
        [SerializeField]
        [Tooltip("Use it for manuel effect")]
        private bool showEffectOnEnable = false;

        [Required]
        [BoxGroup("Pflichtfeld")]
        [SerializeField]
        private BounceAnimation bounceAnimation;

        [BoxGroup("Time")]
        [SerializeField]
        [Tooltip("Destroy Gameobject x seconds after spawn")]
        private bool hasSelfDestruction = false;

        [BoxGroup("Time")]
        [SerializeField]
        [ShowIf("hasSelfDestruction")]
        private float duration = 60f;

        private ItemStats itemStats;
        private float elapsed;
        private bool showEffectOnDisable = true;

        private float blinkDelay;
        private float blinkElapsed;
        private float blinkSpeed = 0.1f;

        private Rigidbody2D myRigidbody;
        private bool canMoveBounce = false;
        private Vector2 direction;

        [AssetIcon]
        private Sprite GetSprite()
        {
            return this.GetComponent<SpriteRenderer>().sprite;
        }

        #region Start Funktionen

        public void SetItem(ItemDrop drop)
        {
            this.itemDrop = drop;
            setItemStats();
        }

        private void setItemStats()
        {
            ItemStats temp = Instantiate(this.itemDrop.stats);
            temp.name = this.itemDrop.name;
            this.itemStats = temp;
        }

        public void SetBounce(bool value, Vector2 direction)
        {
            this.showEffectOnEnable = value;
            this.direction = direction;
        }

        public ItemStats GetStats()
        {
            return this.itemStats;
        }

        public void SetSmoke(bool value) => this.showEffectOnDisable = value;

        private void Start()
        {
            this.myRigidbody = this.GetComponent<Rigidbody2D>();
            Bounce();
            setItemStats();

            string itemName = this.itemDrop.name;

            if (this.itemDrop.progress.ContainsProgress() ||
                (this.itemStats.isKeyItem() && GameEvents.current.HasKeyItem(itemName)))
            {
                this.showEffectOnDisable = false;
                DestroyIt();
            }

            if (this.hasSelfDestruction) this.elapsed = this.duration;
        }

        private void Update()
        {
            if (!this.hasSelfDestruction) return;

            if (this.elapsed > 0) this.elapsed -= Time.deltaTime;
            else DestroyIt();

            BlinkAnimation();
        }

        private void BlinkAnimation()
        {
            if (this.elapsed < 3) this.blinkDelay = 0.25f;
            else if (this.elapsed < 6) this.blinkDelay = 0.5f;
            else if (this.elapsed <= 10) this.blinkDelay = 1f;

            if (this.blinkDelay > 0 && this.itemSprite != null)
            {
                if (this.blinkElapsed > 0) this.blinkElapsed -= Time.deltaTime;
                else
                {
                    this.blinkElapsed = this.blinkDelay;
                    StartCoroutine(BlinkCo(blinkSpeed));
                }
            }
        }

        private IEnumerator BlinkCo(float delay)
        {
            this.itemSprite.DOColor(new Color(0, 0, 0, 0), delay);
            yield return new WaitForSeconds(delay);
            this.itemSprite.DOColor(Color.white, delay);
        }

        private void OnEnable() => Bounce();

        private void Bounce()
        {
            if (this.showEffectOnEnable && this.bounceAnimation != null)
            {
                this.bounceAnimation.Bounce();

                Vector2 targetPosition = (Vector2)this.transform.position + (this.direction * 1.5f);
                this.GetComponent<Rigidbody2D>().DOMove(targetPosition, 1.75f);
            }
        }

        [Button]
        public void SetSelfDestruction(float duration)
        {
            SetSelfDestruction(duration, true);
        }

        public void SetSelfDestruction(float duration, bool hasSelfDestruction)
        {
            this.duration = duration;
            this.hasSelfDestruction = hasSelfDestruction;
        }

        private void OnDisable()
        {
            if (this.showEffectOnDisable) AnimatorUtil.ShowSmoke(this.transform);
        }

        #endregion

        public void playSounds()
        {
            AudioUtil.playSoundEffect(this.gameObject, this.itemStats.getSoundEffect());
        }

        #region Collect Item Funktionen

        public void OnTriggerEnter2D(Collider2D character)
        {
            if (!character.isTrigger) 
            {
                Player player = character.GetComponent<Player>();
                if (player != null && player.isLocalPlayer) CollectIt(player);
            }
        }

        private void CollectIt(Player player)
        {
            if (this.GetComponent<DialogSystem>() != null) this.GetComponent<DialogSystem>().showDialog(player, this);

            this.showEffectOnDisable = false;
            GameEvents.current.DoCollect(this.itemStats);
            this.itemDrop.progress.AddProgress();

            playSounds();
            DestroyIt();
            Instantiate(MasterManager.itemCollectGlitter, this.transform.position, Quaternion.identity);
        }

        public void DestroyIt()
        {
            Destroy(this.gameObject);
        }
        #endregion
    }
}
