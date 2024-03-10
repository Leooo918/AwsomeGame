using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{
    #region PlayerState

    public float moveSpeed = 7f;
    public float jumpForce = 5f;
    public float dashTime = 0.3f;
    public float dashPower = 10f;

    #endregion

    public PlayerStateMachine StateMachine { get; private set; }
    [SerializeField] private InputReader _inputReader;
    public InputReader PlayerInput => _inputReader;


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

    protected override void Start()
    {
        StateMachine.Initialize(PlayerStateEnum.Idle, this);
    }

    protected override void Update()
    {
        StateMachine.CurrentState.UpdateState();
        CheckObjectOnFoot();
    }

    public override void Attack()
    {

    }
}
