using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public abstract class Entity : MonoBehaviour, IAffectable, IAnimationTriggerable
{
    #region ComponentRegion
    [field:SerializeField] public EntityStatSO Stat { get; private set; }
    [SerializeField] private EntitySkillSO entitySkillSO;
    public EntitySkillSO EntitySkillSO => entitySkillSO;

    public EntityVisual visualCompo { get; protected set; }
    public Animator animatorCompo { get; protected set; }
    public SpriteRenderer spriteRendererCompo { get; protected set; }
    public Collider2D colliderCompo { get; protected set; }
    public Rigidbody2D rigidbodyCompo { get; protected set; }
    public EntityMovement MovementCompo { get; protected set; }

    public EntityAttack entityAttack { get; protected set; }

    public Health healthCompo { get; protected set; }
    #endregion

    [Header("Collision info")]
    [SerializeField] protected LayerMask whatIsGroundAndWall;
    [SerializeField] protected LayerMask whatIsProbs;
    [Space(10)]
    [SerializeField] protected Transform groundChecker;
    [SerializeField] protected float groundCheckBoxWidth;
    [SerializeField] protected float groundCheckDistance;
    [Space(10)]
    [SerializeField] protected Transform wallChecker;
    [SerializeField] protected float wallCheckDistance;
    [SerializeField] protected float wallCheckBoxHeight;

    public float moveSpeed => Stat.moveSpeed.GetValue();
    protected float knockbackDuration = 0.5f;
    protected Coroutine knockbackCoroutine;
    public bool isKnockbacked { get; protected set; }

    public float stunDuration { get; protected set; }
    public bool canBeStun { get; protected set; }
    public float airBornDuration { get; protected set; }
    public bool canBeAirBorn { get; protected set; }
    public float upArmorDuration { get; protected set; }

    protected int _statusEffectBit = 0;

    [Space]
    [Header("FeedBack info")]
    public UnityEvent HitEvent;

    public Action<int> OnFlip;
    public int FacingDir { get; protected set; } = 1;
    public bool CanStateChangeable { get; set; } = true;
    public bool IsDead { get; protected set; } = false;
    public bool CanKnockback { get; set; } = true;

    private int _animationTriggerBit = 0;
    private StatusEffectManager _statusEffectManager;
    public StatusEffectManager StatusEffectManager => _statusEffectManager;

    private float _xMovement;

    protected virtual void Awake()
    {
        Transform visualTrm = transform.Find("Visual");
        visualCompo = visualTrm.GetComponent<EntityVisual>();
        animatorCompo = visualTrm.GetComponent<Animator>();
        spriteRendererCompo = visualTrm.GetComponent<SpriteRenderer>();
        rigidbodyCompo = GetComponent<Rigidbody2D>();
        colliderCompo = GetComponent<Collider2D>();
        healthCompo = GetComponent<Health>();
        entityAttack = GetComponent<EntityAttack>();
        MovementCompo = GetComponent<EntityMovement>();
        MovementCompo.Initialize(this);

        Stat = Instantiate(Stat);
        entitySkillSO = ScriptableObject.Instantiate(entitySkillSO);

        _statusEffectManager = new StatusEffectManager(this);
    }

    private void FixedUpdate()
    {
        //rigidbodyCompo.velocity = new Vector2(_xMovement, rigidbodyCompo.velocity.y);
    }

    #region Velocity Section

    /// <summary>
    /// Set velocity zero this Entity
    /// </summary>
    /// <param name="withYAxis">Stop With YAxis</param>

    #endregion

    #region FlipSection

    public virtual void Flip()
    {
        FacingDir = FacingDir * -1;
        transform.Rotate(0, 180f, 0);
        OnFlip?.Invoke(FacingDir);
    }

    public virtual void FlipController(float x)
    {
        if (Mathf.Abs(x) < 0.05f) return;
        x = Mathf.Sign(x); //x 의 부호만 가져오거든
        if (Mathf.Abs(FacingDir + x) < 0.5f)
            Flip();
    }

    #endregion

    #region CheckCollisionSection

    public virtual bool IsGroundDetected(Vector3? offset = null)
    {
        Vector3 acturalOffset = offset ?? Vector3.zero;
        return Physics2D.BoxCast(groundChecker.position + acturalOffset,
            new Vector2(groundCheckBoxWidth, 0.05f), 0,
            Vector2.down, groundCheckDistance, whatIsGroundAndWall);

    }


    public virtual bool IsWallDetected() =>
        Physics2D.BoxCast(wallChecker.position,
            new Vector2(0.05f, wallCheckBoxHeight), 0,
            Vector2.right * FacingDir, wallCheckDistance, whatIsGroundAndWall);

    public virtual void CheckObjectOnFoot()
    {
        RaycastHit2D hit = Physics2D.BoxCast(groundChecker.position,
            new Vector2(groundCheckBoxWidth, 0.05f), 0,
            Vector2.down, groundCheckDistance, whatIsProbs);

        if (hit.collider != null && hit.collider.TryGetComponent(out Probs p))
            p.Interact(this);
    }

    #endregion

    public virtual void Dead(Vector2 dir) { }

    public virtual void KnockBack(Vector2 power)
    {
        if (CanKnockback == false) return;
        MovementCompo.StopImmediately(true);
        if (knockbackCoroutine != null) StopCoroutine(knockbackCoroutine);

        isKnockbacked = true;
        MovementCompo.SetVelocity(power, true);
        knockbackCoroutine = StartDelayCallBack(
            knockbackDuration, () =>
            {
                isKnockbacked = false;
                MovementCompo.StopImmediately(false);
            });
    }

    public virtual void Stun(float duration) { }
    public virtual void Stone(float duration)
    {
        visualCompo.OnStone(true);
        StartDelayCallBack(duration, () => visualCompo.OnStone(false));
    }

    public virtual void AirBorn(float duration) { }

    public virtual void UpArmor(int figure)
    {
        healthCompo.GetArmor(figure);
    }

    public virtual void LostArmor(int figure)
    {
        healthCompo.LostArmor(figure);
    }

    public virtual void Invincibility(float duration)
    {
        healthCompo.EnableInvincibility();
    }

    public virtual void InvincibilityDisable()
    {
        healthCompo.DisableInvincibility();
    }

    public virtual void Clean()
    {
        Debug.Log("Entity에서 실행된 Clean");
        CleanDamageManager.Instance.DamageObject();

        if (stunDuration > 0)
        {
            stunDuration = 0;
            canBeStun = false;
            Debug.Log("스턴 해제됨");
        }
    }

    //얘가 막 몇초 후 실행 시키기 그런걸 다 관리 해줄 거임
    public Coroutine StartDelayCallBack(float delay, Action callBack)
    {
        return StartCoroutine(DelayCoroutine(delay, callBack));
    }

    IEnumerator DelayCoroutine(float delay, Action callBack)
    {
        yield return new WaitForSeconds(delay);
        callBack?.Invoke();
    }

    public StatusEffect ApplyStatusEffect(StatusBuffEffectEnum statusEffect, int level, float duration)
    {
        if (IsUnderStatusEffect(statusEffect)) return null;
        _statusEffectBit |= (int)statusEffect;
        return _statusEffectManager.AddStatusEffect(statusEffect, level, duration);
    }
    public StatusEffect ApplyStatusEffect(StatusDebuffEffectEnum statusEffect, int level, float duration)
    {
        if (IsUnderStatusEffect(statusEffect)) return null;
        _statusEffectBit |= (int)statusEffect;
        return _statusEffectManager.AddStatusEffect(statusEffect, level, duration);
    }

    public void RemoveStatusEffect(StatusBuffEffectEnum statusEffect)
    {
        if (IsUnderStatusEffect(statusEffect) == false) return;
        _statusEffectBit &= ~(int)statusEffect;
    }
    public void RemoveStatusEffect(StatusDebuffEffectEnum statusEffect)
    {
        if (IsUnderStatusEffect(statusEffect) == false) return;
        _statusEffectBit &= ~(int)statusEffect;
    }

    public bool IsUnderStatusEffect(StatusBuffEffectEnum statusEffect)
        => (_statusEffectBit & (int)statusEffect) != 0;
    public bool IsUnderStatusEffect(StatusDebuffEffectEnum statusEffect)
        => (_statusEffectBit & (int)statusEffect) != 0;

    protected virtual void Update()
    {
        _statusEffectManager.UpdateStatusEffects();
    }

#if UNITY_EDITOR

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireCube(wallChecker.position,
            new Vector2(wallCheckDistance, wallCheckBoxHeight));
        Gizmos.DrawWireCube(groundChecker.position,
            new Vector2(groundCheckBoxWidth, groundCheckDistance));
    }

    public virtual void ApplyEffect()
    {
    }

    public virtual void AnimationTrigger(AnimationTriggerEnum trigger)
    {
        _animationTriggerBit |= (int)trigger;
    }

    public virtual bool IsTriggered(AnimationTriggerEnum trigger) => (_animationTriggerBit & (int)trigger) != 0;

    public virtual void RemoveTrigger(AnimationTriggerEnum trigger)
    {
        _animationTriggerBit &= ~(int)trigger;
    }
#endif
}
