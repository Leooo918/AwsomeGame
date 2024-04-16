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
        //���� �����ִ� ���� �ʹ� �ִٸ� �� �����̵���
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

        //������ ������ �� ���� ������
        if (Vector2.Distance(enemy.transform.position, enemy.patrolOriginPos.position) <= 0.5f)
        {
            Debug.Log("�� ���ƿ�");
            enemy.patrolEndTime = Time.time;
            enemyStateMachine.ChangeState(SlimeEnum.Idle);
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
        enemy.SetVelocity(0, jumpSkill.jumpPower.GetValue());
        isJumping = true;
    }
}
