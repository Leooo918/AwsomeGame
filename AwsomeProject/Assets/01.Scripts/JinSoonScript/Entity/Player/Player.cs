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

    public float moveSpeed { get; protected set; } = 7f;
    public float jumpForce { get; protected set; } = 10f;

    #endregion

    #region DashInfo

    public float dashTime { get; private set; }
    public float dashPower { get; private set; }
    public bool isInvincibleWhileDash { get; private set; }
    public bool isAttackWhileDash { get; private set; }

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

    [HideInInspector]public int ComboCounter = 0;
    [HideInInspector] public float lastAttackTime;

    #endregion

    public GameObject stunEffect { get; private set; }

    private bool isInventoryOpen = false;

    protected override void Awake()
    {
        base.Awake();

        moveSpeed = Stat.moveSpeed.GetValue();
        jumpForce = Stat.jumpForce.GetValue();


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

        stunEffect = transform.Find("StunEffect").gameObject;
        stunEffect.SetActive(false);
    }

    private void OnEnable()
    {
        _inputReader.PressTabEvent += InventoryOpen;
        healthCompo.onKnockBack += KnockBack;
        healthCompo.onDie += OnDie;
        healthCompo.onHit += OnHit;
    }

    private void OnDisable()
    {
        _inputReader.PressTabEvent -= InventoryOpen;
        healthCompo.onKnockBack -= KnockBack;
        healthCompo.onDie -= OnDie;
        healthCompo.onHit -= OnHit;
    }

    protected void Start()
    {
        StateMachine.Initialize(PlayerStateEnum.Idle, this);
    }


    protected void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            DashSkill d = SkillSO.GetSkillByEnum(PlayerSkillEnum.Dash).skill as DashSkill;
            d.canUseSkill = true;
        }

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

    public void Dash(float dashTime, float dashPower, bool isInvincibleWhileDash = false, bool isAttackWhileDash = false)
    {
        this.dashTime = dashTime;
        this.dashPower = dashPower;
        this.isInvincibleWhileDash = isInvincibleWhileDash;
        this.isAttackWhileDash = isAttackWhileDash;

        StateMachine.ChangeState(PlayerStateEnum.Dash);
    }

    public void SetHpSlider()
    {
        hpSlider.maxValue = healthCompo.maxHp.GetValue();
        hpSlider.value = healthCompo.curHp;
    }

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

    public void AnimationFinishTrigger()
    {
        StateMachine.CurrentState.AnimationFinishTrigger();
    }

    public void OnHit() => HitEvent?.Invoke();

    public void OnDie(Vector2 hitDir)
    {
        isDead = true;
        StateMachine.ChangeState(PlayerStateEnum.Dead);
    }
}
