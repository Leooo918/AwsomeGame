using UnityEngine;

public class PlayerNormalAttackState : PlayerState
{
    private int comboCounterHash;
    private PlayerNormalAttackSO playerNormalAttackSO;
    private PlayerAttack playerAttack;

    public PlayerNormalAttackState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
        comboCounterHash = Animator.StringToHash("ComboCounter");
        playerNormalAttackSO = player.playerStatus.GetSkillByEnum(PlayerSkill.NormalAttack) as PlayerNormalAttackSO;
        playerAttack = player.GetComponent<PlayerAttack>();
    }

    public override void Enter()
    {
        base.Enter();

        //시간이 지나고 공격하면 콤보카운터 초기화
        if (player.lastAttackTime + playerNormalAttackSO.attackComboDragTime <= Time.time)
            player.ComboCounter = 0;

        //공격 세팅해주고
        if (player.ComboCounter == 0)
            playerAttack.SetCurrentAttackInfo(playerNormalAttackSO.firstAttackInfo);
        else if(player.ComboCounter == 1)
            playerAttack.SetCurrentAttackInfo(playerNormalAttackSO.secondAttackInfo);

        player.StopImmediately(false);
        playerNormalAttackSO = player.playerStatus.GetSkillByEnum(PlayerSkill.NormalAttack) as PlayerNormalAttackSO;

        //애니메이션 실행 콤보카운터로 그리고 +1 근데 2이상이면 다시 0으로
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
