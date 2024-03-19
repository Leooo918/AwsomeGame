using System;
using UnityEngine;
using UnityEngine.InputSystem;

public enum PlayerStateEnum
{
    Idle,
    Move,
    Jump,
    Fall,
    Dash,
    Gathering,
    Stun,
    Attack
}


public class Player : Entity
{
    #region PlayerState

    //이거 나중에 SO로 빼고
    [Header("PlayerStat")]
    public float moveSpeed = 7f;
    public float jumpForce = 5f;
    public float dashTime = 0.3f;
    public float dashPower = 10f;

    #endregion

    public PlayerStateMachine StateMachine { get; private set; }
    [SerializeField] private InputReader _inputReader;
    public InputReader PlayerInput => _inputReader;

    private bool isInventoryOpen = false;


    protected override void Awake()
    {
        base.Awake();
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
    }

    private void OnEnable()
    {
        _inputReader.PressTabEvent += InventoryOpen;
    }

    private void OnDisable()
    {
        _inputReader.PressTabEvent -= InventoryOpen;
    }

    protected override void Start()
    {
        StateMachine.Initialize(PlayerStateEnum.Idle, this);
    }

    protected override void Update()
    {
        StateMachine.CurrentState.UpdateState();
        CheckObjectOnFoot();
    }

    public override void Stun(Vector2 stunPower, float duration)
    {
        //if (canBeStun == false) return;

        this.stunPower = stunPower;
        this.stunDuration = duration;

        StateMachine.ChangeState(PlayerStateEnum.Stun);
    }

    public override void Attack()
    {

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
}
