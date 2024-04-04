using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public enum StatEnum
{
    MaxHealth,
    Attack,
    AttackSpeed,
    CriticalPercent,
    MoveSpeed,
    GatheringSpeed,
    Defence
}

public enum PlayerSkill
{
    Dash,
    NormalAttack
}

[CreateAssetMenu(menuName = "SO/Status/PlayerStatus")]
public class PlayerStatusSO : StatusSO
{
    public Stat GatheringSpeed;

    public Dictionary<StatEnum, Stat> statDic = new Dictionary<StatEnum, Stat>();
    public Dictionary<PlayerSkill, SkillSO> skillDic = new Dictionary<PlayerSkill, SkillSO>();

    [Header("PlayerSkillSetting")]
    public List<SkillSO> skills;

    protected void OnEnable()
    {
        statDic = new Dictionary<StatEnum, Stat>();

        Type playerStatType = typeof(PlayerStatusSO);
        foreach (StatEnum statType in Enum.GetValues(typeof(StatEnum)))
        {
            string fieldName = statType.ToString();

            try
            {
                FieldInfo playerStatField = playerStatType.GetField(fieldName);
                Stat stat = playerStatField.GetValue(this) as Stat;

                statDic.Add(statType, stat);
            }
            catch (Exception ex)
            {
                Debug.LogError($"There are no stat field in player: {fieldName}, msg:{ex.Message}");
            }
        }

        foreach (PlayerSkill skillType in Enum.GetValues(typeof(PlayerSkill)))
        {
            foreach (var item in skills)
            {
                if (item.skillName == skillType.ToString())
                {
                    skillDic[skillType] = item;
                }
            }
        }
    }

    public Stat GetStatByEnum(StatEnum playerStat)
    {
        return statDic[playerStat];
    }

    public SkillSO GetSkillByEnum(PlayerSkill skillType)
    {
        return skillDic[skillType];
    }
}
