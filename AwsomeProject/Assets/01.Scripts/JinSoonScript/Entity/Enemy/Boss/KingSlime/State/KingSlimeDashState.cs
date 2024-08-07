using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingSlimeDashState : EnemyState<KingSlimeStateEnum>
{
    private KingDashSkillSO _dashSkillSO;
    private BossHealth _bossHealth;
    private Collider2D _dashContactCollider;
    private Collider2D _contactcollider;

    private readonly int _maxDashCnt = 3;
    private int _curDashCnt = 0;

    public KingSlimeDashState(Enemy<KingSlimeStateEnum> enemy, EnemyStateMachine<KingSlimeStateEnum> enemyStateMachine, string animBoolName) : base(enemy, enemyStateMachine, animBoolName)
    {
        if (enemy?.EntitySkillSO.GetSkillSO("KingDash") != null)
            _dashSkillSO = enemy?.EntitySkillSO.GetSkillSO("KingDash") as KingDashSkillSO;

        _bossHealth = enemy.healthCompo as BossHealth;
    }

    private int _dashStartHash = Animator.StringToHash("DashStart");
    private bool _isDashing = false;
    private float dashTime;
    private float dashDir;

    public override void Enter()
    {
        base.Enter();

        //이건 고치고
        _dashContactCollider = enemy.transform.Find("DashContactCollider").GetComponent<Collider2D>();
        _contactcollider = enemy.transform.Find("EnemyContactCollider").GetComponent<Collider2D>();

        enemy.CanKnockback = false;
        enemy.CanStateChangeable = false;
        dashDir = enemy.FacingDir;
        enemy.DashPower = _dashSkillSO.dashPower;
        enemy.DashTime = _dashSkillSO.dashTime;
    }

    public override void UpdateState()
    {
        base.UpdateState();

        if (_isDashing == false) return;

        enemy.SetVelocity(enemy.DashPower * dashDir, 0, true);
        if (Time.time - dashTime > enemy.DashTime)
        {
            if (_bossHealth.currentPhase == 1 && ++_curDashCnt < _maxDashCnt)
            {
                DashAgain();
                return;
            }

            enemy.CanStateChangeable = true;
            enemyStateMachine.ChangeState(KingSlimeStateEnum.Ready);
        }

        if (enemy.IsWallDetected())
            HitWall();
    }

    public override void Exit()
    {
        base.Exit();

        CameraManager.Instance.StopShakeCam();
        _dashContactCollider.enabled = false;
        _contactcollider.enabled = true;

        enemy.colliderCompo.enabled = true;
        enemy.CanKnockback = true;

        _curDashCnt = 0;
        _isDashing = false;
        enemy.StopImmediately(false);
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();

        //대쉬 중 무적?
        CameraManager.Instance.StartShakeCam(3, 3);
        enemy.colliderCompo.enabled = false;

        _dashContactCollider.enabled = true;
        _contactcollider.enabled = false;

        enemy.animatorCompo.SetTrigger(_dashStartHash);
        _isDashing = true;

        enemy.SetVelocity(enemy.DashPower * dashDir, 0, false);
        dashTime = Time.time;
    }

    private void DashAgain()
    {
        enemy?.animatorCompo?.SetBool(animBoolHash, false);
        CameraManager.Instance.StopShakeCam();
        _dashContactCollider.enabled = false;
        _contactcollider.enabled = true;

        enemy.colliderCompo.enabled = true;
        enemy.CanKnockback = true;

        _isDashing = false;
        enemy.StopImmediately(false);
        dashDir = PlayerManager.Instance.PlayerTrm.position.x < enemy.transform.position.x ? -1 : 1;
        if (dashDir != enemy.FacingDir) enemy.Flip();

        enemy.StartDelayCallBack(0.3f, Enter);
    }

    private void HitWall()
    {
        CameraManager.Instance.ShakeCam(10, 10, 0.2f);
        Vector2 knockDir = new Vector2(-enemy.FacingDir * 2, 1).normalized;
        knockDir *= 10f;
        enemy.CanStateChangeable = true;
        enemy.Stun(4f);
        enemy.KnockBack(knockDir);
    }
}
