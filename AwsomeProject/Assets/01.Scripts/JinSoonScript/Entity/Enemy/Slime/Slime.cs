using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum SlimeStateEnum
{
    Idle,
    Patrol,
    Chase,
    Return,
    JumpAttack,
    Stun,
    AirBorn,
    Dead
}

public enum SlimeSkillEnum
{
    JumpAttack
}

public class Slime : Enemy<SlimeStateEnum>
{
    public SlimeSkill Skills { get; private set; }

    [SerializeField] private GameObject _contactDamage;

    #region SkillSection

    public Stack<SkillSO> readySkill = new Stack<SkillSO>();
    public List<Tuple<SkillSO, float>> notReady = new List<Tuple<SkillSO, float>>();

    #endregion

    [HideInInspector] public bool moveAnim = false;


    protected override void Awake()
    {
        base.Awake();

        Skills = gameObject.AddComponent<SlimeSkill>();
        Skills.Init(EntitySkillSO);

        foreach (var item in EntitySkillSO.skills)
        {
            item.skill.SetOwner(this);
        }

        moveSpeed = Stat.moveSpeed.GetValue();
        PatrolTime = EnemyStat.patrolTime.GetValue();
        PatrolDelay = EnemyStat.patrolDelay.GetValue();
        detectingDistance = EnemyStat.detectingDistance.GetValue();
    }

    private void OnEnable()
    {
        healthCompo.OnKnockBack += KnockBack;
        healthCompo.OnHit += OnHit;
        healthCompo.OnDie += OnDie;
    }

    private void OnDisable()
    {
        healthCompo.OnKnockBack -= KnockBack;
        healthCompo.OnHit -= OnHit;
        healthCompo.OnDie -= OnDie;
    }

    protected void Start()
    {
        StateMachine.Initialize(SlimeStateEnum.Idle, this);

        ShuffleSkillStack();
    }

    protected override void Update()
    {
        base.Update();
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
    }

    public void AnimationFinishTrigger() => StateMachine.CurrentState.AnimationFinishTrigger();

    public override void Stun(float duration)
    {
        base.Stun(duration);
        if (IsDead) return;
        stunDuration = duration;
        StateMachine.ChangeState(SlimeStateEnum.Stun);
    }

    public override void Stone(float duration)
    {
        base.Stone(duration);
        if (IsDead) return;
        stunDuration = duration;
        StateMachine.ChangeState(SlimeStateEnum.Stun);
    }

    public override void AirBorn(float duration, int damagePercent)
    {
        base.AirBorn(duration, damagePercent);
        if (IsDead) return;
        airBornDuration = duration;
        StateMachine.ChangeState(SlimeStateEnum.AirBorn);
    }

    public override void Dead(Vector2 dir)
    {

    }

    public void Attack()
    {
        //준비된 스킬중 Peek의 스킬을 사용하고 쿨타임 돌려주고 준비된 스킬에 이녀석은 이제 없다.
        SkillSO skill = readySkill.Peek();
        if (skill == null)
        {
            StateMachine.ChangeState(SlimeStateEnum.Idle);
            return;
        }

        skill.skill.UseSkill();
        notReady.Add(new Tuple<SkillSO, float>(skill, Time.time));
        readySkill.Pop();
        attackDistance = 0;
    }
    private void ShuffleSkillStack()
    {
        List<SkillSO> skills = EntitySkillSO.skills;
        for (int i = 0; i < 10; i++)
        {
            int a = UnityEngine.Random.Range(0, skills.Count);
            int b = UnityEngine.Random.Range(0, skills.Count);

            SkillSO temp = skills[a];
            skills[a] = skills[b];
            skills[b] = temp;
        }

        //우선 공격스택을 다 비우고
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
        StateMachine.ChangeState(SlimeStateEnum.Chase);
    }

    private void OnDie(Vector2 dir)
    {
        for (int i = 0; i < EnemyStat.dropItems.Count; i++)
        {
            if (UnityEngine.Random.Range(0, 101) < EnemyStat.dropItems[i].appearChance)
            {
                DropItem dropItem = Instantiate(EnemyStat.dropItems[i].dropItemPf).GetComponent<DropItem>();
                dropItem.transform.position = transform.position + Vector3.up;
                dropItem.SpawnItem(dir);
            }
        }

        CanStateChangeable = true;
        StateMachine.ChangeState(SlimeStateEnum.Dead);
        CanStateChangeable = false;
        _contactDamage.SetActive(false);
        IsDead = true;
    }
}
