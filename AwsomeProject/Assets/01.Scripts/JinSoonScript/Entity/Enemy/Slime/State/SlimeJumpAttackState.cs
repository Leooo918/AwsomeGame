using Spine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeJumpAttackState : EnemyState<SlimeEnum>
{
    private float playerDir;
    private SlimeJumpSkillSO jumpSkill;
    private Slime slime;

    public SlimeJumpAttackState(Enemy enemy, EnemyStateMachine<SlimeEnum> enemyStateMachine, string animBoolName) : base(enemy, enemyStateMachine, animBoolName)
    {
        slime = enemy as Slime;
        jumpSkill = slime.slimeStatus.GetSkillByEnum(SlimeSkillEnum.JumpAttack) as SlimeJumpSkillSO;
    }

    public override void Enter()
    {
        base.Enter();
        enemy.SetVelocity(0, jumpSkill.jumpPower.GetValue());
        playerDir = Mathf.Sign((PlayerManager.Instance.playerTrm.position - enemy.transform.position).x);
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
                player.healthCompo.TakeDamage(5, knockPower, enemy);
            }

            //망할 Idle로 전환하니까 등뒤에 있으면 Patrol로 감
            enemyStateMachine.ChangeState(SlimeEnum.Chase);
        }
    }
}
