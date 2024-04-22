using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashState : PlayerState
{
    public PlayerDashState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName) { }

    private float dashTime;
    private float xInput;

    public override void Enter()
    {
        base.Enter();

        if (player.IsInvincibleWhileDash == true) player.colliderCompo.enabled = false;

        if (player.IsAttackWhileDash == true) player.transform.Find("DashAttackCollider").GetComponent<Collider2D>().enabled = true;

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

        if (player.IsAttackWhileDash == true) player.transform.Find("DashAttackCollider").GetComponent<Collider2D>().enabled = false;

        player.StopImmediately(false);
    }

}
