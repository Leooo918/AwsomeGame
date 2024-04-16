using System;
using System.Collections.Generic;
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
    public EntitySkill<SlimeSkillEnum> SkillSO { get; private set; }

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

        SkillSO = new EntitySkill<SlimeSkillEnum>();
        SkillSO.Init(EntitySkillSO);
        
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

        foreach (var item in EntitySkillSO.skills)
        {
            item.skill.SetOwner(this);
            Type type = item.skill.GetType();
            gameObject.AddComponent(type);
        }

        Debug.Log("여기 고쳐요!!!");
        //moveSpeed = Stat.MoveSpeed.GetValue();
        //PatrolDelay = Stat.PatrolDelay;
        //PatrolTime = Stat.PatrolTime;
        //detectingDistance = Stat.DetectingDistance;

        pivot = hpBar.Find("Pivot");
    }

    private void OnEnable()
    {
        healthCompo.onKnockBack += KnockBack;
        healthCompo.onHit += OnHit;
        healthCompo.onDie += OnDie;
    }

    private void OnDisable()
    {
        healthCompo.onKnockBack -= KnockBack;
        healthCompo.onHit -= OnHit;
        healthCompo.onDie -= OnDie;
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

        float hpPercentage = (float)healthCompo.curHp / healthCompo.maxHp.GetValue();
        hpBar.localScale = new Vector3(FacingDir * hpBar.localScale.x, hpBar.localScale.y, hpBar.localScale.z);
        pivot.localScale = new Vector3(hpPercentage, 1, 1);
    }

    public void AnimationFinishTrigger() => StateMachine.CurrentState.AnimationFinishTrigger();

    public override void KnockBack(Vector2 power)
    {
        StopImmediately(true);
        if (knockbackCoroutine != null) StopCoroutine(knockbackCoroutine);

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
        //준비된 스킬중 Peek의 스킬을 사용하고 쿨타임 돌려주고 준비된 스킬에 이녀석은 이제 없다.
        SkillSO skill = readySkill.Peek();
        if (skill == null)
        {
            StateMachine.ChangeState(SlimeEnum.Idle);
            return;
        }

        skill.skill.UseSkill();
        notReady.Add(new Tuple<SkillSO, float>(skill, Time.time));
        readySkill.Pop();
        attackDistance = 0;
    }

    public void Jump()
    {

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
        StateMachine.ChangeState(SlimeEnum.Chase);
    }

    private void OnDie(Vector2 dir)
    {
        isDead = true;
        for (int i = 0; i < Stat.dropItems.Count; i++)
        {
            if (UnityEngine.Random.Range(0, 101) < Stat.dropItems[i].appearChance)
            {
                DropItem dropItem = Instantiate(Stat.dropItems[i].dropItem).GetComponent<DropItem>();
                dropItem.transform.position = transform.position + Vector3.up;
                dropItem.SpawnItem(dir);
            }
        }


        StateMachine.ChangeState(SlimeEnum.Dead);
    }
}
