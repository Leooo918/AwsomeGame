using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGatheringState : PlayerGroundState
{
    public PlayerGatheringState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        player.StopImmediately(true);
    }
}
