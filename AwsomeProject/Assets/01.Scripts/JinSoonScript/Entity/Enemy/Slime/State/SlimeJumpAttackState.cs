using Spine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeJumpAttackState : EnemyState<SlimeStateEnum>
{
    private float playerDir;
    private SlimeJumpSkillSO jumpSkill;
    private Slime slime;

    private int _jumpDownAnimHash = Animator.StringToHash("JumpDown");
    private int _landAnimHash = Animator.StringToHash("Land");

    public SlimeJumpAttackState(Enemy<SlimeStateEnum> enemy, EnemyStateMachine<SlimeStateEnum> enemyStateMachine, string animBoolName) : base(enemy, enemyStateMachine, animBoolName)
    {
        slime = enemy as Slime;
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();

        //망할 Idle로 전환하니까 등뒤에 있으면 Patrol로 감
        enemyStateMachine.ChangeState(SlimeStateEnum.Chase);
    }

    public override void Enter()
    {
        base.Enter();
        if(jumpSkill == null)
            jumpSkill = slime.Skills.GetSkillByEnum(SlimeSkillEnum.JumpAttack) as SlimeJumpSkillSO;

        enemy.MovementCompo.SetVelocity(new Vector2(0, jumpSkill.jumpPower.GetValue()));
        playerDir = Mathf.Sign((PlayerManager.Instance.PlayerTrm.position - enemy.transform.position).x);
    }

    public override void UpdateState()
    {
        base.UpdateState();

        enemy.MovementCompo.SetVelocity(new Vector2(playerDir * enemy.moveSpeed, enemy.rigidbodyCompo.velocity.y));

        if (enemy.rigidbodyCompo.velocity.y < 0)
            enemy.animatorCompo.SetTrigger(_jumpDownAnimHash);

        if (enemy.IsGroundDetected() && enemy.rigidbodyCompo.velocity.y < 0)
        {
            enemy.entityAttack.SetCurrentAttackInfo(jumpSkill.AttackInfo);
            enemy.entityAttack.Attack();

            enemy.animatorCompo.SetTrigger(_landAnimHash);
            enemy.MovementCompo.StopImmediately(false);
        }
    }
}
