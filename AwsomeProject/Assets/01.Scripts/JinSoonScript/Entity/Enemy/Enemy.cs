using DG.Tweening;
using System;
using UnityEngine;
public abstract class Enemy<T> : Entity where T : Enum
{
    public EnemyStateMachine<T> StateMachine { get; private set; }
    public EnemyStatSO EnemyStat { get; private set; }

    #region EnemyStat
    public float PatrolTime { get => EnemyStat.patrolTime.GetValue(); protected set => EnemyStat.patrolTime.SetDefaultValue(value); }
    public float PatrolDelay { get => EnemyStat.patrolDelay.GetValue(); protected set => EnemyStat.patrolDelay.SetDefaultValue(value); }
    public float detectingDistance { get => EnemyStat.detectingDistance.GetValue(); protected set => EnemyStat.detectingDistance.SetDefaultValue(value); }
    public float attackDistance { get => EnemyStat.attackDistance.GetValue(); protected set => EnemyStat.attackDistance.SetDefaultValue(value); }

    #endregion

    #region Settings

    [SerializeField] protected LayerMask whatIsPlayer;
    [SerializeField] protected LayerMask whatIsObstacle;

    [Header("Attack Settings")]
    public float runAwayDistance;
    [HideInInspector] public float lastAttackTime;

    #endregion

    public float defaultMoveSpeed { get; protected set; }
    protected int lastAnimationBoolHash;

    private bool playerDetected = false;

    protected override void Awake()
    {
        base.Awake();
        EnemyStat = Stat as EnemyStatSO;
        defaultMoveSpeed = EnemyStat.moveSpeed.GetValue();

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


    /// <summary>
    /// �÷��̾ �ٶ󺸰�, detectingDistance ��ŭ ������ ������
    /// </summary>
    /// <returns></returns>
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

    /// <summary>
    /// �÷��̾���� ������ �����ִ���
    /// </summary>
    /// <param name="distance">�÷��̾������ �Ÿ�</param>
    /// <returns></returns>
    public virtual bool IsObstacleInLine(float distance)
    {
        Vector2 dir = ((PlayerManager.Instance.PlayerTrm.position + Vector3.up) - transform.position).normalized;

        return Physics2D.Raycast(transform.position, dir, distance, whatIsObstacle);
    }

    /// <summary>
    /// ������� ���ʿ� �ٴ����ִ��� Ȯ��
    /// </summary>
    /// <returns></returns>
    public bool CheckFront() => Physics2D.Raycast(wallChecker.position, Vector2.down, 5f, whatIsGroundAndWall);
    #endregion

    public void MissPlayer() => playerDetected = false;

    public void OnCompletelyDie()
    {
        //Ǯ�� �ϸ� ���⿡�ٰ� �߰����ָ� ����
        StopAllCoroutines();
        Destroy(gameObject);
    }
}
