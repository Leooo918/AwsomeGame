using UnityEngine;
using UnityEngine.Rendering;

public class SlimeChaseState : EnemyState<SlimeStateEnum>
{
    private Slime _slime;
    private Player _player;

    public SlimeChaseState(Enemy<SlimeStateEnum> enemy, EnemyStateMachine<SlimeStateEnum> enemyStateMachine, string animBoolName) : base(enemy, enemyStateMachine, animBoolName)
    {
        _slime = enemy as Slime;
        _player = PlayerManager.Instance.Player;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void UpdateState()
    {
        base.UpdateState();

        if ((_player = enemy.IsPlayerDetected()) && enemy.IsPlayerInAttackRange() == null)
        {
            Vector3 moveDir = ((_player.transform.position.x - _slime.transform.position.x) * Vector3.right).normalized;

            enemy.MovementCompo.SetVelocity(moveDir * enemy.EnemyStat.moveSpeed.GetValue());
        }
        else
            enemyStateMachine.ChangeState(SlimeStateEnum.Idle);
    }
}
