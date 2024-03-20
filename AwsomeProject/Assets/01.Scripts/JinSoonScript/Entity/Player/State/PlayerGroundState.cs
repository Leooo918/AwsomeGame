using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundState : PlayerState
{
    private float coyoteTimer;
    private bool timerStart = false;

    public PlayerGroundState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName) { }

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

        if (player.IsGroundDetected() == false)
            player.StartDelayCallBack(player.CoyoteTime, () => player.canJump = false);
        else
            player.canJump = true;



        if (player.canJump == false && !player.IsGroundDetected())
        {
            Debug.Log("¹Ö¹Ö¹Ö");
            player.canJump = false;
            stateMachine.ChangeState(PlayerStateEnum.Fall);
        }
    }



    #region HandleEventSection

    private void HandleJumpEvent()
    {
        if (player.canJump)
        {
            stateMachine.ChangeState(PlayerStateEnum.Jump);
            player.canJump = false;
        }
    }

    private void HandleDashEvent()
    {
        stateMachine.ChangeState(PlayerStateEnum.Dash);
    }

    #endregion
}
