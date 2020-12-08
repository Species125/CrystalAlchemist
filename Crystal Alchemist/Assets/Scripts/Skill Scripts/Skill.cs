using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.Events;

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

public class Skill : MonoBehaviour
{
    #region Attribute

    [Space(10)]
    [BoxGroup("Easy Access")]
    public SpriteRenderer spriteRenderer;

    [BoxGroup("Easy Access")]
    public Rigidbody2D myRigidbody;

    [BoxGroup("Easy Access")]
    public Animator animator;

    [BoxGroup("Easy Access")]
    public SkillCollider skillCollider;

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

    ////////////////////////////////////////////////////////////////

    private float durationTimeLeft;
    private float delayTimeLeft;
    private float timeDistortion = 1;
    private bool triggerIsActive = true;
    private float positionOffset;
    private bool lockDirection;
    private bool canAffectedBytimeDistortion;
    private bool hasDelay;
    private float delay;
    private bool hasDuration;
    private float maxDuration;
    private bool isRapidFire;
    private bool attached;
    private float progress;
    private float percentage;

    #endregion

    [Button]
    private void AddCharacters()
    {
        this.sender = FindObjectOfType<AI>();
        this.target = FindObjectOfType<Player>();
        this.gameObject.SetActive(false);
    }


    [Button]
    private void OverrideDelay()
    {
        this.AfterDelay?.Invoke();
    }

    #region Start Funktionen (Init, set Basics, Update Sender, set Position

    public void InitializeStandAlone(Character sender, Character target, Quaternion rotation)
    {
        this.transform.rotation = rotation;
        this.sender = sender;
        this.target = target;
    }

    public void Initialize(float offset, bool lockDirection, bool isRapidFire, bool affectTimeDistortion, bool attached)
    {
        this.positionOffset = offset;
        this.lockDirection = lockDirection;
        this.isRapidFire = isRapidFire;
        this.canAffectedBytimeDistortion = affectTimeDistortion;
        this.attached = attached;
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

        if (this.lockDirection) GameEvents.current.DoDirectionLock();
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
        if (this.GetComponent<SkillPositionZModule>() != null) this.GetComponent<SkillPositionZModule>().Initialize();

        SetDirection(RotationUtil.SetStartDirection(this));

        if (this.GetComponent<SkillRotationModule>() != null) this.GetComponent<SkillRotationModule>().Initialize();
        this.transform.position += ((Vector3)GetDirection() * this.positionOffset);

        if (this.GetComponent<SkillBlendTreeModule>() != null) this.GetComponent<SkillBlendTreeModule>().Initialize();
    }

    public Vector2 GetDirection()
    {
        return this.direction.normalized;
    }

    public Vector2 GetPosition()
    {
        if (this.skillCollider == null) return this.transform.position;
        else return this.skillCollider.GetPosition();
    }

    public void SetDirection(Vector2 direction)
    {
        this.direction = direction.normalized;
    }

    #endregion


    #region Update Funktionen   

    public void Update()
    {
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

        if (this.lockDirection && !this.isRapidFire) GameEvents.current.DoDirectionLock();
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
        target.gotHit(this);
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
        target.gotHit(this, percentage);
    }

    #endregion

    public bool isAttachedToSender()
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
        else AnimatorUtil.SetAnimatorParameter(this.animator, "Explode", true);
    }

    public void PlaySoundEffect(AudioClip audioClip) => AudioUtil.playSoundEffect(this.gameObject, audioClip);

    public void DestroyIt()
    {
        DestroyIt(0f);
    }

    public void DestroyIt(float delay)
    {
        if(this.sender != null) this.sender.values.activeSkills.Remove(this);
        Destroy(this.gameObject, delay);
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

