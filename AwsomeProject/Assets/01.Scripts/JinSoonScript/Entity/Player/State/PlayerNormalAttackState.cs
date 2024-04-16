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

        //�ð��� ������ �����ϸ� �޺�ī���� �ʱ�ȭ
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

        //���� �������ְ�
        if (player.ComboCounter == 0)
            playerAttack.SetCurrentAttackInfo(fAttackInfo);
        else if(player.ComboCounter == 1)
            playerAttack.SetCurrentAttackInfo(sAttackInfo);

        player.StopImmediately(false);
        playerNormalAttackSO = player.SkillSO.GetSkillByEnum(PlayerSkillEnum.NormalAttack) as PlayerNormalAttackSO;

        //�ִϸ��̼� ���� �޺�ī���ͷ� �׸��� +1 �ٵ� 2�̻��̸� �ٽ� 0����
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
