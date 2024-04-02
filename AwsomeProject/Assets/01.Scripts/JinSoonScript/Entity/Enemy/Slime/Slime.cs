using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum SlimeEnum
{
    Idle,
    Patrol,
    Return,
    Chase,
    JumpAttack,
    Stun,
    Dead
}

public enum SlimeSkillEnum
{
    JumpAttack
}

public class Slime : Enemy
{
    public SlimeStatusSO slimeStatus { get; protected set; }
    public EnemyStateMachine<SlimeEnum> StateMachine { get; private set; }

    #region SkillSection

    public Stack<SkillSO> readySkill = new Stack<SkillSO>();
    public List<Tuple<SkillSO, float>> notReady = new List<Tuple<SkillSO, float>>();

    #endregion

    [HideInInspector] public bool moveAnim = false;
    [HideInInspector] public bool readyFlip = false;

    #region HpBar

    [SerializeField] private Transform hpBar;
    private Transform pivot;

    #endregion

    protected override void Awake()
    {
        base.Awake();
        slimeStatus = enemyStatus as SlimeStatusSO;
        StateMachine = new EnemyStateMachine<SlimeEnum>();

        foreach (SlimeEnum stateEnum in Enum.GetValues(typeof(SlimeEnum)))
        {
            string typeName = stateEnum.ToString();
            Type t = Type.GetType($"Slime{typeName}State");
            try
            {
                var enemyState = Activator.CreateInstance(t, this, StateMachine, typeName) as EnemyState<SlimeEnum>;
                StateMachine.AddState(stateEnum, enemyState);
            }
            catch (Exception e)
            {
                Debug.LogError($"Enemy Slime : no state [ {typeName} ]");
                Debug.LogError(e);
            }
        }

        foreach (var item in slimeStatus.skillDic)
        {
            item.Value.skill.SetOwner(this);
            Type type = item.Value.skill.GetType();
            gameObject.AddComponent(type);
        }


        moveSpeed = slimeStatus.MoveSpeed.GetValue();
        PatrolDelay = slimeStatus.PatrolDelay;
        PatrolTime = slimeStatus.PatrolTime;
        detectingDistance = slimeStatus.DetectingDistance;

        pivot = hpBar.Find("Pivot");
    }

    private void OnEnable()
    {
        enemyHealth.onKnockBack += KnockBack;
        enemyHealth.onHit += () => HitEvent?.Invoke();
        enemyHealth.onHit += () => StateMachine.ChangeState(SlimeEnum.Chase);
    }

    private void OnDisable()
    {
        enemyHealth.onKnockBack -= KnockBack;
        enemyHealth.onHit -= () => HitEvent?.Invoke();
        enemyHealth.onHit -= () => StateMachine.ChangeState(SlimeEnum.Chase);
    }

    protected void Start()
    {
        StateMachine.Initialize(SlimeEnum.Idle, this);
        patrolEndTime = Time.time;

        ShuffleSkillStack();
    }

    private void Update()
    {
        StateMachine.CurrentState.UpdateState();

        for (int i = 0; i < notReady.Count; ++i)
        {
            var item = notReady[i];
            if (item.Item2 + item.Item1.skillCoolTime.GetValue() < Time.time)
            {
                notReady.Remove(item);
                if (readySkill.Count <= 0) attackDistance = item.Item1.attackDistance.GetValue();
                readySkill.Push(item.Item1);
                --i;
            }
        }

        float hpPercentage = enemyHealth.curHp / enemyHealth.maxHp.GetValue();
        pivot.localScale = new Vector3(hpPercentage, 1, 1);
    }

    public override void KnockBack(Vector2 power)
    {
        StopImmediately(true);
        if (knockbackCoroutine != null) StopCoroutine(knockbackCoroutine);

        Debug.Log(power);
        isKnockbacked = true;
        SetVelocity(power.x, power.y, true, true);
        knockbackCoroutine = StartDelayCallBack(
            knockbackDuration, () =>
            {
                isKnockbacked = false;
                StopImmediately(false);
            });
    }

    public override void Stun(float duration)
    {
        if (isDead) return;
        stunDuration = duration;
        StateMachine.ChangeState(SlimeEnum.Stun);
    }

    public override void Dead(Vector2 dir)
    {

    }

    public void Attack()
    {
        //�غ�� ��ų�� Peek�� ��ų�� ����ϰ� ��Ÿ�� �����ְ� �غ�� ��ų�� �̳༮�� ���� ����.
        SkillSO skill = readySkill.Peek();
        if (skill == null)
        {
            Debug.Log("��");
            StateMachine.ChangeState(SlimeEnum.Idle);
            return;
        }

        skill.skill.UseSkill();
        notReady.Add(new Tuple<SkillSO, float>(skill, Time.time));
        readySkill.Pop();
        attackDistance = 0;
    }

    private void ShuffleSkillStack()
    {
        List<SkillSO> skills = slimeStatus.skills;
        for (int i = 0; i < 10; i++)
        {
            int a = UnityEngine.Random.Range(0, slimeStatus.skills.Count);
            int b = UnityEngine.Random.Range(0, slimeStatus.skills.Count);

            SkillSO temp = skills[a];
            skills[a] = skills[b];
            skills[b] = temp;
        }

        //�켱 ���ݽ����� �� ����
        readySkill.Clear();
        for (int i = 0; i < skills.Count; i++)
        {
            //��Ÿ�����̸� ���ݽ��ÿ� ��������
            if (readySkill.Contains(skills[i])) continue;

            //��Ÿ���� �ƴ� �༮�鸸 ���ÿ� �־�ξ��
            readySkill.Push(skills[i]);
        }
        if (readySkill.Peek() == null) return;

        attackDistance = readySkill.Peek().attackDistance.GetValue();
    }
}
