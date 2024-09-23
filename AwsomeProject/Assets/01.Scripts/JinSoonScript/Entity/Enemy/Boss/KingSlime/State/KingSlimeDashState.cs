using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VHierarchy.Libs;

public class KingSlimeDashState : EnemyState<KingSlimeStateEnum>
{
    private KingSlime _kingSlime;
    private Vector2 _dashDir;

    private bool _isDashStarted = false;

    public KingSlimeDashState(Enemy<KingSlimeStateEnum> enemy, EnemyStateMachine<KingSlimeStateEnum> enemyStateMachine, string animBoolName) : base(enemy, enemyStateMachine, animBoolName)
    {
        _kingSlime = enemy as KingSlime;
    }

    public override void Enter()
    {
        base.Enter();
        _kingSlime.StartCoroutine(DashCoroutine());
    }

    public override void UpdateState()
    {
        base.UpdateState();
        if (_isDashStarted)
        {
            if (_kingSlime.IsWallDetected(out Collider2D collider)) // �Ĺھҵ�? �� �� �ĹھҴ����� �Ⱦ˷�����? �� �˰�ʹ�!!!!! ��� �����ε�
            {
                if(collider.TryGetComponent(out GrowingBush bush))
                {
                    bush.gameObject.Destroy();
                }
                else
                {
                    _kingSlime.FlipController(-_dashDir.x);
                }
                _kingSlime.KnockBack(-_dashDir * 2f);
                enemyStateMachine.ChangeState(KingSlimeStateEnum.Idle);
            }
        }
    }

    public override void Exit()
    {
        _isDashStarted = false;
        base.Exit();
    }

    private IEnumerator DashCoroutine()
    {
        //���� �� _kingSlime.dashInfos �ϳ� �̾ƿͼ� ���� �����̶� �̰����� �� �����ָ� ��.
        //��ġ�� �ٲ��ְ�.
        //DOJump��� �� ��� �޼��尡 �־��ݾ�???
        DashInfo info = _kingSlime.dashInfos[Random.Range(0, _kingSlime.dashInfos.Length)];
        _dashDir = info.direction;
        _kingSlime.FlipController(-_dashDir.x);
        _kingSlime.transform.DOJump(info.dashStartPos.position, 5, 1, 0.5f);
        yield return new WaitForSeconds(0.5f);
        _kingSlime.FlipController(_dashDir.x);
        yield return new WaitForSeconds(_kingSlime.beforeDashDelay);
        _kingSlime.MovementCompo.SetVelocity(_dashDir * _kingSlime.dashSpeed);
        _isDashStarted = true;
    }
}
