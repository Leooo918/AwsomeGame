using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeJumpAttackState : EnemyState<SlimeEnum>
{
    public SlimeJumpAttackState(Enemy enemy, EnemyStateMachine<SlimeEnum> enemyStateMachine, string animBoolName) : base(enemy, enemyStateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        enemy.SetVelocity(0, 5);
    }

    public override void UpdateState()
    {
        base.UpdateState();

        Debug.Log(enemy.IsGroundDetected() );
        if (enemy.IsGroundDetected() && enemy.rigidbodyCompo.velocity.y < 0)
        {
            Player player = enemy.DetectEnemyPos(1);
            Debug.Log(player);
            if (player != null)
            {
                Vector2 knockPower = (Vector2)(player.transform.position - enemy.transform.position).normalized + Vector2.up;
                player.playerHealth.TakeDamage(5, knockPower, enemy);
            }

            enemyStateMachine.ChangeState(SlimeEnum.Idle);
        }
    }
}
