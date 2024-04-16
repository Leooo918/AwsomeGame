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
        player.PlayerInput.DashEvent += player.SkillSO.GetSkillByEnum(PlayerSkillEnum.Dash).skill.UseSkill;
        player.PlayerInput.AttackEvent += player.SkillSO.GetSkillByEnum(PlayerSkillEnum.NormalAttack).skill.UseSkill;
    }

    public override void Exit()
    {
        base.Exit();
        player.PlayerInput.DashEvent -= player.SkillSO.GetSkillByEnum(PlayerSkillEnum.Dash).skill.UseSkill;
        player.PlayerInput.AttackEvent -= player.SkillSO.GetSkillByEnum(PlayerSkillEnum.NormalAttack).skill.UseSkill;
    }


    public override void UpdateState()
    {
        base.UpdateState();

        //떨어질 때는 조금 천천히 움직여지게
        float xInput = player.PlayerInput.XInput;

        if (Mathf.Abs(player.FacingDir + xInput) > 1.5f && player.IsWallDetected()) return;

        player.SetVelocity(player.moveSpeed * xInput, rigidbody.velocity.y);
    }
}
