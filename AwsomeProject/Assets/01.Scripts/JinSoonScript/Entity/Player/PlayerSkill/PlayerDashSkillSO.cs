using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Skill/Player/PlayerDashSkill")]
public class PlayerDashSkillSO : SkillSO
{
    public Stat dashPower;
    [Header("DevideBy10")]
    public Stat dashTime;

    [Space(16)]

    [Header("SpecialAbility")]
    [SerializeField] private bool canUseSkill = false;
    [SerializeField] private bool isInvincibleWhileDash = false;
    [SerializeField] private bool isAttackWhileDash = false;

    public bool CanUseSkill
    {
        get { return canUseSkill; }
        set
        {
            canUseSkill = value;
            PlayerDashSkill dash = skill as PlayerDashSkill;
            dash.Init(dashPower.GetValue(), dashTime.GetValue(), canUseSkill, isInvincibleWhileDash, isAttackWhileDash, skillCoolTime.GetValue());
        }
    }
    public bool IsInvincibleWhileDash
    {
        get
        {
            return isInvincibleWhileDash;
        }
        set
        {
            isInvincibleWhileDash = value;
            PlayerDashSkill dash = skill as PlayerDashSkill;
            dash.Init(dashPower.GetValue(), dashTime.GetValue(), canUseSkill, isInvincibleWhileDash, isAttackWhileDash, skillCoolTime.GetValue());
        }
    }
    public bool IsAttackWhileDash
    {
        get
        {
            return isAttackWhileDash;
        }
        set
        {
            isAttackWhileDash = value;
            PlayerDashSkill dash = skill as PlayerDashSkill;
            dash.Init(dashPower.GetValue(), dashTime.GetValue(), canUseSkill, isInvincibleWhileDash, isAttackWhileDash, skillCoolTime.GetValue());
        }
    }

    private void OnEnable()
    {
        skill = new PlayerDashSkill();
        PlayerDashSkill dash = skill as PlayerDashSkill;
        dash.Init(dashPower.GetValue(), dashTime.GetValue(), canUseSkill, isInvincibleWhileDash, isAttackWhileDash, skillCoolTime.GetValue());
    }
}
