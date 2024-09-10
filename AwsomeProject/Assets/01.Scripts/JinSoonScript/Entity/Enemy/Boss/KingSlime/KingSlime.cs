using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum KingSlimeStateEnum
{
    Disable,
    Ready,
    JumpAttack,
    Dash,
    MucusAttack,
    Vined,          //덩굴에 얽힘
    Stun,
    Dead
}

public enum KingSlimeSkillEnum
{
    KingJumpAttack,
    KingMucusAttack,
    KingDash
}



public class KingSlime : Enemy<KingSlimeStateEnum>, IBoss
{
    public KingSlimeSkill Skills { get; private set; }
    bool IBoss.IsBossDead { get; set; }
    public CinemachineVirtualCamera _bossRoomCam { get; set; }
    public CinemachineVirtualCamera _bossWatchingCam { get; set; }

    #region Skill

    public Queue<SkillSO> readySkill = new Queue<SkillSO>();
    public List<Tuple<SkillSO, float>> notReady = new List<Tuple<SkillSO, float>>();
    public GameObject mucus;

    #endregion

    public FallingRockSpawner spanwer;

    [HideInInspector] public bool _readyFlip = false;
    private float _currentSkillAfterDelay;
    private bool _canUseSkill = true;
    private BossHealth _bossHealth;
    private BossHpBarUI _bossHpBar;

    protected override void Awake()
    {
        base.Awake();

        _bossHealth = healthCompo as BossHealth;
        _bossHealth.OnChangePhase += GoToNextPhase;
        StateMachine.Initialize(KingSlimeStateEnum.Ready, this);
        Skills = gameObject.AddComponent<KingSlimeSkill>();
        Skills.Init(EntitySkillSO);

        foreach (var item in EntitySkillSO.skills)
        {
            item.skill.SetOwner(this);
            //Type type = item.skill.GetType();
            //gameObject.AddComponent(type);
        }

        moveSpeed = Stat.moveSpeed.GetValue();
        detectingDistance = EnemyStat.detectingDistance.GetValue();
        _bossHpBar = UIManager.Instance.panelDictionary[UIType.BossHp] as BossHpBarUI;
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
        ShuffleSkillStack();
    }

    private void Update()
    {
        StateMachine.CurrentState.UpdateState();
        //float hpPercentage = (float)healthCompo.curHp / healthCompo.maxHp.GetValue();
        //_hpBar.localScale = new Vector3(FacingDir * _hpBar.localScale.x, _hpBar.localScale.y, _hpBar.localScale.z);
        //_pivot.localScale = new Vector3(hpPercentage, 1, 1);
    }

    private void LateUpdate()
    {
        for (int i = 0; i < notReady.Count; ++i)
        {
            var item = notReady[i];
            if (item.Item2 + item.Item1.skillCoolTime.GetValue() < Time.time)
            {
                notReady.RemoveAt(i--);
                if (readySkill.Count <= 0) attackDistance = item.Item1.attackDistance.GetValue();
                readySkill.Enqueue(item.Item1);
            }
        }
    }

    #region SkillSection

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
            readySkill.Enqueue(skills[i]);
        }
        if (readySkill.Peek() == null) return;

        attackDistance = readySkill.Peek().attackDistance.GetValue();
    }

    public void UseSkill()
    {
        //현재 스킬을 못쓴다면 return
        if (_canUseSkill == false) return;

        //준비된 스킬중 Peek의 스킬을 사용하고 쿨타임 돌려주고 준비된 스킬에 이녀석은 이제 없다.
        if (readySkill.TryPeek(out SkillSO skill) == false || skill == null)
        {
            if (skill != null)
            {
                readySkill.Dequeue();
                readySkill.Enqueue(skill);
            }
            StateMachine.ChangeState(KingSlimeStateEnum.Ready);
            return;
        }

        //스킬의 공격 범위에 플레이어가 없다면
        attackDistance = skill.attackDistance.GetValue();
        Player player = PlayerManager.Instance.Player;
        if (player == null)
        {
            StateMachine.ChangeState(KingSlimeStateEnum.Ready);
            readySkill.Dequeue();
            readySkill.Enqueue(skill);
            return;
        }

        skill.skill.UseSkill();
        _currentSkillAfterDelay = skill.skillAfterDelay.GetValue();
        notReady.Add(new Tuple<SkillSO, float>(skill, Time.time));
        readySkill.Dequeue();
    }

    public void SetSkillAfterDelay()
    {
        _canUseSkill = false;
        StartDelayCallBack(_currentSkillAfterDelay, () => _canUseSkill = true);
    }

    #endregion

    #region HealthSection

    private void OnHit()
    {
        HitEvent?.Invoke();
        StateMachine.ChangeState(KingSlimeStateEnum.Ready);
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


        StateMachine.ChangeState(KingSlimeStateEnum.Dead);
    }

    public override void Stun(float duration)
    {
        if (IsDead) return;

        stunDuration = duration;
        StateMachine.ChangeState(KingSlimeStateEnum.Stun);
    }

    #endregion

    public void AnimationFinishTrigger() => StateMachine.CurrentState.AnimationFinishTrigger();


    [SerializeField] private Vector2 left, right;
    public Vector3 GetJumpPos()
    {
        float leftDir = Mathf.Abs(left.x - transform.position.x);
        float rightDir = Mathf.Abs(right.x - transform.position.x);

        if (leftDir < 1)
            return right;
        else if (rightDir < 1)
            return left;
        else
        {
            if (leftDir < rightDir)
                return left;
            else
                return right;
        }

    }
    

    #region BossSection

    public void StartBoss()
    {
        StartCoroutine(EnableBossRoutine());
        _bossHpBar.SetOwner(healthCompo);
        healthCompo.OnHit += _bossHpBar.SetEnemyHealth;
    }

    public void EndBoss()
    {
        healthCompo.OnHit -= _bossHpBar.SetEnemyHealth;
    }


    public void GoToNextPhase(int phase)
    {
        if(phase == 1)
            StartCoroutine(ChangePhaseRoutine());
        Debug.Log("밍밍밍");
        Debug.Log($"{phase}페이지 밍밍");
    }

    private IEnumerator ChangePhaseRoutine()
    {
        yield return null;
    }

    private IEnumerator EnableBossRoutine()
    {
        PlayerManager.Instance.DisablePlayerMovementInput();
        PlayerManager.Instance.DisablePlayerInventoryInput();
        StateMachine.ChangeState(KingSlimeStateEnum.Disable);

        CameraManager.Instance.ChangeCam(_bossWatchingCam, false);
        CameraManager.Instance.ChangeFollow(transform);
        
        yield return new WaitForSeconds(0.5f);

        UIManager.Instance.Open(UIType.BossStageEnter);
        
        yield return new WaitForSeconds(2.3f);
        
        CameraManager.Instance.ChangeCam(_bossRoomCam, false);
        UIManager.Instance.Open(UIType.BossHp);


        yield return new WaitForSeconds(0.5f);

        PlayerManager.Instance.EnablePlayerMovementInput();
        PlayerManager.Instance.EnablePlayerInventoryInput();
        StateMachine.ChangeState(KingSlimeStateEnum.Ready);
    }

    #endregion
}
