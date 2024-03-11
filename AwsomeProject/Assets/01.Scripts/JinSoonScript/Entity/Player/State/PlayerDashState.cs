using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashState : PlayerState
{
    public PlayerDashState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName) { }

    private float dashTime;
    float xInput;

    public override void Enter()
    {
        base.Enter();

        xInput = player.PlayerInput.XInput;

        if (xInput == 0) xInput = player.FacingDir * -0.5f;

        player.SetVelocity(player.dashPower * xInput, 0, true);
        dashTime = Time.time;
    }
    public override void UpdateState()
    {
        base.UpdateState();

        player.SetVelocity(player.dashPower * xInput, 0, true);

        if (Time.time - dashTime > player.dashTime)
        {
            stateMachine.ChangeState(PlayerStateEnum.Idle);
        }
    }

    public override void Exit()
    {
        base.Exit();
        player.StopImmediately(false);
    }

}
