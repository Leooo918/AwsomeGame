using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="SO/Skill/Player/PlayerNormalAttack")]
public class PlayerNormalAttackSO : SkillSO
{
    [Header("AttackInfos")]
    public AttackInfo firstAttackInfo;
    public AttackInfo secondAttackInfo;
    public float attackComboDragTime;

    private void OnEnable()
    {
        skill = new NormalAttackSkill();
    }
}

[System.Serializable]
public struct AttackInfo
{
    public float attackMultiplier;
    public Vector2 offset;
    public Vector2 knockBackPower;
    public float radius;
    [HideInInspector]public int damage;
}