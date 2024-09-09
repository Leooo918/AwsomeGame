using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class PlayerFallState : PlayerAirState
{
    public PlayerFallState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    } 

    public override void UpdateState()
    {
        base.UpdateState();

        if (player.IsGroundDetected())
            stateMachine.ChangeState(PlayerStateEnum.Idle);
    }
}
