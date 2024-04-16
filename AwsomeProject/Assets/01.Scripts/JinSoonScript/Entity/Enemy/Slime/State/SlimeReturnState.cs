using UnityEngine;

public class SlimeReturnState : EnemyState<SlimeEnum>
{
    private Slime slime;
    private SlimeJumpSkillSO jumpSkill;
    private bool isJumping = false;

    public SlimeReturnState(Enemy enemy, EnemyStateMachine<SlimeEnum> enemyStateMachine, string animBoolName) : base(enemy, enemyStateMachine, animBoolName)
    {
        slime = enemy as Slime;
        jumpSkill = slime.SkillSO.GetSkillByEnum(SlimeSkillEnum.JumpAttack) as SlimeJumpSkillSO;
    }

    private Vector2 dir;

    public override void Enter()
    {
        base.Enter();
        //만약 원래있던 곳이 너무 멀다면 걍 순간이동해
        if (Vector2.Distance(enemy.transform.position, enemy.patrolOriginPos.position) > 30)
        {
            enemy.transform.position = enemy.patrolOriginPos.position;
            enemy.patrolEndTime = Time.time;
            enemyStateMachine.ChangeState(SlimeEnum.Idle);
        }
    }

    public override void UpdateState()
    {
        base.UpdateState();

        //순찰을 시작할 곳 까지 움직여
        if (Vector2.Distance(enemy.transform.position, enemy.patrolOriginPos.position) <= 0.5f)
        {
            Debug.Log("밍 돌아온");
            enemy.patrolEndTime = Time.time;
            enemyStateMachine.ChangeState(SlimeEnum.Idle);
        }

        dir = (enemy.patrolOriginPos.position - enemy.transform.position).normalized * enemy.moveSpeed;
        if (slime.moveAnim == true)
            enemy.SetVelocity(dir.x, enemy.rigidbodyCompo.velocity.y);

        //만약 앞에 벽이 가로막고 있다면
        if (enemy.IsWallDetected() == true)
        {
            if (isJumping == false) Jump();
        }

        if (enemy.CheckFront() == true) isJumping = false;
    }

    private void Jump()
    {
        enemy.SetVelocity(0, jumpSkill.jumpPower.GetValue());
        isJumping = true;
    }
}
