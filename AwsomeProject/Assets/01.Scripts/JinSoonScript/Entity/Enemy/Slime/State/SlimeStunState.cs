using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeStunState : EnemyState<SlimeStateEnum>
{
    public SlimeStunState(Enemy<SlimeStateEnum> enemy, EnemyStateMachine<SlimeStateEnum> enemyStateMachine, string animBoolName) : base(enemy, enemyStateMachine, animBoolName) { }

    private float _stunStartTime;

    public override void Enter()
    {
        base.Enter();
        _stunStartTime = Time.time;
        enemy.CanStateChangeable = false;
    }

    public override void Exit()
    {
        base.Exit();
        enemy.animatorCompo.speed = 1;
    }

    public override void UpdateState()
    {
        base.UpdateState();

        if (enemy.IsGroundDetected() && enemy.rigidbodyCompo.velocity.x != 0)
        {
            enemy.MovementCompo.StopImmediately();
        }

        if (Time.time > enemy.stunDuration + _stunStartTime)
        {
            enemy.CanStateChangeable = true;
            enemyStateMachine.ChangeState(SlimeStateEnum.Idle);
        }
    }
}
