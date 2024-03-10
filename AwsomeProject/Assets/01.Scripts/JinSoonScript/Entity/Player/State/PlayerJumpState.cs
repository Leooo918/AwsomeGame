using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : PlayerState
{
    public PlayerJumpState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName){}

    public override void Enter()
    {
        base.Enter();

        float xInput = player.PlayerInput.XInput;

        player.SetVelocity(xInput, player.jumpForce);
        stateMachine.ChangeState(PlayerStateEnum.Fall);
    }
}
