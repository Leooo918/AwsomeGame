using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirBirdIdleState : EnemyState<AirBirdEnum>
{
    private bool _isGoDown = false;

    private float _originHeight;
    private float _upDownSpeed = 2f;

    public AirBirdIdleState(Enemy<AirBirdEnum> enemy, EnemyStateMachine<AirBirdEnum> enemyStateMachine, string animBoolName) : base(enemy, enemyStateMachine, animBoolName)
    {
        _originHeight = enemy.transform.position.y;
    }

    public override void Enter()
    {
        base.Enter();
        enemy.MovementCompo.RigidbodyCompo.gravityScale = 0;
        enemy.MovementCompo.StopImmediately(true);
    }

    public override void UpdateState()
    {
        base.UpdateState();

        Player player;
        if(player = enemy.IsPlayerDetected())
        {
            Vector2 playerDir = player.transform.position + Vector3.up - enemy.transform.position;
            if (enemy.IsObstacleInLine(playerDir.magnitude)) return;

            enemy.FlipController(playerDir.x);

            if (enemy.IsPlayerInAttackRange())
            {
                if (enemy.lastAttackTime + enemy.attackCool < Time.time)
                {
                    enemy.lastAttackTime = Time.time;
                    enemyStateMachine.ChangeState(AirBirdEnum.Shoot);
                }
            }
            else
            {
                enemyStateMachine.ChangeState(AirBirdEnum.Chase);
            }
        }
    }
}
