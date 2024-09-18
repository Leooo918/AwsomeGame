using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WildBoarEnum
{
    Idle,
    Move,
    Rush,
    Groggy,
    Stun,
    AirBorn,
    Dead,
}

public enum WildBoarSkillEnum
{
    Rush
}

public class WildBoar : Enemy<WildBoarEnum>
{
    public WildBoarSkill Skills { get; private set; }

    public GameObject dashAttackCollider; 
    private Transform _playerTrm;

    private SkillSO _rushSkill;

    private float _attackDelay;
    public void SetAttackDelay(float delay) => _attackDelay = Time.time + delay;


    protected override void Awake()
    {
        base.Awake();
        _playerTrm = PlayerManager.Instance.PlayerTrm;

        Skills = gameObject.AddComponent<WildBoarSkill>();
        Skills.Init(EntitySkillSO);

        foreach (var item in EntitySkillSO.skills)
        {
            item.skill.SetOwner(this);
        }

        detectingDistance = EnemyStat.detectingDistance.GetValue();
        _rushSkill = Skills.GetSkillByEnum(WildBoarSkillEnum.Rush);
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

    private void Start()
    {
        StateMachine.Initialize(WildBoarEnum.Idle, this);
        attackDistance = Skills.GetSkillByEnum(WildBoarSkillEnum.Rush).attackDistance.GetValue();
    }

    protected override void Update()
    {
        base.Update();
        StateMachine.CurrentState.UpdateState();
    }

    public void AnimationFinishTrigger() => StateMachine.CurrentState.AnimationFinishTrigger();

    public override void Stun(float duration)
    {
        if (IsDead) return;
        stunDuration = duration;
        StateMachine.ChangeState(WildBoarEnum.Stun);
    }

    public override void Dead(Vector2 dir) { }

    public void TryAttack()
    {
        float dist = (transform.position - _playerTrm.position).magnitude;

        if (Time.time > _attackDelay && dist < _rushSkill.attackDistance.GetValue())
        {
            _rushSkill.skill.UseSkill();
        }
        else if(dist < attackDistance)
        {
            //_rushSkill.skill.UseSkill();
        }
    }

    public override void AirBorn(float duration)
    {
        base.AirBorn(duration);
        if (IsDead) return;
        airBornDuration = duration;
        StateMachine.ChangeState(WildBoarEnum.AirBorn);
    }

    public void Attack()
    {

    }

    private void OnHit()
    {
        HitEvent?.Invoke();
        StateMachine.ChangeState(WildBoarEnum.Idle);
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
        StateMachine.ChangeState(WildBoarEnum.Dead);
        CanStateChangeable = false;
        IsDead = true;
    }
}
