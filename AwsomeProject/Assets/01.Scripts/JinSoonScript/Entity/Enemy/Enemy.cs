using DG.Tweening;
using System;
using UnityEngine;
public abstract class Enemy : Entity
{
    [SerializeField] private EnemyStatSO enemyStat;
    public EnemyStatSO EnemyStat => enemyStat;

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
        enemyStat = Instantiate(enemyStat);
    }

    #region DetectRegion

    public virtual Player IsPlayerDetected()
    {
        Collider2D player = Physics2D.OverlapCircle(transform.position, detectingDistance, whatIsPlayer);

        if (player == null)
            return null;

        Player playerCompo = player.GetComponent<Player>();
        float dist = Vector2.Distance(player.transform.position, transform.position);

        if (dist < 2.5f) return playerCompo;

        float dir = player.transform.position.x - transform.position.x;

        if (dir > 0 == (FacingDir > 0)) return playerCompo;
        else return null;
    }

    public virtual bool IsObstacleInLine(float distance)
    {
        Vector2 dir = ((PlayerManager.Instance.playerTrm.position + Vector3.up) - transform.position).normalized;

        return Physics2D.Raycast(transform.position, dir, distance, whatIsObstacle);
    }

    public bool CheckFront() => Physics2D.Raycast(wallChecker.position, Vector2.down, 5f, whatIsGroundAndWall);
    #endregion

    public virtual void FindPlayerEvt(Action action)
    {
        //이미 감지됬었다면 걍 액션 인보크 시켜주고 리턴
        if (playerDetected == true)
        {
            action?.Invoke();
            return;
        }

        
        SpriteRenderer sr = findPlayerMark.GetComponent<SpriteRenderer>();
        sr.color = new Color(1, 1, 1, 1);
        findPlayerMark.localScale = Vector3.one;

        //대충 느낌표로 찾았다 어필해주고 움직이기 시작
        findPlayerMark.DOScaleY(1, 0.5f)
            .SetDelay(0.3f)
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
        //풀링 하면 여기에다가 추가해주면 도미
        Destroy(gameObject);
    }

}
