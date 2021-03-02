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

        [BoxGroup("Pflichtfeld")]
        [SerializeField]
        private float delay = 1f;

        private float elapsed;
        private bool showEffectOnDisable = true;

        private float blinkDelay;
        private float blinkElapsed;
        private float blinkSpeed = 0.1f;

        private Rigidbody2D myRigidbody;
        private Vector2 direction;
        private bool canCollect = false;

        [AssetIcon]
        private Sprite GetSprite()
        {
            return this.GetComponent<SpriteRenderer>().sprite;
        }

        #region Start Funktionen

        public void SetItem(ItemDrop drop)
        {
            this.itemDrop = drop;            
        }        

        public void SetBounce(bool value, Vector2 direction)
        {
            this.showEffectOnEnable = value;
            this.direction = direction;
        }

        public ItemStats GetStats()
        {
            return this.itemDrop.stats;
        }

        public void SetSmoke(bool value) => this.showEffectOnDisable = value;

        private void Start()
        {
            this.myRigidbody = this.GetComponent<Rigidbody2D>();
            Bounce(true);

            if (this.itemDrop.HasKeyItem())
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

        public override void OnEnable()
        {
            base.OnEnable();
            Bounce();            
        }

        private void ChangeCollectState()
        {
            this.canCollect = true;
        }

        private void Bounce(bool onStart = false)
        {
            if (this.showEffectOnEnable && this.bounceAnimation != null && this.myRigidbody != null)
            {
                Invoke("ChangeCollectState", this.delay);
                this.bounceAnimation.Bounce();

                Vector2 targetPosition = (Vector2)this.transform.position + (this.direction * 1.5f);
                this.myRigidbody.DOMove(targetPosition, 1.75f);
            }
            else
            {
                if (onStart) ChangeCollectState();
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

        public override void OnDisable()
        {
            base.OnDisable();
            if (this.showEffectOnDisable) AnimatorUtil.ShowSmoke(this.transform);
        }

        #endregion

        public void playSounds()
        {
            AudioUtil.playSoundEffect(this.gameObject, this.itemDrop.stats.getSoundEffect());
        }

        #region Collect Item Funktionen

        private void OnTriggerEnter2D(Collider2D character) => CollectEnter(character);

        private void OnTriggerStay2D(Collider2D character) => CollectEnter(character);

        private void CollectEnter(Collider2D character)
        {
            if (!character.isTrigger && this.canCollect)
            {
                Player player = character.GetComponent<Player>();
                if (player != null && player.isLocalPlayer) CollectIt(player);
            }
        }

        private void CollectIt(Player player)
        {
            if (this.GetComponent<DialogSystem>() != null) this.GetComponent<DialogSystem>().showDialog(player, this);

            this.showEffectOnDisable = false;
            GameEvents.current.DoCollect(this.itemDrop);

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
