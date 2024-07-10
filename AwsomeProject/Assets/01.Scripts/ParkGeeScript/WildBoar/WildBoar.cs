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
    Stun,
    Dead
}

public enum WildBoarSkillEnum
{
    Rush
}

public class WildBoar : Enemy<WildBoarEnum>
{
    //public WildBoarStatusSO wildBoarStatus { get; protected set; }
    public WildBoarSkill Skills { get; private set; }

    public Stack<SkillSO> readySkill = new Stack<SkillSO>();
    public List<Tuple<SkillSO, float>> notReady = new List<Tuple<SkillSO, float>>();

    [HideInInspector] public bool moveAnima = false;
    [HideInInspector] public bool readyFlip = false;

    [SerializeField] private Transform hpBar;
    private Transform pivot;

    protected override void Awake()
    {
        base.Awake();
        Skills = gameObject.AddComponent<WildBoarSkill>();
        Skills.Init(EntitySkillSO);

        foreach (var item in EntitySkillSO.skills)
        {
            item.skill.SetOwner(this);
            Type type = item.skill.GetType();
            gameObject.AddComponent(type);
        }

        moveSpeed = Stat.moveSpeed.GetValue();
        detectingDistance = EnemyStat.detectingDistance.GetValue();

        //pivot = hpBar.Find("Pivot");
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

        //float hpPercentage = (float)healthCompo.curHp / healthCompo.maxHp.GetValue();
        //hpBar.localScale = new Vector3(FacingDir * hpBar.localScale.x, hpBar.localScale.y, hpBar.localScale.z);
        //pivot.localScale = new Vector3(hpPercentage, 1, 1);
    }

    public void AnimationFinishTrigger() => StateMachine.CurrentState.AnimationFinishTrigger();

    public override void Stun(float duration)
    {
        if (isDead) return;
        stunDuration = duration;
        StateMachine.ChangeState(WildBoarEnum.Stun);
    }

    public override void Dead(Vector2 dir)
    {
        StateMachine.ChangeState(WildBoarEnum.Dead);
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

    public void SkillStack()
    {
        List<SkillSO> skills = EntitySkillSO.skills;

        readySkill.Clear();
        for (int i = 0; i < skills.Count; i++)
        {
            //쿨타임중이면 공격스택에 넣지말고
            if (readySkill.Contains(skills[i])) continue;

            //쿨타임이 아닌 녀석들만 스택에 넣어두어라
            readySkill.Push(skills[i]);
        }
        if (readySkill.Peek() == null) return;

        attackDistance = readySkill.Peek().attackDistance.GetValue();
    }

    private void OnHit()
    {
        HitEvent?.Invoke();
        StateMachine.ChangeState(WildBoarEnum.Rush);
    } 

    private void OnDie(Vector2 dir)
    {
        isDead = true;
        for(int i = 0; i< EnemyStat.dropItems.Count; i++)
        {
            if (UnityEngine.Random.Range(0, 101) < EnemyStat.dropItems[i].appearChance)
            {
                DropItem dropItem = Instantiate(EnemyStat.dropItems[i].dropItemPf).GetComponent<DropItem>();
                dropItem.transform.position = transform.position + Vector3.up;
                dropItem.SpawnItem(dir);
            }
        }

        StateMachine.ChangeState(WildBoarEnum.Dead);
    }
}
