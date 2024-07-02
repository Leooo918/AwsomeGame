using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerClimbState : PlayerState
{
    private Skill dashSkill;
    private Skill normalAttackSkill;

    public PlayerClimbState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
        dashSkill = player.SkillSO.GetSkillByEnum(PlayerSkillEnum.Dash).skill;
        normalAttackSkill = player.SkillSO.GetSkillByEnum(PlayerSkillEnum.NormalAttack).skill;
    }

    public override void Enter()
    {
        base.Enter();
        player.PlayerInput.JumpEvent += HandleJumpEvent;
        player.PlayerInput.DashEvent += HandleDashEvent;
        player.PlayerInput.AttackEvent += HandleAttackEvent;
    }


    public override void Exit()
    {
        player.PlayerInput.JumpEvent -= HandleJumpEvent;
        player.PlayerInput.DashEvent -= HandleDashEvent;
        player.PlayerInput.AttackEvent -= HandleAttackEvent;
        base.Exit();
    }

    public override void UpdateState()
    {
        base.UpdateState();

        float xInput = player.PlayerInput.XInput;
        float yInput = player.PlayerInput.YInput;

        player.SetVelocity(xInput * player.MoveSpeed / 2, yInput * player.MoveSpeed);

        if (player.canClimb == false)
            stateMachine.ChangeState(PlayerStateEnum.Idle);
    }

    #region HandleEventSection

    private void HandleJumpEvent()
    {
        stateMachine.ChangeState(PlayerStateEnum.Jump);
        player.canJump = false;
    }

    private void HandleDashEvent()
    {
        dashSkill.UseSkill();
    }

    private void HandleAttackEvent()
    {
        normalAttackSkill.UseSkill();
    }

    #endregion
}
