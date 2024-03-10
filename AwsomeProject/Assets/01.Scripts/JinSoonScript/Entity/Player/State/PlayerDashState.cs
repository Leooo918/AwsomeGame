using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashState : PlayerState
{
    public PlayerDashState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName){}

    private float dashTime;

    public override void Enter()
    {
        base.Enter();

        float xInput = player.PlayerInput.XInput;

        if (xInput == 0) xInput = -0.5f;

        player.SetVelocity(player.dashPower * xInput, rigidbody.velocity.y, true);
        dashTime = Time.time;
    }
    public override void UpdateState()
    {
        base.UpdateState();

        if(Time.time - dashTime > player.dashTime)
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
