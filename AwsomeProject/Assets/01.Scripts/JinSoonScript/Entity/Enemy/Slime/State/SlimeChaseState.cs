using UnityEngine;

public class SlimeChaseState : EnemyState<SlimeEnum>
{
    private Slime slime;
    private SlimeJumpSkillSO jumpSkill;
    private bool canJump = false;
    private Transform playerTrm;
    private bool chaseStart = false;

    public SlimeChaseState(Enemy enemy, EnemyStateMachine<SlimeEnum> enemyStateMachine, string animBoolName) : base(enemy, enemyStateMachine, animBoolName)
    {
        slime = enemy as Slime;
        jumpSkill = slime.SkillSO.GetSkillByEnum(SlimeSkillEnum.JumpAttack) as SlimeJumpSkillSO;
    }

    public override void Enter()
    {
        base.Enter();

        playerTrm = PlayerManager.Instance.playerTrm;
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

        //쫒고있는 중이아니라면 return
        if (chaseStart == false) return;

        //거리가 멀어지면 걍 돌아가
        float dist = Vector3.Distance(playerTrm.position, enemy.transform.position);

        if (dist > enemy.detectingDistance + 5)
        {
            enemy.MissPlayer();
            enemyStateMachine.ChangeState(SlimeEnum.Return);
        }

        //공격 범위내에 들어오면 공격!
        if (dist <= enemy.attackDistance) slime.Attack();

        //앞에 땅이 없다면 지켜만 봐
        if (slime.CheckFront() == false) return;

        //벽이 가로막고 있으면 점프
        canJump = slime.IsGroundDetected();
        if (slime.IsWallDetected() == true && canJump == true) Jump();

        //계속 쫒아가게 해주고
        Vector2 dir = (playerTrm.position - enemy.transform.position).normalized;
        if (Mathf.Sign(dir.x) != Mathf.Sign(enemy.FacingDir)) enemy.Flip();

        if (slime.moveAnim == true)
            enemy.SetVelocity(dir.x * enemy.moveSpeed, enemy.rigidbodyCompo.velocity.y);
    }

    private void Jump()
    {
        Debug.Log("점프");
        enemy.SetVelocity(0, jumpSkill.jumpPower.GetValue());
    }
}
