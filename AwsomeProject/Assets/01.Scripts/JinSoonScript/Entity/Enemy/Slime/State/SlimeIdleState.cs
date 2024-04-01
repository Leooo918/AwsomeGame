using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeIdleState : EnemyState<SlimeEnum>
{
    public SlimeIdleState(Enemy enemy, EnemyStateMachine<SlimeEnum> enemyStateMachine, string animBoolName) : base(enemy, enemyStateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void UpdateState()
    {
        base.UpdateState();

        Player player = enemy.IsPlayerDetected();

        if (player != null && enemy.IsObstacleInLine(enemy.runAwayDistance) == false)
            enemy.FindPlayerEvt(() => enemyStateMachine.ChangeState(SlimeEnum.Chase));

        if (enemy.patrolEndTime + enemy.PatrolDelay < Time.time)
            enemyStateMachine.ChangeState(SlimeEnum.Patrol);
    }
}
