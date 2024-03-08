using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class PlayerMoveState : PlayerGroundState
{
    public PlayerMoveState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName) { }

    public override void UpdateState()
    {
        base.UpdateState();
        float xInput = player.PlayerInput.XInput;
        player.SetVelocity(xInput * player.moveSpeed, rigidbody.velocity.y);

        if (Mathf.Abs(xInput) < 0.05f || player.IsWallDetected())
        {
            stateMachine.ChangeState(PlayerStateEnum.Idle);
        }
    }
}
