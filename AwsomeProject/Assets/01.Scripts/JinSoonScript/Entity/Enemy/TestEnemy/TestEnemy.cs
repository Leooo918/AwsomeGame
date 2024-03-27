using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TestEnemyEnum
{
    Idle,
    Run
}

public class TestEnemy : Enemy {
    public EnemyStateMachine<TestEnemyEnum> StateMachine { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        StateMachine = new EnemyStateMachine<TestEnemyEnum>();

        foreach (TestEnemyEnum stateEnum in Enum.GetValues(typeof(TestEnemyEnum)))
        {
            string typeName = stateEnum.ToString();
            Type t = Type.GetType($"TestEnemy{typeName}State");
            try
            {
                var enemyState = Activator.CreateInstance(t, this, StateMachine, typeName) as EnemyState<TestEnemyEnum>;
                StateMachine.AddState(stateEnum, enemyState);
            }
            catch (Exception e)
            {
                Debug.LogError($"Enemy TestEnemy : no state [ {typeName} ]");
                Debug.LogError(e);
            }
        }
    }


    protected void Start()
    {
        StateMachine.Initialize(TestEnemyEnum.Idle, this);
    }

    protected void Update()
    {
        StateMachine.CurrentState.UpdateState();
    }
}
