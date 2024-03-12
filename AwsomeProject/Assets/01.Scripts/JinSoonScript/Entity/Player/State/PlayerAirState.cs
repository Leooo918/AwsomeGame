using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAirState : PlayerState
{
    public PlayerAirState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName) {}

    public override void Enter()
    {
        base.Enter();
        player.PlayerInput.DashEvent += HandleDashEvent;
    }

    public override void Exit()
    {
        base.Exit();
        player.PlayerInput.DashEvent -= HandleDashEvent;
    }


    public override void UpdateState()
    {
        base.UpdateState();

        //������ ���� ���� õõ�� ����������
        float xInput = player.PlayerInput.XInput;

        if (Mathf.Abs(player.FacingDir + xInput) > 1.5f && player.IsWallDetected()) return;

        player.SetVelocity(player.moveSpeed * xInput, rigidbody.velocity.y);
    }


    private void HandleDashEvent()
    {
        player.StateMachine.ChangeState(PlayerStateEnum.Dash);
    }
}