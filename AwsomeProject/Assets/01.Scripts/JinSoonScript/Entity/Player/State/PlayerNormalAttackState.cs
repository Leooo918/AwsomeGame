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

        //�ð��� ������ �����ϸ� �޺�ī���� �ʱ�ȭ
        if (player.lastAttackTime + playerNormalAttackSO.attackComboDragTime <= Time.time)
            player.ComboCounter = 0;

        //���� �������ְ�
        if (player.ComboCounter == 0)
            playerAttack.SetCurrentAttackInfo(playerNormalAttackSO.firstAttackInfo);
        else if(player.ComboCounter == 1)
            playerAttack.SetCurrentAttackInfo(playerNormalAttackSO.secondAttackInfo);

        player.StopImmediately(false);
        playerNormalAttackSO = player.playerStatus.GetSkillByEnum(PlayerSkill.NormalAttack) as PlayerNormalAttackSO;

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
