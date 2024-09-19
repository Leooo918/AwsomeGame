using Spine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeJumpAttackState : EnemyState<SlimeStateEnum>
{
    private float playerDir;
    private SlimeJumpSkillSO jumpSkill;
    private Slime _slime;

    private int _jumpDownAnimHash = Animator.StringToHash("JumpDown");
    private int _landAnimHash = Animator.StringToHash("Land");

    public SlimeJumpAttackState(Enemy<SlimeStateEnum> enemy, EnemyStateMachine<SlimeStateEnum> enemyStateMachine, string animBoolName) : base(enemy, enemyStateMachine, animBoolName)
    {
        _slime = enemy as Slime;
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();

        //망할 Idle로 전환하니까 등뒤에 있으면 Patrol로 감
        enemyStateMachine.ChangeState(SlimeStateEnum.Idle);
    }

    public override void Enter()
    {
        base.Enter();

    }

    public override void UpdateState()
    {
        base.UpdateState();
    }
}
