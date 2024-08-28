using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/SKill/GoatSkill/LongDistAttack")]
public class GoatLongDistAttackSO : SkillSO
{
    public float throwingPower;


    private void OnEnable()
    {
        skill = new GoatLongDistAttack();
    }
}
