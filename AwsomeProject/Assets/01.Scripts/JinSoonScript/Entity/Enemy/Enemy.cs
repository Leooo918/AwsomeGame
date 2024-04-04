using DG.Tweening;
using System;
using UnityEngine;
public abstract class Enemy : Entity
{
    public StatusSO enemyStatus;

    #region EnemyStat
    public float moveSpeed { get; protected set; }
    public float PatrolTime { get; protected set; }
    public float PatrolDelay { get; protected set; }
    public float detectingDistance { get; protected set; }
    public float attackDistance {  get; protected set; }

    #endregion

    #region Settings

    [SerializeField] private Transform findPlayerMark;

    [SerializeField] private LayerMask whatIsPlayer;
    [SerializeField] private LayerMask whatIsObstacle;

    public Transform patrolOriginPos;

    [Header("Attack Settings")]
    public float runAwayDistance;
    [HideInInspector] public float lastAttackTime;

    #endregion

    [HideInInspector] public float patrolStartTime;
    [HideInInspector] public float patrolEndTime;

    public float defaultMoveSpeed { get; protected set; }
    protected int lastAnimationBoolHash;

    private bool playerDetected = false;

    protected override void Awake()
    {
        base.Awake();
        defaultMoveSpeed = moveSpeed;
        healthCompo.Init(enemyStatus);
    }


    #region DetectRegion

    public virtual Player IsPlayerDetected()
    {
        Collider2D player = Physics2D.OverlapCircle(transform.position, detectingDistance, whatIsPlayer);

        if (player == null)
            return null;

        float dir = player.transform.position.x - transform.position.x;

        if (dir > 0 == (FacingDir > 0)) return player.GetComponent<Player>();
        else return null;
    }

    public virtual bool IsObstacleInLine(float distance)
    {
        Vector2 dir = ((PlayerManager.instance.playerTrm.position + Vector3.up) - transform.position).normalized;

        return Physics2D.Raycast(transform.position, dir, distance, whatIsObstacle);
    }

    public Player DetectEnemyPos(float radius)
    {
        Collider2D coll = Physics2D.OverlapCircle(transform.position, radius, whatIsPlayer);

        if(coll == null) return null;

        return coll.GetComponent<Player>();
    }

    #endregion


    public virtual void FindPlayerEvt(Action action)
    {
        if (playerDetected == true)
        {
            action?.Invoke();
            return;
        }

        SpriteRenderer sr = findPlayerMark.GetComponent<SpriteRenderer>();
        sr.color = new Color(1, 1, 1, 1);
        findPlayerMark.localScale = Vector3.one;

        findPlayerMark.DOScaleY(1, 0.5f)
            .OnComplete(() =>
            {
                action?.Invoke();
                sr.DOFade(0, 0.5f);
                playerDetected = true;
            });
    }

    public void MissPlayer() => playerDetected = false;

    public void OnCompletelyDie()
    {
        //Ǯ�� �ϸ� ���⿡�ٰ� �߰����ָ� ����
        Destroy(gameObject);
    }

    public void DropItem()
    {
        //'0'
    }
}
