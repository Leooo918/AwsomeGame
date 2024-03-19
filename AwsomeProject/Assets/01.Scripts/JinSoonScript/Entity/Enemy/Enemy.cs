using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{
    [Header("Common Settings")]
    public float moveSpeed;
    public float battleTime;
    public float idleTime;

    public float defaultMoveSpeed;

    [SerializeField] private LayerMask whatIsPlayer;
    [SerializeField] private LayerMask whatIsObstacle;

    [Header("Attack Settings")]
    public float runAwayDistance;
    public float attackDistance;
    public float attackCoolDown;
    [HideInInspector] public float lastAttackTime;

    protected int lastAnimationBoolHash;

    protected override void Awake()
    {
        base.Awake();
        defaultMoveSpeed = moveSpeed;
    }

    public virtual RaycastHit2D IsPlayerDetected()
       => Physics2D.Raycast(wallChecker.position, Vector2.right * FacingDir, runAwayDistance, whatIsPlayer);

    public virtual bool IsObstacleInLine(float distance)
        => Physics2D.Raycast(wallChecker.position, Vector2.right * FacingDir, distance, whatIsObstacle);
}
