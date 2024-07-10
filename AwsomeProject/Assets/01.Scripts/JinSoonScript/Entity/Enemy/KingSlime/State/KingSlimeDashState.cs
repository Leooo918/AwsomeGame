using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingSlimeDashState : EnemyState<KingSlimeStateEnum>
{
    private KingDashSkillSO _dashSkillSO;

    public KingSlimeDashState(Enemy<KingSlimeStateEnum> enemy, EnemyStateMachine<KingSlimeStateEnum> enemyStateMachine, string animBoolName) : base(enemy, enemyStateMachine, animBoolName)
    {
        if (enemy?.EntitySkillSO.GetSkillSO("KingDash") != null)
            _dashSkillSO = enemy?.EntitySkillSO.GetSkillSO("KingDash") as KingDashSkillSO;
    }

    private int _dashStartHash = Animator.StringToHash("DashStart");
    private bool _isDashing = false;
    private float dashTime;
    private float dashDir;

    public override void Enter()
    {
        base.Enter();
        Debug.Log("밍");

        //대쉬 중 무적?
        //enemy.colliderCompo.enabled = false;

        //이건 고치고
        //enemy.transform.Find("DashAttackCollider").GetComponent<Collider2D>().enabled = true;

        enemy.CanStateChangeable = false;
        dashDir = enemy.FacingDir;
        enemy.DashPower = _dashSkillSO.dashPower;
        enemy.DashTime = _dashSkillSO.dashTime;
        if (dashDir == 0) dashDir = enemy.FacingDir;
    }
    public override void UpdateState()
    {
        base.UpdateState();

        Debug.Log(_isDashing);
        if (_isDashing == false) return;

        enemy.SetVelocity(enemy.DashPower * dashDir, 0, true);

        if (Time.time - dashTime > enemy.DashTime)
        {
            enemy.CanStateChangeable = true;
            enemyStateMachine.ChangeState(KingSlimeStateEnum.Ready);
        }

        if (enemy.IsWallDetected())
        {
            Vector2 knockDir = new Vector2(-enemy.FacingDir * 2, 1).normalized;
            knockDir *= 10f;
            enemy.CanStateChangeable = true;
            enemy.Stun(4f);
            enemy.KnockBack(knockDir);
        }
    }

    public override void Exit()
    {
        base.Exit();
        //대쉬 중 무적?
        //enemy.colliderCompo.enabled = true;

        //이건 고치고
        //enemy.transform.Find("DashAttackCollider").GetComponent<Collider2D>().enabled = false;
        _isDashing = false;
        enemy.StopImmediately(false);
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
        enemy.animatorCompo.SetTrigger(_dashStartHash);
        _isDashing = true;

        enemy.SetVelocity(enemy.DashPower * dashDir, 0, false);
        dashTime = Time.time;
    }
}
