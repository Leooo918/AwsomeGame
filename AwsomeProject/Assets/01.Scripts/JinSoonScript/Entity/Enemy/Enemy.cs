using DG.Tweening;
using System;
using UnityEngine;
public abstract class Enemy<T> : Entity where T : Enum
{
    [SerializeField] private EnemyStatSO enemyStat;
    public EnemyStateMachine<T> StateMachine { get; private set; }
    public EnemyStatSO EnemyStat => enemyStat;

    #region EnemyStat
    public float moveSpeed { get; protected set; }
    public float PatrolTime { get; protected set; }
    public float PatrolDelay { get; protected set; }
    public float detectingDistance { get; protected set; }
    public float attackDistance { get; protected set; }

    #endregion

    #region Settings

    [SerializeField] protected Transform findPlayerMark;

    [SerializeField] protected LayerMask whatIsPlayer;
    [SerializeField] protected LayerMask whatIsObstacle;

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
        enemyStat = ScriptableObject.Instantiate(enemyStat);

        StateMachine = new EnemyStateMachine<T>();
        foreach (T stateEnum in Enum.GetValues(typeof(T)))
        {
            string typeName = stateEnum.ToString();
            string scriptName = GetType().ToString();
            Type t = Type.GetType($"{scriptName}{typeName}State");

            try
            {
                var enemyState = Activator.CreateInstance(type: t, this, StateMachine, typeName) as EnemyState<T>;
                StateMachine.AddState(stateEnum, enemyState);
            }
            catch (Exception e)
            {
                Debug.LogError($"Enemy {scriptName} : no state [ {typeName} ]");
                Debug.LogError(e);
            }
        }
    }

    #region DetectRegion

    public virtual Player IsPlayerDetected()
    {
        Collider2D player = Physics2D.OverlapCircle(transform.position, detectingDistance, whatIsPlayer);
        Debug.Log(player + " " + detectingDistance);
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
        Vector2 dir = ((PlayerManager.Instance.PlayerTrm.position + Vector3.up) - transform.position).normalized;

        return Physics2D.Raycast(transform.position, dir, distance, whatIsObstacle);
    }

    public bool CheckFront() => Physics2D.Raycast(wallChecker.position, Vector2.down, 5f, whatIsGroundAndWall);
    #endregion

    public virtual void FindPlayerEvt(Action action)
    {
        //�̹� ��������ٸ� �� �׼� �κ�ũ �����ְ� ����
        if (playerDetected == true)
        {
            action?.Invoke();
            return;
        }


        SpriteRenderer sr = findPlayerMark.GetComponent<SpriteRenderer>();
        sr.color = new Color(1, 1, 1, 1);
        findPlayerMark.localScale = Vector3.one;

        //���� ����ǥ�� ã�Ҵ� �������ְ� �����̱� ����
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
        //Ǯ�� �ϸ� ���⿡�ٰ� �߰����ָ� ����
        Destroy(gameObject);
    }

}
