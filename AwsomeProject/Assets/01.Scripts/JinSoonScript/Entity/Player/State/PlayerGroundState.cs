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

        if (player.IsGroundDetected() == false)
            player.StartDelayCallBack(player.CoyoteTime, () => player.canJump = false);
        else
            player.canJump = true;



        if (player.canJump == false && !player.IsGroundDetected())
        {
            player.canJump = false;
            stateMachine.ChangeState(PlayerStateEnum.Fall);
        }
    }



    #region HandleEventSection

    private void HandleJumpEvent()
    {
        if (player.canJump)
        {
            stateMachine.ChangeState(PlayerStateEnum.Jump);
            player.canJump = false;
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

    #endregion
}
