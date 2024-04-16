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
        jumpSkill = slime.SkillSO.GetSkillByEnum(SlimeSkillEnum.JumpAttack) as SlimeJumpSkillSO;
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
            enemy.entityAttack.SetCurrentAttackInfo(jumpSkill.AttackInfo);
            enemy.entityAttack.Attack();

            //���� Idle�� ��ȯ�ϴϱ� ��ڿ� ������ Patrol�� ��
            enemyStateMachine.ChangeState(SlimeEnum.Chase);
        }
    }
}
