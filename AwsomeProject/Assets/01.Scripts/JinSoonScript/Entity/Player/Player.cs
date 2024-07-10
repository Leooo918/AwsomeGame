using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public enum PlayerStateEnum
{
    Idle,
    Move,
    Jump,
    Fall,
    Dash,
    Gathering,
    Stun,
    NormalAttack,
    Climb,
    Dead
}

public enum PlayerSkillEnum
{
    Dash,
    NormalAttack
}

public class Player : Entity
{
    public PlayerSkill SkillSO { get; private set; }

    #region Status

    public Slider hpSlider;

    public float MoveSpeed { get; protected set; } = 7f;
    public float JumpForce { get; protected set; } = 10f;

    #endregion

    #region DashInfo

    public float DashTime { get; private set; }
    public float DashPower { get; private set; }
    public bool IsInvincibleWhileDash { get; private set; }
    public bool IsAttackWhileDash { get; private set; }

    #endregion

    #region CoyoteTime

    [SerializeField] private float coyoteTime = 0.3f;
    public float CoyoteTime => coyoteTime;
    [HideInInspector]
    public bool canJump = false;

    #endregion

    #region ComponentRegion

    public PlayerStateMachine StateMachine { get; private set; }
    [SerializeField] private InputReader _inputReader;
    public InputReader PlayerInput => _inputReader;

    #endregion

    #region Attack

    [HideInInspector] public int ComboCounter = 0;
    [HideInInspector] public float lastAttackTime;

    #endregion

    public GameObject StunEffect { get; private set; }
    private bool isInventoryOpen = false;
    public bool canClimb { get; private set; } = false;

    [SerializeField] private GameObject thowingPortionPf;
    [SerializeField] private HpBar hpBar;

    protected override void Awake()
    {
        base.Awake();

        MoveSpeed = Stat.moveSpeed.GetValue();
        JumpForce = Stat.jumpForce.GetValue();


        SkillSO = gameObject.AddComponent<PlayerSkill>();
        SkillSO.Init(EntitySkillSO);

        StateMachine = new PlayerStateMachine();
        foreach (PlayerStateEnum stateEnum in Enum.GetValues(typeof(PlayerStateEnum)))
        {
            string typeName = stateEnum.ToString();
            try
            {
                Type t = Type.GetType($"Player{typeName}State");
                PlayerState state = Activator.CreateInstance(t, this, StateMachine, typeName) as PlayerState;

                StateMachine.AddState(stateEnum, state);
            }
            catch (Exception ex)
            {
                Debug.LogError($"{typeName} is loading error!");
                Debug.LogError(ex);
            }
        }

        foreach (var item in EntitySkillSO.skills)
            item.skill.SetOwner(this);

        StunEffect = transform.Find("StunEffect").gameObject;
        StunEffect.SetActive(false);
    }

    private void OnEnable()
    {
        _inputReader.PressTabEvent += InventoryOpen;
        _inputReader.OpenOptionEvent += OpenOption;
        healthCompo.onKnockBack += KnockBack;
        healthCompo.onDie += OnDie;
        healthCompo.onHit += OnHit;
        healthCompo.onHit += () => hpBar.TakeDamage();
    }

    private void OnDisable()
    {
        _inputReader.PressTabEvent -= InventoryOpen;
        _inputReader.OpenOptionEvent -= OpenOption;
        healthCompo.onKnockBack -= KnockBack;
        healthCompo.onDie -= OnDie;
        healthCompo.onHit -= OnHit;
        healthCompo.onHit -= () => hpBar.TakeDamage();
    }

    protected void Start()
    {
        StateMachine.Initialize(PlayerStateEnum.Idle, this);
    }

    protected void Update()
    {
        SetHpSlider();

        StateMachine.CurrentState.UpdateState();
        CheckObjectOnFoot();
    }

    public override void Stun(float duration)
    {
        //if (canBeStun == false) return;
        this.stunDuration = duration;

        StateMachine.ChangeState(PlayerStateEnum.Stun);
    }

    public override void AirBorn(float duration)
    {

    }

    //public override void UpArmor(float figure)
    //{

    //}

    public override void Invincibility(float duration)
    {
        base.Invincibility(duration);
        StateMachine.ChangeState(PlayerStateEnum.Stun);
    }

    public override void InvincibilityDisable()
    {
        base.InvincibilityDisable();
        StateMachine.ChangeState(PlayerStateEnum.Idle);
    }

    public override void Clean()
    {
        base.Clean();
        StateMachine.ChangeState(PlayerStateEnum.Idle);
    }

    /// <summary>
    /// 대쉬하는
    /// </summary>
    /// <param name="dashTime">대쉬하는 시간</param>
    /// <param name="dashPower">대쉬하는 속도</param>
    /// <param name="isInvincibleWhileDash">대쉬하는 동안 무적인가</param>
    /// <param name="isAttackWhileDash">대쉬공격하는가</param>
    public void Dash(float dashTime, float dashPower, bool isInvincibleWhileDash = false, bool isAttackWhileDash = false)
    {
        this.DashTime = dashTime;
        this.DashPower = dashPower;
        this.IsInvincibleWhileDash = isInvincibleWhileDash;
        this.IsAttackWhileDash = isAttackWhileDash;

        StateMachine.ChangeState(PlayerStateEnum.Dash);
    }

    public void SetHpSlider()
    {
        //hpSlider.maxValue = healthCompo.maxHp.GetValue();
        //hpSlider.value = healthCompo.curHp;
    }

    /// <summary>
    /// 인벤토리 열때
    /// 움직이는거 다 빼뒀다가 나중에 다시 넣어주
    /// </summary>
    private void InventoryOpen()
    {
        if (isInventoryOpen == false)
        {
            _inputReader.Controlls.asset.FindAction("XMovement").Disable();
            _inputReader.Controlls.asset.FindAction("YMovement").Disable();
            isInventoryOpen = true;
        }
        else
        {
            _inputReader.Controlls.asset.FindAction("XMovement").Enable();
            _inputReader.Controlls.asset.FindAction("YMovement").Enable();
            isInventoryOpen = false;
        }
    }

    private void OpenOption()
    {
        UIManager.Instance.Open(UIType.Option);
    }

    public void AnimationFinishTrigger() => StateMachine.CurrentState.AnimationFinishTrigger();
    public void OnHit() => HitEvent?.Invoke();

    public void OnDie(Vector2 hitDir)
    {
        isDead = true;
        StateMachine.ChangeState(PlayerStateEnum.Dead);
    }

    public void ThrowPortion(PortionItem portion)
    {
        Vector3 spawnPosition = transform.position + new Vector3(0, 1, 0);
        ThrowingPortion throwingPortion =
            Instantiate(thowingPortionPf, spawnPosition, Quaternion.identity)
            .GetComponent<ThrowingPortion>();

        throwingPortion.Init(portion);
    }

    public void WeaponEnchant(PortionItem portion)
    {

    }

    public void Climb(bool b)
    {
        if (b == true)
            canClimb = true;
        else
            canClimb = false;
    }
}
