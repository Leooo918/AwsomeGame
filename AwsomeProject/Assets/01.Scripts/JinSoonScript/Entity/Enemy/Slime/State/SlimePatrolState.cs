using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;

public class SlimePatrolState : EnemyState<SlimeStateEnum>
{
    private Slime slime;
    private bool readyChangeState = false;

    public SlimePatrolState(Enemy<SlimeStateEnum> enemy, EnemyStateMachine<SlimeStateEnum> enemyStateMachine, string animBoolName) : base(enemy, enemyStateMachine, animBoolName)
    {
        slime = enemy as Slime;
    }

    public override void Enter()
    {
        base.Enter();
        readyChangeState = false;
        enemy.patrolStartTime = Time.time;
        if (slime.readyFlip)
        {
            enemy.Flip();
            slime.readyFlip = false;
        }
    }

    public override void Exit()
    {
        base.Exit();
        enemy.patrolEndTime = Time.time;
    }

    public override void UpdateState()
    {
        base.UpdateState();

        if (readyChangeState) return;

        if (slime.moveAnim == true )
            enemy.SetVelocity(enemy.FacingDir * enemy.moveSpeed, 0, false, false);

        if (enemy.patrolStartTime + enemy.PatrolTime < Time.time)
        {
            enemyStateMachine.ChangeState(SlimeStateEnum.Idle);
            slime.readyFlip = true;
            return;
        }

        Player player = enemy.IsPlayerDetected();

        float dist = 0;
        if (player != null) dist = Vector3.Distance(player.transform.position + Vector3.up, enemy.transform.position);

        if (player != null && enemy.IsObstacleInLine(dist) == false)
        {
            enemyStateMachine.ChangeState(SlimeStateEnum.Chase);
            return;
        }

        if ((enemy.IsGroundDetected() == false || enemy.IsWallDetected() == true) && slime.readyFlip == false)
        {
            enemyStateMachine.ChangeState(SlimeStateEnum.Idle);
            slime.readyFlip = true;
            return;
        }
    }
}
