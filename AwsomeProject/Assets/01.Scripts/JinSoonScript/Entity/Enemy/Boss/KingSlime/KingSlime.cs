using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct DashInfo
{
    public Transform dashStartPos;
    public Vector2 direction;
}

public enum KingSlimeStateEnum
{
    Idle = 0,
    JumpAndFall = 1,
    Dash = 2,
    Stun,
    Dead
    //Turret,
    //Stun
}

public class KingSlime : Enemy<KingSlimeStateEnum>
{
    [Header("Boss Variable")]
    public EnemyContactHit contactHit;
    public int idleTime;
    public PatternEffect[] patternEffects;

    [Header("Pattern Settings")]
    public DashInfo[] dashInfos;
    public float beforeDashDelay = 3f;
    public float dashSpeed = 20f;
    public LayerMask whatIsWall;
    [Space]
    public float beforeJumpDelay = 0.5f;
    public float beforeFallDelay = 2f;
    public float jumpYDistance;
    public GrowingBush bushPrefab;
    public Transform centerTrm;


    protected override void Awake()
    {
        base.Awake();
        StateMachine.Initialize(KingSlimeStateEnum.Idle, this);

        healthCompo.OnDie += OnDie;
    }


    private void OnDestroy()
    {
        healthCompo.OnDie -= OnDie;
    }

    public override void Stun(float duration)
    {
        if (IsDead) return;
        base.Stun(duration);
        if (StateMachine.CurrentState is not KingSlimeIdleState)
            return;
        StateMachine.ChangeState(KingSlimeStateEnum.Stun);
    }

    private void OnDie(Vector2 dir)
    {
        CanStateChangeable = true;
        StateMachine.ChangeState(KingSlimeStateEnum.Dead);
        CanStateChangeable = false;
        IsDead = true;
    }

    protected override void Update()
    {
        base.Update();
    }
}
