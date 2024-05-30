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

    public override void Enter()
    {
        base.Enter();
        //���� �����ִ� ���� �ʹ� �ִٸ� �� �����̵���
        if (Vector2.Distance(enemy.transform.position, enemy.patrolOriginPos.position) > 30)
        {
            enemy.transform.position = enemy.patrolOriginPos.position;
            enemy.patrolEndTime = Time.time;
            enemyStateMachine.ChangeState(SlimeStateEnum.Idle);
        }
    }

    public override void UpdateState()
    {
        base.UpdateState();

        //������ ������ �� ���� ������
        if (Vector2.Distance(enemy.transform.position, enemy.patrolOriginPos.position) <= 0.5f)
        {
            Debug.Log("�� ���ƿ�");
            enemy.patrolEndTime = Time.time;
            enemyStateMachine.ChangeState(SlimeStateEnum.Idle);
        }

        dir = (enemy.patrolOriginPos.position - enemy.transform.position).normalized * enemy.moveSpeed;
        if (slime.moveAnim == true)
            enemy.SetVelocity(dir.x, enemy.rigidbodyCompo.velocity.y);

        //���� �տ� ���� ���θ��� �ִٸ�
        if (enemy.IsWallDetected() == true)
        {
            if (isJumping == false) Jump();
        }

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
