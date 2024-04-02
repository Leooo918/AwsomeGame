using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeReturnState : EnemyState<SlimeEnum>
{
    public SlimeReturnState(Enemy enemy, EnemyStateMachine<SlimeEnum> enemyStateMachine, string animBoolName) : base(enemy, enemyStateMachine, animBoolName)
    {
    }

    private Vector2 dir;

    public override void Enter()
    {
        base.Enter();
        if (Vector2.Distance(enemy.transform.position, enemy.patrolOriginPos.position) > 20)
        {
            enemy.transform.position = enemy.patrolOriginPos.position;
            enemy.patrolEndTime = Time.time;
            enemyStateMachine.ChangeState(SlimeEnum.Idle);
        }
    }

    public override void UpdateState()
    {
        base.UpdateState();
        if (Vector2.Distance(enemy.transform.position, enemy.patrolOriginPos.position) <= 0.5f)
        {
            enemy.patrolEndTime = Time.time;
            enemyStateMachine.ChangeState(SlimeEnum.Idle);
        }

        dir = (enemy.patrolOriginPos.position - enemy.transform.position).normalized * enemy.moveSpeed;
        enemy.SetVelocity(dir.x, dir.y);
    }
}
