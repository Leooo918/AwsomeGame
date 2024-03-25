using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class PlayerIdleState : PlayerGroundState
{
    public PlayerIdleState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.StopImmediately(false);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void UpdateState()
    {
        base.UpdateState();

        float xInput = player.PlayerInput.XInput;

        //이동방향과 벽방향이 같다면 return
        if (Mathf.Abs(player.FacingDir + xInput) > 1.5f && player.IsWallDetected())
            return;

        if (Mathf.Abs(xInput) > 0.05f)
            stateMachine.ChangeState(PlayerStateEnum.Move);
    }
}
