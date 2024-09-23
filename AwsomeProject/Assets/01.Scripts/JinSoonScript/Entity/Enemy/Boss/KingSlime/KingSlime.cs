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
    //JumpAndFall,
    Dash,
    //Turret,
    //Stun
}

public class KingSlime : Enemy<KingSlimeStateEnum>
{
    [Header("Boss Variable")]
    public int idleTime;

    [Header("Pattern Settings")]
    public DashInfo[] dashInfos;
    public float beforeDashDelay = 3f;
    public float dashSpeed = 20f;
    public LayerMask whatIsWall;
    [Space]
    public Transform[] onPlatformTrms;
    public float fireDelay = 1.0f;


    protected override void Awake()
    {
        base.Awake();
        StateMachine.Initialize(KingSlimeStateEnum.Idle, this);
    }


    protected override void Update()
    {
        base.Update();
    }
}
