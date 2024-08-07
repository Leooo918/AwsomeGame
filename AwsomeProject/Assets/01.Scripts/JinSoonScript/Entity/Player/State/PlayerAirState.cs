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
        player.PlayerInput.JumpEvent += HandleJumpEvent;
        player.PlayerInput.DashEvent += HandleDashEvent;
        player.PlayerInput.AttackEvent += HandleAttackEvent;
    }

    public override void Exit()
    {
        base.Exit();
        player.PlayerInput.JumpEvent -= HandleJumpEvent;
        player.PlayerInput.DashEvent -= HandleDashEvent;
        player.PlayerInput.AttackEvent -= HandleAttackEvent;
    }


    public override void UpdateState()
    {
        base.UpdateState();

        //떨어질 때는 조금 천천히 움직여지게
        float xInput = player.PlayerInput.XInput;

        if (player.canClimb && player.PlayerInput.YInput != 0)
            stateMachine.ChangeState(PlayerStateEnum.Climb);

        if (Mathf.Abs(player.FacingDir + xInput) > 1.5f && player.IsWallDetected()) return;

        player.SetVelocity(player.MoveSpeed * xInput, rigidbody.velocity.y);
    }

    private void HandleJumpEvent()
    {
        if (player.CanJump)
        {
            player.curJumpCnt++;
            stateMachine.ChangeState(PlayerStateEnum.Jump);
            //player.CanJump = false;
        }
    }

    private void HandleDashEvent() => player.SkillSO.GetSkillByEnum(PlayerSkillEnum.Dash).skill.UseSkill();
    private void HandleAttackEvent() => player.SkillSO.GetSkillByEnum(PlayerSkillEnum.NormalAttack).skill.UseSkill();
}
