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
    public bool isInvincibleWhileDash = false;
    public bool isAttackWhileDash = false;

    private void OnEnable()
    {
        Skill = new PlayerDashSkill();
        PlayerDashSkill dash = Skill as PlayerDashSkill;
        dash.Init(dashPower.GetValue(), dashTime.GetValue());
        skillType = PlayerSkill.Dash;
    }
}
