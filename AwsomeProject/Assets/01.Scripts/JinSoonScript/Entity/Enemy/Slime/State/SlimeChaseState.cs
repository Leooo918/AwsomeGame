using UnityEngine;

public class SlimeChaseState : EnemyState<SlimeStateEnum>
{
    private Slime slime;
    private SlimeJumpSkillSO jumpSkill;
    private bool canJump = false;
    private Transform playerTrm;
    private bool chaseStart = false;

    public SlimeChaseState(Enemy<SlimeStateEnum> enemy, EnemyStateMachine<SlimeStateEnum> enemyStateMachine, string animBoolName) : base(enemy, enemyStateMachine, animBoolName)
    {
        slime = enemy as Slime;
    }

    public override void Enter()
    {
        base.Enter();

        playerTrm = PlayerManager.Instance.PlayerTrm;
        enemy.animatorCompo.SetBool(animBoolHash, false);

        enemy.FindPlayerEvt(() =>
        {
            chaseStart = true;
            enemy.animatorCompo.SetBool(animBoolHash, true);
        });
    }

    public override void Exit()
    {
        base.Exit();
        chaseStart = false;
    }

    public override void UpdateState()
    {
        base.UpdateState();

        //�i���ִ� ���̾ƴ϶�� return
        if (chaseStart == false) return;

        //�Ÿ��� �־����� �� ���ư�
        float dist = Vector3.Distance(playerTrm.position, enemy.transform.position);

        if (dist > enemy.detectingDistance + 5)
        {
            enemy.MissPlayer();
            enemyStateMachine.ChangeState(SlimeStateEnum.Return);
        }

        //���� �������� ������ ����!
        if (dist <= enemy.attackDistance) slime.Attack();

        //�տ� ���� ���ٸ� ���Ѹ� ��
        if (slime.CheckFront() == false) return;

        //���� ���θ��� ������ ����
        canJump = slime.IsGroundDetected();
        if (slime.IsWallDetected() == true && canJump == true) Jump();

        //��� �i�ư��� ���ְ�
        Vector2 dir = (playerTrm.position - enemy.transform.position).normalized;
        if (Mathf.Sign(dir.x) != Mathf.Sign(enemy.FacingDir)) enemy.Flip();

        if (slime.moveAnim == true)
            enemy.SetVelocity(dir.x * enemy.moveSpeed, enemy.rigidbodyCompo.velocity.y);
    }

    private void Jump()
    {
        if (jumpSkill == null)
            jumpSkill = slime.Skills.GetSkillByEnum(SlimeSkillEnum.JumpAttack) as SlimeJumpSkillSO;

        enemy.SetVelocity(0, jumpSkill.jumpPower.GetValue());
    }
}
