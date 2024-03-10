using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundState : PlayerState
{
    public PlayerGroundState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName) {}



    public override void Enter()
    {
        base.Enter();
        player.PlayerInput.JumpEvent += HandleJumpEvent;
        player.PlayerInput.DashEvent += HandleDashEvent;
    }


    public override void Exit()
    {
        player.PlayerInput.JumpEvent -= HandleJumpEvent;
        player.PlayerInput.DashEvent -= HandleDashEvent;
        base.Exit();
    }


    public override void UpdateState()
    {
        base.UpdateState();
        if (!player.IsGroundDetected())
        {
            stateMachine.ChangeState(PlayerStateEnum.Fall);
        }
    }


    #region HandleEventSection

    private void HandleJumpEvent()
    {
        if (player.IsGroundDetected())
            stateMachine.ChangeState(PlayerStateEnum.Jump);
    }

    private void HandleDashEvent()
    {
        stateMachine.ChangeState(PlayerStateEnum.Dash);
    }

    #endregion
}
