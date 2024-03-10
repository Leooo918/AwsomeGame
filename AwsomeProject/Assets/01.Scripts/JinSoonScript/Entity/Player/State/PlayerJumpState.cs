using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : PlayerAirState
{
    public PlayerJumpState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName){}

    public override void Enter()
    {
        base.Enter();

        float xInput = player.PlayerInput.XInput;

        player.SetVelocity(xInput, player.jumpForce);
    }


    public override void UpdateState()
    {
        base.UpdateState();

        //float xInput = player.PlayerInput.XInput;
        //player.SetVelocity(xInput * player.moveSpeed, rigidbody.velocity.y);

        if (player.rigidbodyCompo.velocity.y <= 0)
            player.StateMachine.ChangeState(PlayerStateEnum.Fall);
    }
}
