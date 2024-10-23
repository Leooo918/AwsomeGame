using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoatChaseState : EnemyState<GoatEnum>
{
    public GoatChaseState(Enemy<GoatEnum> enemy, EnemyStateMachine<GoatEnum> enemyStateMachine, string animBoolName) : base(enemy, enemyStateMachine, animBoolName)
    {
    }

    private Slime _slime;
    private Player _player;
    private float _lastJumpTime;
    private float _jumpCool = 0.7f;

    public override void Exit()
    {
        base.Exit();
    }

    public override void UpdateState()
    {
        base.UpdateState();

        if (_player = enemy.IsPlayerDetected())
        {
            if (enemy.IsPlayerInAttackRange() == null)
            {
                Vector3 moveDir = ((_player.transform.position.x - _slime.transform.position.x) * Vector3.right).normalized;
                enemy.MovementCompo.SetVelocity(moveDir * enemy.EnemyStat.moveSpeed.GetValue());
            }
            else if (enemy.IsPlayerInAttackRange(1) == null)
            {
                if (enemy.lastAttackTime + _slime.attackCool < Time.time && enemy.IsGroundDetected())
                {
                    enemy.lastAttackTime = Time.time;
                    enemyStateMachine.ChangeState(GoatEnum.Attack);
                    return;
                }
                Vector3 moveDir = ((_slime.transform.position.x - _player.transform.position.x) * Vector3.right).normalized;
                enemy.FlipController(moveDir.x);
                if (enemy.IsFrontGround())
                    enemy.MovementCompo.SetVelocity(moveDir * enemy.EnemyStat.moveSpeed.GetValue());
                else
                {
                    enemy.FlipController(_player.transform.position.x - _slime.transform.position.x);
                    enemyStateMachine.ChangeState(GoatEnum.Idle);
                    return;
                }
            }
            else
            {
                enemy.FlipController(_player.transform.position.x - _slime.transform.position.x);
                enemyStateMachine.ChangeState(GoatEnum.Idle);
                return;
            }
        }
        else
        {
            enemyStateMachine.ChangeState(GoatEnum.Idle);
            return;
        }

        if (_slime.IsWallDetected())
        {
            if (_slime.IsWallDetected(1) == false)
            {
                if (_lastJumpTime + _jumpCool < Time.time)
                {
                    _lastJumpTime = Time.time;
                    enemy.MovementCompo.SetVelocity(Vector2.up * enemy.EnemyStat.jumpForce.GetValue(), withYVelocity: true);
                }
            }
            else
            {
                enemyStateMachine.ChangeState(GoatEnum.Idle);
            }
        }
    }
}
