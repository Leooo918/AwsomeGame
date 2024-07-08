using System;
using System.Collections.Generic;


public class EnemyStateMachine<T> where T : Enum
{
    public EnemyState<T> CurrentState;
    public Dictionary<T, EnemyState<T>> stateDictionary;

    private Enemy<T> enemy;

    public EnemyStateMachine()
    {
        stateDictionary = new Dictionary<T, EnemyState<T>>();
    }

    public void Initialize(T startState, Enemy<T> enemy)
    {
        this.enemy = enemy;
        CurrentState = stateDictionary[startState];
        CurrentState.Enter();
    }

    public void ChangeState(T newState)
    {
        if (!enemy.CanStateChangeable) return;

        CurrentState.Exit();
        CurrentState = stateDictionary[newState];
        CurrentState.Enter();
    }

    public void AddState(T stateEnum, EnemyState<T> state)
    {
        stateDictionary.Add(stateEnum, state);
    }
}
