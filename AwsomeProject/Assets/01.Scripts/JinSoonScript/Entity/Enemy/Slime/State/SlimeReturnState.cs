using UnityEngine;

public class SlimeReturnState : EnemyState<SlimeStateEnum>
{
    private Slime slime;
    private SlimeJumpSkillSO jumpSkill;
    private bool isJumping = false;

    public SlimeReturnState(Enemy<SlimeStateEnum> enemy, EnemyStateMachine<SlimeStateEnum> enemyStateMachine, string animBoolName) : base(enemy, enemyStateMachine, animBoolName)
    {
        slime = enemy as Slime;
    }

    private Vector2 dir;


    public override void UpdateState()
    {
        base.UpdateState();

        if (slime.moveAnim == true)
            enemy.SetVelocity(dir.x, enemy.rigidbodyCompo.velocity.y);

        //만약 앞에 벽이 가로막고 있다면
        if (enemy.IsWallDetected() == true)
            if (isJumping == false) Jump();

        if (enemy.CheckFront() == true) isJumping = false;
    }

    private void Jump()
    {
        if(jumpSkill == null)
            jumpSkill = slime.Skills.GetSkillByEnum(SlimeSkillEnum.JumpAttack) as SlimeJumpSkillSO;

        enemy.SetVelocity(0, jumpSkill.jumpPower.GetValue());
        isJumping = true;
    }
}
