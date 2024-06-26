using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingSlimeDashState : EnemyState<KingSlimeStateEnum>
{
    public KingSlimeDashState(Enemy<KingSlimeStateEnum> enemy, EnemyStateMachine<KingSlimeStateEnum> enemyStateMachine, string animBoolName) : base(enemy, enemyStateMachine, animBoolName)
    {
    }

    private float dashTime;
    private float xInput;

    public override void Enter()
    {
        base.Enter();

        //대쉬 중 무적?
        //enemy.colliderCompo.enabled = false;

        //이건 고치고
        //enemy.transform.Find("DashAttackCollider").GetComponent<Collider2D>().enabled = true;

        xInput = enemy.FacingDir;

        if (xInput == 0) xInput = enemy.FacingDir * -0.5f;

        enemy.SetVelocity(enemy.DashPower * xInput, 0, true);
        dashTime = Time.time;
    }
    public override void UpdateState()
    {
        base.UpdateState();

        enemy.SetVelocity(enemy.DashPower * xInput, 0, true);

        if (Time.time - dashTime > enemy.DashTime)
        {
            enemyStateMachine.ChangeState(KingSlimeStateEnum.Ready);
        }
    }

    public override void Exit()
    {
        base.Exit();
        //대쉬 중 무적?
        //enemy.colliderCompo.enabled = true;

        //이건 고치고
        //enemy.transform.Find("DashAttackCollider").GetComponent<Collider2D>().enabled = false;

        enemy.StopImmediately(false);
    }
}
