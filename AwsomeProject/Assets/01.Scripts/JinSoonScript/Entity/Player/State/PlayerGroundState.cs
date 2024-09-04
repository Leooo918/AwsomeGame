using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundState : PlayerState
{
    private Skill dashSkill;
    private Skill normalAttackSkill;

    public PlayerGroundState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
        dashSkill = player.SkillSO.GetSkillByEnum(PlayerSkillEnum.Dash).skill;
        normalAttackSkill = player.SkillSO.GetSkillByEnum(PlayerSkillEnum.NormalAttack).skill;
    }

    public override void Enter()
    {
        base.Enter();
        player.curJumpCnt = 0;
        player.PlayerInput.JumpEvent += HandleJumpEvent;
        player.PlayerInput.DashEvent += HandleDashEvent;
        player.PlayerInput.AttackEvent += HandleAttackEvent;
        player.PlayerInput.OnTryUseQuickSlot += HandleThrowEvent;
    }

    public override void Exit()
    {
        player.PlayerInput.JumpEvent -= HandleJumpEvent;
        player.PlayerInput.DashEvent -= HandleDashEvent;
        player.PlayerInput.AttackEvent -= HandleAttackEvent;
        player.PlayerInput.OnTryUseQuickSlot -= HandleThrowEvent;
        base.Exit();
    }


    public override void UpdateState()
    {
        base.UpdateState();

        if (player.IsGroundDetected() == false)
            player.StartDelayCallBack(player.CoyoteTime, () => player.CanJump = false);
        else
            player.CanJump = true;

        Transform trm = player.CheckObstacleInFront();
        if (trm != null)
        {
            player.CurrentPushTrm = trm;
            stateMachine.ChangeState(PlayerStateEnum.Push);
        }

        if (player.CanJump == false && !player.IsGroundDetected())
        {
            player.CanJump = false;
            stateMachine.ChangeState(PlayerStateEnum.Fall);
        }
    }

    #region HandleEventSection

    private void HandleJumpEvent()
    {
        if (player.PlayerInput.YInput < 0)
        {
            player.CheckOneWayPlatform();
            return;
        }

        if (player.CanJump)
        {
            player.curJumpCnt = 1;
            stateMachine.ChangeState(PlayerStateEnum.Jump);
            //_player.CanJump = false;
        }
    }

    private void HandleDashEvent()
    {
        dashSkill.UseSkill();
    }

    private void HandleAttackEvent()
    {
        normalAttackSkill.UseSkill();
    }

    private void HandleThrowEvent()
    {
        stateMachine.ChangeState(PlayerStateEnum.Throw);
    }

    #endregion
}
