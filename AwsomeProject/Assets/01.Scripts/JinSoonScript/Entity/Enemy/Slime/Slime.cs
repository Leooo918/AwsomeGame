using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SlimeEnum
{
    Idle,
    Move,
    Chase,
    Attack,
    Stun,
    Dead
}


public class Slime : Enemy
{
    public EnemyStateMachine<SlimeEnum> StateMachine { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        StateMachine = new EnemyStateMachine<SlimeEnum>();

        foreach (SlimeEnum stateEnum in Enum.GetValues(typeof(SlimeEnum)))
        {
            string typeName = stateEnum.ToString();
            Type t = Type.GetType($"Skeleton{typeName}State");
            try
            {
                var enemyState = Activator.CreateInstance(t, this, StateMachine, typeName) as EnemyState<SlimeEnum>;
                StateMachine.AddState(stateEnum, enemyState);
            }
            catch (Exception e)
            {
                Debug.LogError($"Enemy Skeleton : no state [ {typeName} ]");
                Debug.LogError(e);
            }
        }
    }

    protected void Start()
    {
        StateMachine.Initialize(SlimeEnum.Idle, this);
    }

    private void Update()
    {
        StateMachine.CurrentState.UpdateState();
    }

    public override void Stun(float duration)
    {
        if (isDead) return;
        stunDuration = duration;
        StateMachine.ChangeState(SlimeEnum.Stun);
    }

    public override void Dead(Vector2 dir)
    {

    }
}
