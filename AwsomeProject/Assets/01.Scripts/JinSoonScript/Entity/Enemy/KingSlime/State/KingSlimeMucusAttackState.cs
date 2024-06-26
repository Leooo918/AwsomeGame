using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingSlimeMucusAttackState : EnemyState<KingSlimeStateEnum>
{
    private KingSlime kingSlime;
    private GameObject mucusPf;
    private Tween moveTween;
    private bool isFired = false;
    private int doMucusAttackAnimTriggerHash = Animator.StringToHash("DoMucusAttack");

    public KingSlimeMucusAttackState(Enemy<KingSlimeStateEnum> enemy, EnemyStateMachine<KingSlimeStateEnum> enemyStateMachine, string animBoolName) : base(enemy, enemyStateMachine, animBoolName)
    {
        kingSlime = enemy as KingSlime;
        mucusPf = kingSlime?.mucus;
    }

    //���� �κ��� ���� ��ü�� ���� ¥��
    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();

        if (isFired == true)
        {
            enemy.StateMachine.ChangeState(KingSlimeStateEnum.Ready);
            return;
        }

        Vector2 fireDirection = new Vector2(0, 1);
        Vector2 playerDir = PlayerManager.Instance.PlayerTrm.position - kingSlime.transform.position;
        fireDirection.y *= fireDirection.magnitude;
        fireDirection.x = playerDir.x;

        fireDirection = fireDirection.normalized * fireDirection.magnitude;

        KingSlimeMucus mucusInstance = MonoBehaviour.Instantiate(mucusPf).GetComponent<KingSlimeMucus>();
        mucusInstance.transform.position = kingSlime.transform.position;
        mucusInstance.Fire(fireDirection * 2);
        isFired = true;
    }

    public override void Enter()
    {
        base.Enter();
        Vector3 targetPosition = kingSlime.GetJumpPos();
        moveTween = enemy.transform.DOJump(targetPosition, 10, 1, 0.5f)
            .OnComplete(() =>
            {
                enemy.animatorCompo.SetTrigger(doMucusAttackAnimTriggerHash);
            });
    }

    public override void Exit()
    {
        isFired = false;
        kingSlime.SetSkillAfterDelay();
        base.Exit();
    }
}
