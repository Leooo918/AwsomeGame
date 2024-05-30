using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Skill/KingSlime/KingMucusAttack")]
public class KingMucusAttackSkillSO : SkillSO
{
    public GameObject mucusPf;
    public float mucusFirePower = 7f;

    private void OnEnable()
    {
        skill = new KingMucusAttackSkill();
    }
}
