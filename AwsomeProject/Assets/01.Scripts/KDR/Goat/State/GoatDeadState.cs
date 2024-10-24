using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoatDeadState : EnemyState<GoatEnum>
{
    public GoatDeadState(Enemy<GoatEnum> enemy, EnemyStateMachine<GoatEnum> enemyStateMachine, string animBoolName) : base(enemy, enemyStateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        GameManager.Instance.killCnt++;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void UpdateState()
    {
        base.UpdateState();
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
        enemy.OnCompletelyDie();
    }
}