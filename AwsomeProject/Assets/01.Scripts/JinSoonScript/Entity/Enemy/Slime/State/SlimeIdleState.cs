using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class SlimeIdleState : EnemyState<SlimeStateEnum>
{
    public SlimeIdleState(Enemy<SlimeStateEnum> enemy, EnemyStateMachine<SlimeStateEnum> enemyStateMachine, string animBoolName) : base(enemy, enemyStateMachine, animBoolName) { }

    private float _startTime;
    private float _patrolCool;
    private Player _player;
    private Slime _slime;


    public override void Enter()
    {
        base.Enter();

        enemy.MovementCompo.StopImmediately();

        _startTime = Time.time;
        _patrolCool = Random.Range(1f, 3f);
    }

    public override void UpdateState()
    {
        base.UpdateState();


        if ((_player = enemy.IsPlayerInAttackRange()) && _slime.lastAttackTime + _slime.attackCool < Time.time)
        {
            _slime.lastAttackTime = Time.time;
            enemyStateMachine.ChangeState(SlimeStateEnum.JumpAttack);
        }
        else if (enemy.IsPlayerDetected() != null)
        {
            enemyStateMachine.ChangeState(SlimeStateEnum.Chase);
        }

        if (_startTime + _patrolCool < Time.time)
        {
            enemyStateMachine.ChangeState(SlimeStateEnum.Patrol);
        }
    }
}
