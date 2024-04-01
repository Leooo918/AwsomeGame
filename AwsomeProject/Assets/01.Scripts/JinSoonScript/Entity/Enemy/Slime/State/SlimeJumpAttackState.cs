using Spine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeJumpAttackState : EnemyState<SlimeEnum>
{
    private float playerDir;

    public SlimeJumpAttackState(Enemy enemy, EnemyStateMachine<SlimeEnum> enemyStateMachine, string animBoolName) : base(enemy, enemyStateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        enemy.SetVelocity(0, 5);
        playerDir = Mathf.Sign((PlayerManager.instance.playerTrm.position - enemy.transform.position).x);
    }

    public override void UpdateState()
    {
        base.UpdateState();

        enemy.SetVelocity(playerDir * enemy.moveSpeed, enemy.rigidbodyCompo.velocity.y);

        if (enemy.IsGroundDetected() && enemy.rigidbodyCompo.velocity.y < 0)
        {
            Player player = enemy.DetectEnemyPos(1);
            if (player != null)
            {
                Vector2 knockPower = (Vector2)(player.transform.position - enemy.transform.position).normalized + Vector2.up;
                knockPower *= 10f;
                player.playerHealth.TakeDamage(5, knockPower, enemy);
            }

            //���� Idle�� ��ȯ�ϴϱ� ��ڿ� ������ Patrol�� ��
            enemyStateMachine.ChangeState(SlimeEnum.Chase);
        }
    }
}
