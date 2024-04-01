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
    public bool canUseSkill = false;
    public bool isInvincibleWhileDash = false;
    public bool isAttackWhileDash = false;

    private void OnEnable()
    {
        skill = new PlayerDashSkill();
        PlayerDashSkill dash = skill as PlayerDashSkill;
        dash.Init(dashPower.GetValue(), dashTime.GetValue(), canUseSkill, isInvincibleWhileDash, isAttackWhileDash);
    }
}
