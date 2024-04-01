using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="SO/Skill/Player/PlayerNormalAttack")]
public class PlayerNormalAttackSO : SkillSO
{
    [Header("AttackInfos")]
    public Stat FirstAttackMultiplier;
    public float attackComboDragTime;
    public Stat SecondAttackMultiplier;

    private void OnEnable()
    {
        skill = new PlayerNormalAttack();
        PlayerNormalAttack attack = skill as PlayerNormalAttack;
        attack.Init(FirstAttackMultiplier, SecondAttackMultiplier);
    }
}