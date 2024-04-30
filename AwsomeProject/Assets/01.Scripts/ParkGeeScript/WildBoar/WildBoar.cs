using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WildBoarEnum
{
    Idle,
    Ready,
    Rush,
    Groggy,
    Dead
}

public enum WildBoarSkillEnum
{
    Rush
}

public class WildBoar : Enemy
{
    public WildBoarStatusSO wildBoarStatus { get; protected set; }
    public EnemyStateMachine<WildBoarEnum> StateMachine { get; private set; }

    public Stack<SkillSO> readySkill = new Stack<SkillSO>();
    public List<Tuple<SkillSO, float>> notReady = new List<Tuple<SkillSO, float>>();

    [HideInInspector] public bool moveAnima = false;
    [HideInInspector] public bool readyFlip = false;

    [SerializeField] private Transform hpBar;
    private Transform pivot;

    protected override void Awake()
    {
        base.Awake();
        wildBoarStatus = enemyStatus as WildBoarStatusSO;
        StateMachine = new EnemyStateMachine<WildBoarEnum>();

        foreach(WildBoarEnum stateEnum in Enum.GetValues(typeof(WildBoarEnum)))
        {
            string typeName = stateEnum.ToString();
            Type t = Type.GetType($"WildBoar{typeName}State");
            try
            {
                var enemyState = Activator.CreateInstance(t, this, StateMachine, typeName)
                    as EnemyState<WildBoarEnum>;
                StateMachine.AddState(stateEnum, enemyState);
            }
            catch(Exception e)
            {
                Debug.LogError($"Enemy WildBoar : no State {typeName}");
                Debug.LogError(e);
            }
        }

        foreach (var item in wildBoarStatus.skillDic)
        {
            item.Value.skill.SetOwner(this);
            Type type = item.Value.skill.GetType();
            gameObject.AddComponent(type);
        }

        moveSpeed = wildBoarStatus.MoveSpeed.GetValue();
        detectingDistance = wildBoarStatus.DetectingDistance;

        pivot = hpBar.Find("Pivot");
    }

    private void OnEnable()
    {
        healthCompo.onHit += OnHit;
        healthCompo.onDie += OnDie;
    }

    private void OnDisable()
    {
        healthCompo.onHit -= OnHit;
        healthCompo.onDie -= OnDie;
    }

    private void Start()
    {
        StateMachine.Initialize(WildBoarEnum.Idle, this);
    }

    private void Update()
    {
        StateMachine.CurrentState.UpdateState();

        for (int i = 0; i<notReady.Count; ++i)
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

        float hpPercentage = (float)healthCompo.curHp / healthCompo.maxHp.GetValue();
        hpBar.localScale = new Vector3(FacingDir * hpBar.localScale.x, hpBar.localScale.y, hpBar.localScale.z);
        pivot.localScale = new Vector3(hpPercentage, 1, 1);
    }

    public void AnimationFinishTrigger() => StateMachine.CurrentState.AnimationFinishTrigger();

    public override void Dead(Vector2 dir)
    {

    }

    public void Attack()
    {
        SkillSO skill = readySkill.Peek();
        if(skill == null)
        {
            StateMachine.ChangeState(WildBoarEnum.Idle);
            return;
        }

        skill.skill.UseSkill();
        notReady.Add(new Tuple<SkillSO, float>(skill, Time.time));
        readySkill.Pop();
        attackDistance = 0;
    }

    private void OnHit()
    {
        HitEvent?.Invoke();
        StateMachine.ChangeState(WildBoarEnum.Ready);
    }

    private void OnDie(Vector2 dir)
    {
        isDead = true;
        for(int i = 0; i<wildBoarStatus.dropItems.Count; i++)
        {
            if (UnityEngine.Random.Range(0, 101) < wildBoarStatus.dropItems[i].perecentage)
            {
                DropItem dropItem = Instantiate(wildBoarStatus.dropItems[i].dropItemPf).GetComponent<DropItem>();
                dropItem.transform.position = transform.position + Vector3.up;
                dropItem.SpawnItem(dir);
            }
        }

        StateMachine.ChangeState(WildBoarEnum.Dead);
    }
}
