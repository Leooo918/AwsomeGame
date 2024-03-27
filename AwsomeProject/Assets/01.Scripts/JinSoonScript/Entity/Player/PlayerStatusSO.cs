using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public enum PlayerStat
{
    MaxHealth,
    Attack,
    AttackSpeed,
    CriticalPercent,
    MoveSpeed,
    GatheringSpeed,
    Defence,
    DashCoolTime
}

public enum PlayerSkill
{
    Dash
}

[CreateAssetMenu(menuName = "SO/Status/PlayerStatus")]
public class PlayerStatusSO : StatusSO
{
    public Dictionary<PlayerStat, Stat> statDic;
    public Dictionary<PlayerSkill, SkillSO> skillDic;

    [Header("PlayerSkillSetting")]
    public List<SkillSO> skills;

    protected void OnEnable()
    {
        statDic = new Dictionary<PlayerStat, Stat>();

        Type playerStatType = typeof(PlayerStat);

        foreach (PlayerStat statType in Enum.GetValues(typeof(PlayerStat)))
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
    }

    protected void Start()
    {
        foreach (PlayerSkill skillType in Enum.GetValues(typeof(PlayerSkill)))
        {
            foreach (var item in skills)
            {
                if (item.skillType == skillType)
                {
                    skillDic[skillType] = item;
                }
            }
        }
    }

    public Stat GetStatByEnum(PlayerStat playerStat)
    {
        return statDic[playerStat];
    }
}
