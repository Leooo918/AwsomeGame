using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirBirdStunState : EnemyState<AirBirdEnum>
{
    public AirBirdStunState(Enemy<AirBirdEnum> enemy, EnemyStateMachine<AirBirdEnum> enemyStateMachine, string animBoolName) : base(enemy, enemyStateMachine, animBoolName)
    {
    }
    private float _stunStartTime;

    public override void Enter()
    {
        base.Enter();
        _stunStartTime = Time.time;
        enemy.CanStateChangeable = false;
        enemy.rigidbodyCompo.gravityScale = 3.5f;
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

        if (Time.time > enemy.stunDuration + _stunStartTime && enemy.IsUnderStatusEffect(StatusDebuffEffectEnum.Floating) == false)
        {
            enemy.CanStateChangeable = true;
            enemyStateMachine.ChangeState(AirBirdEnum.Idle);
        }
    }
}
