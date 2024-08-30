using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;

public class SlimePatrolState : EnemyState<SlimeStateEnum>
{
    private Slime _slime;

    private float _turningDelay = 2f;
    private float _lastTurnTime;
    private Player _player;

    public SlimePatrolState(Enemy<SlimeStateEnum> enemy, EnemyStateMachine<SlimeStateEnum> enemyStateMachine, string animBoolName) : base(enemy, enemyStateMachine, animBoolName)
    {
        _player = PlayerManager.Instance.Player;
        _slime = enemy as Slime;
    }

    public override void Enter()
    {
        base.Enter();
        _lastTurnTime = Time.time;
    }

    public override void UpdateState()
    {
        base.UpdateState();

        if (_lastTurnTime + _turningDelay < Time.time)
        {
            enemy.Flip();
            _lastTurnTime = Time.time;
        }

        if (_slime.moveAnim == true)
            enemy.SetVelocity(enemy.FacingDir * enemy.moveSpeed, 0, false, false);

        Player player = enemy.IsPlayerDetected();

        float dist = dist = Vector3.Distance(_player.transform.position + Vector3.up, enemy.transform.position);
        if ((player != null && enemy.IsObstacleInLine(dist) == false) || dist < 3)
        {
            enemyStateMachine.ChangeState(SlimeStateEnum.Chase);
            return;
        }

        if ((enemy.IsGroundDetected() == false || enemy.IsWallDetected() == true))
        {
            enemy.Flip();
            _lastTurnTime = Time.time;
            return;
        }
    }
}
