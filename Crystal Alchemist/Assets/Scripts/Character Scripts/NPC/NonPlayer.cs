using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace CrystalAlchemist
{
    public class NonPlayer : Character
    {
        [Required]
        [BoxGroup("Pflichtfelder")]
        public CharacterStats NPCStats;

        [BoxGroup("Events")]
        [SerializeField]
        public UnityEvent onDefeated;

        public override void Awake()
        {
            this.stats = NPCStats;
            base.Awake();
        }

        public override void Start()
        {
            //GameEvents.current.OnKill += DestroyItWithoutDrop;
            if (GameManager.current.loadingCompleted)
            {
                SetCharacterSprites(false);
                SpawnOut();
            }            

            if (this.stats.showAnalyse)
            {
                AnalyseInfo analyse = Instantiate(MasterManager.analyseInfo, this.GetHeadPosition(), Quaternion.identity, this.transform);
                analyse.SetTarget(this.gameObject);
            }

            if (GameManager.current.loadingCompleted)
            {
                SetCharacterSprites(true);
                PlayRespawnAnimation();
                SpawnIn();
            }

            base.Start();
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
        }
        
        public override void DestroyItWithoutDrop()
        {
            this.onDefeated?.Invoke();
            base.DestroyItWithoutDrop();
        }
    }
}
