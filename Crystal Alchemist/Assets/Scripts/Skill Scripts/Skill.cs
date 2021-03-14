using Photon.Pun;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace CrystalAlchemist
{
    public enum SkillType
    {
        physical,
        magical,
        item
    }

    public enum StateType
    {
        none,
        attack,
        defend
    }

    public class Skill : NetworkBehaviour
    {
        #region Attribute

        [Space(10)]
        [BoxGroup("Easy Access")]
        public SpriteRenderer spriteRenderer;

        [BoxGroup("Easy Access")]
        public Rigidbody2D myRigidbody;

        [BoxGroup("Easy Access")]
        [InfoBox("Use Trigger 'Explode' when skill duration is over")]
        public Animator animator;

        [BoxGroup("Actions")]
        [SerializeField]
        public UnityEvent OnStart;

        [BoxGroup("Actions")]
        [SerializeField]
        public UnityEvent AfterDelay;


        [BoxGroup("Debug")]
        public Character sender;
        [BoxGroup("Debug")]
        public Character target;
        [BoxGroup("Debug")]
        [SerializeField]
        private Vector2 direction = Vector2.right;
        [BoxGroup("Debug")]
        public bool standAlone = true;
        [BoxGroup("Debug")]
        public bool isRapidFire = false;
        [BoxGroup("Debug")]
        public bool isAttached = false;
        [BoxGroup("Debug")]
        public bool canAffectedBytimeDistortion = false;

        ////////////////////////////////////////////////////////////////

        private float durationTimeLeft;
        private float delayTimeLeft;
        private float timeDistortion = 1;
        private bool triggerIsActive = true;
        private bool lockDirection;
        private float lockDuration;        
        private bool hasDelay;
        private float delay;
        private bool hasDuration;
        private float maxDuration;
        private bool attached;
        private float progress;
        private bool isActive = true;
        private float percentage;
        #endregion

        #region Start Funktionen (Init, set Basics, Update Sender, set Position

        public void SetDirectionLock(float duration)
        {
            this.lockDirection = true;
            this.lockDuration = duration;
        }

        public void InitializeStandAlone(Character sender, Character target, Quaternion rotation)
        {
            this.transform.rotation = rotation;
            this.sender = sender;
            this.target = target;
        }        

        public void SetMaxDuration(bool hasDuration, float maxDuration)
        {
            this.hasDuration = hasDuration;
            this.maxDuration = maxDuration;
        }

        public void SetDelay(bool hasDelay, float delay)
        {
            this.hasDelay = hasDelay;
            this.delay = delay;
        }

        public void SetStandAlone(bool value) => this.standAlone = value;

        private void Start()
        {
            this.isActive = true;
            //GameEvents.current.OnKill += DestroyIt;
            SetComponents();

            if (!this.standAlone)
            {
                SetVectors();
            }
            else
            {
                if (this.GetComponent<SkillRotationModule>() != null) this.GetComponent<SkillRotationModule>().Initialize();
                SetDirection(RotationUtil.DegreeToVector2(this.transform.rotation.eulerAngles.z));
            }

            SkillModule[] modules = this.GetComponents<SkillModule>();
            for (int i = 0; i < modules.Length; i++) modules[i].Initialize();

            SkillExtension[] extensions = this.GetComponents<SkillExtension>();
            for (int i = 0; i < extensions.Length; i++) extensions[i].Initialize();

            if (this.lockDirection && this.sender.GetComponent<PlayerMovement>()!= null)
                this.sender.GetComponent<PlayerMovement>().SetDirectionLock(this.lockDuration);
            this.OnStart?.Invoke();
        }

        //private void OnDestroy() => GameEvents.current.OnKill -= DestroyIt;

        private void SetComponents()
        {
            if (this.myRigidbody == null) this.myRigidbody = GetComponent<Rigidbody2D>();
            if (this.spriteRenderer == null) this.spriteRenderer = GetComponent<SpriteRenderer>();
            if (this.animator == null) this.animator = GetComponent<Animator>();

            this.durationTimeLeft = this.maxDuration;
            this.delayTimeLeft = this.delay;
        }

        public void SetVectors()
        {
            this.transform.position = this.sender.GetShootingPosition();
            if (this.GetComponent<SkillPositionModule>() != null) this.GetComponent<SkillPositionModule>().Initialize();

            SetDirection(RotationUtil.SetStartDirection(this));

            if (this.GetComponent<SkillRotationModule>() != null) this.GetComponent<SkillRotationModule>().Initialize();            

            if (this.GetComponent<SkillBlendTreeModule>() != null) this.GetComponent<SkillBlendTreeModule>().Initialize();

            if (this.GetComponent<SkillPositionModule>() != null) this.GetComponent<SkillPositionModule>().LateInitialize();
        }

        public Vector2 GetDirection()
        {
            return this.direction.normalized;
        }


        public void SetDirection(Vector2 direction)
        {
            this.direction = direction.normalized;
        }

        #endregion


        #region Update Funktionen   

        public void Update()
        {
            if (!this.isActive)
            {
                DestroyInactiveSkill();
                return;
            }

            if (this.animator != null && !this.lockDirection)
                AnimatorUtil.SetAnimDirection(GetDirection(), this.animator);

            AnimatorUtil.SetAnimatorParameter(this.animator, "Active", true);

            if (this.hasDelay)
            {
                if (this.delayTimeLeft > 0)
                {
                    this.delayTimeLeft -= (Time.deltaTime * this.timeDistortion);
                    this.progress = 1 - (this.delayTimeLeft / this.delay);
                }
                else
                {
                    this.AfterDelay?.Invoke();
                    this.hasDelay = false;
                }
            }

            //Prüfe ob der Skill eine Maximale Dauer hat
            if (this.hasDuration && !this.hasDelay)
            {
                if (this.durationTimeLeft > 0) this.durationTimeLeft -= (Time.deltaTime * this.timeDistortion);
                else this.DeactivateIt();
            }

            if (this.target != null && !this.target.gameObject.activeInHierarchy) this.DeactivateIt();

            SkillModule[] modules = this.GetComponents<SkillModule>();
            for (int i = 0; i < modules.Length; i++) modules[i].Updating();

            SkillExtension[] extensions = this.GetComponents<SkillExtension>();
            for (int i = 0; i < extensions.Length; i++) extensions[i].Updating();

            if (this.lockDirection && !this.isRapidFire && this.sender.GetComponent<PlayerMovement>() != null)
                this.sender.GetComponent<PlayerMovement>().SetDirectionLock(this.lockDuration);
        }

        public float GetDurationLeft()
        {
            return this.durationTimeLeft;
        }

        public float GetProgress()
        {
            return this.progress;
        }

        public void hitIt(Collider2D hittedObject)
        {
            if (hittedObject.GetComponent<Character>() != null)
            {
                hitIt(hittedObject.GetComponent<Character>());
            }
        }

        public void hitIt(Character target)
        {
            //Gegner zurückstoßen + Hit
            target.GotHit(this);
        }

        public void hitIt(Collider2D hittedObject, float percentage)
        {
            if (hittedObject.GetComponent<Character>() != null)
            {
                hitIt(hittedObject.GetComponent<Character>(), percentage);
            }
        }

        public void hitIt(Character target, float percentage)
        {
            //Gegner zurückstoßen + Hit
            target.GotHit(this, percentage);
        }

        #endregion

        public bool IsAttachedToSender()
        {
            return this.attached;
        }

        public bool isDirectionLocked()
        {
            return this.lockDirection;
        }

        #region AnimatorEvents

        public void PlayAnimation(string trigger)
        {
            AnimatorUtil.SetAnimatorParameter(this.animator, trigger);
        }

        public void SetTriggerActive(int value)
        {
            Debug.Log(this.gameObject.name);
            if (value == 0) this.triggerIsActive = false;
            else this.triggerIsActive = true;
        }

        public bool GetTriggerActive()
        {
            return this.triggerIsActive;
        }

        public void resetRotation()
        {
            this.transform.rotation = Quaternion.Euler(0, 0, 0);
        }

        public void DeactivateIt()
        {
            if (this.animator == null || !AnimatorUtil.HasParameter(this.animator, "Explode"))
            {
                SetTriggerActive(1);
                DestroyIt();
            }
            else AnimatorUtil.SetAnimatorParameter(this.animator, "Explode");
        }

        public void PlaySoundEffect(AudioClip audioClip) => AudioUtil.playSoundEffect(this.gameObject, audioClip);

        public void DestroyIt() => this.isActive = false;        

        public void DestroyInactiveSkill()
        {
            if (this.sender != null) this.sender.values.activeSkills.Remove(this);
            Destroy(this.gameObject);
        }


        #endregion


        #region Update Extern

        public void updateTimeDistortion(float distortion) //Signal?
        {
            if (this.canAffectedBytimeDistortion)
            {
                this.timeDistortion = 1 + (distortion / 100);

                if (this.animator != null) this.animator.speed = this.timeDistortion;
                if (this.triggerIsActive && this.GetComponent<SkillProjectile>() != null) this.GetComponent<SkillProjectile>().setVelocity();
            }
        }

        public float getTimeDistortion()
        {
            return this.timeDistortion;
        }

        public void SetPercentage(float value) => this.percentage = value;

        #endregion

    }
}