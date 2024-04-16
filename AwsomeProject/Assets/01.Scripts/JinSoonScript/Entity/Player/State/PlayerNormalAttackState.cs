using UnityEngine;

public class PlayerNormalAttackState : PlayerState
{
    private int comboCounterHash;
    private PlayerNormalAttackSO playerNormalAttackSO;
    private EntityAttack playerAttack;

    public PlayerNormalAttackState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
        comboCounterHash = Animator.StringToHash("ComboCounter");
        playerNormalAttackSO = player.SkillSO.GetSkillByEnum(PlayerSkillEnum.NormalAttack) as PlayerNormalAttackSO;
        playerAttack = player.GetComponent<EntityAttack>();
    }

    public override void Enter()
    {
        base.Enter();

        //시간이 지나고 공격하면 콤보카운터 초기화
        if (player.lastAttackTime + playerNormalAttackSO.attackComboDragTime <= Time.time)
            player.ComboCounter = 0;

        AttackInfo fAttackInfo = playerNormalAttackSO.firstAttackInfo;
        fAttackInfo.damage = 
            (int)(fAttackInfo.attackMultiplier * 
            player.Stat.GetStatByEnum(StatType.Damage).GetValue());

        AttackInfo sAttackInfo = playerNormalAttackSO.secondAttackInfo;
        sAttackInfo.damage = 
            (int)(sAttackInfo.attackMultiplier* 
            player.Stat.GetStatByEnum(StatType.Damage).GetValue());

        //공격 세팅해주고
        if (player.ComboCounter == 0)
            playerAttack.SetCurrentAttackInfo(fAttackInfo);
        else if(player.ComboCounter == 1)
            playerAttack.SetCurrentAttackInfo(sAttackInfo);

        player.StopImmediately(false);
        playerNormalAttackSO = player.SkillSO.GetSkillByEnum(PlayerSkillEnum.NormalAttack) as PlayerNormalAttackSO;

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
