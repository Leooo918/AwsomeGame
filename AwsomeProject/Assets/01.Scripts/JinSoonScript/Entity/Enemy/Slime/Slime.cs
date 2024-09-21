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
    }

    protected override void Update()
    {
        base.Update();
        StateMachine.CurrentState.UpdateState();
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

    public override void AirBorn(float duration)
    {
        base.AirBorn(duration);
        if (IsDead) return;
        airBornDuration = duration;
        StateMachine.ChangeState(SlimeStateEnum.AirBorn);
    }

    public override void Dead(Vector2 dir)
    {

    }

    private void OnHit()
    {
        HitEvent?.Invoke();
        StateMachine.ChangeState(SlimeStateEnum.Idle);
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
        IsDead = true;
    }
}
