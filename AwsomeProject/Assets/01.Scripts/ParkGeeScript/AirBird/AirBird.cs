using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AirBirdEnum
{
    Idle,
    Patrol,
    Chase,
    Shoot,
    Stun,
    Dead
}

public enum AirBirdSkillEnum
{
    Shoot
}

public class AirBird : Enemy<AirBirdEnum>
{
    public AirBirdSkill Skills { get; private set; }

    public Stack<SkillSO> readySkill = new Stack<SkillSO>();
    public List<Tuple<SkillSO, float>> notReady = new List<Tuple<SkillSO, float>>();

    [HideInInspector] public bool moveAnima = false;
    [HideInInspector] public bool readyFlip = false;

    [SerializeField] private Transform hpBar;
    private Transform pivot;

    protected override void Awake()
    {
        base.Awake();
        Skills = gameObject.AddComponent<AirBirdSkill>();
        Skills.Init(EntitySkillSO);

        foreach (var item in EntitySkillSO.skills)
        {
            item.skill.SetOwner(this);
            Type type = item.skill.GetType();
            gameObject.AddComponent(type);
        }

        moveSpeed = Stat.moveSpeed.GetValue();
        detectingDistance = EnemyStat.detectingDistance.GetValue();

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
        StateMachine.Initialize(AirBirdEnum.Idle, this);
    }

    public void AnimationFinishTrigger() => StateMachine.CurrentState.AnimationFinishTrigger();

    public override void Stun(float duration)
    {
        if (IsDead) return;
        stunDuration = duration;
        StateMachine.ChangeState(AirBirdEnum.Stun);
    }

    public override void Dead(Vector2 dir)
    {
    }

    public void Attack()
    {
        SkillSO skill = readySkill.Peek();
        if (skill == null)
        {
            StateMachine.ChangeState(AirBirdEnum.Idle);
            return;
        }

        skill.skill.UseSkill();
        notReady.Add(new Tuple<SkillSO, float>(skill, Time.time));
        readySkill.Pop();
        attackDistance = 0;
    }

    public void ShuffleSkillStack()
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

        readySkill.Clear();
        for (int i = 0; i < skills.Count; i++)
        {
            if (readySkill.Contains(skills[i])) continue;

            readySkill.Push(skills[i]);
        }
        if (readySkill.Peek() == null) return;

        attackDistance = readySkill.Peek().attackDistance.GetValue();
    }

    private void OnHit()
    {
        HitEvent?.Invoke();
        StateMachine.ChangeState(AirBirdEnum.Shoot);
    }

    private void OnDie(Vector2 dir)
    {
        IsDead = true;
        for (int i = 0; i < EnemyStat.dropItems.Count; i++)
        {
            if (UnityEngine.Random.Range(0, 101) < EnemyStat.dropItems[i].appearChance)
            {
                DropItem dropItem = Instantiate(EnemyStat.dropItems[i].dropItemPf).GetComponent<DropItem>();
                dropItem.transform.position = transform.position + Vector3.up;
                dropItem.SpawnItem(dir);
            }
        }

        StateMachine.ChangeState(AirBirdEnum.Dead);
    }
}
