using UnityEngine;

public class PlayerNormalAttackState : PlayerState
{
    private int comboCounterHash;
    private PlayerNormalAttackSO playerNormalAttackSO;

    public PlayerNormalAttackState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
        comboCounterHash = Animator.StringToHash("ComboCounter");
    }

    public override void Enter()
    {
        base.Enter();
        playerNormalAttackSO = player.playerStatus.GetSkillByEnum(PlayerSkill.NormalAttack) as PlayerNormalAttackSO;
        if (player.lastAttackTime + playerNormalAttackSO.attackComboDragTime <= Time.time) player.ComboCounter = 0;
        player.animatorCompo.SetInteger(comboCounterHash, player.ComboCounter++);
        if (player.ComboCounter > 1) player.ComboCounter = 0;

        player.lastAttackTime = Time.time;
    }

    public override void AnimationFinishTrigger()
    {
        player.StateMachine.ChangeState(PlayerStateEnum.Idle);
        base.AnimationFinishTrigger();
    }
}
