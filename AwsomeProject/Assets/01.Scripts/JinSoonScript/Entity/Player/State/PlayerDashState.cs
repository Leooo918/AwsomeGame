using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashState : PlayerState
{
    public PlayerDashState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName) 
    {
        _dashAttackColl = player.transform.Find("DashAttackCollider").GetComponent<Collider2D>();
        _dashTrail = player.transform.Find("DashTrail").GetComponent<ParticleSystem>();
    }

    private Collider2D _dashAttackColl;
    private ParticleSystem _dashTrail;
    private float dashTime;
    private float xInput;

    public override void Enter()
    {
        base.Enter();

        if (player.IsInvincibleWhileDash == true) player.colliderCompo.enabled = false;

        if (player.IsAttackWhileDash == true)
            _dashAttackColl.enabled = true;
        _dashTrail.Play();

            xInput = player.PlayerInput.XInput;

        if (xInput == 0) xInput = player.FacingDir * -0.5f;

        player.SetVelocity(player.DashPower * xInput, 0, true);
        dashTime = Time.time;
    }
    public override void UpdateState()
    {
        base.UpdateState();

        player.SetVelocity(player.DashPower * xInput, 0, true);

        if (Time.time - dashTime > player.DashTime)
        {
            stateMachine.ChangeState(PlayerStateEnum.Idle);
        }
    }

    public override void Exit()
    {
        base.Exit();
        if (player.IsInvincibleWhileDash == true) player.colliderCompo.enabled = true;

        if (player.IsAttackWhileDash == true)
            _dashAttackColl.enabled = false;
        _dashTrail.Stop();

        player.StopImmediately(false);
    }

}
