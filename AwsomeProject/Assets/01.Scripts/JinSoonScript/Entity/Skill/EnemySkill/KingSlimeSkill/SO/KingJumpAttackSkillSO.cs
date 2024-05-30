using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Skill/KingSlime/KingJumpAttack")]
public class KingJumpAttackSkillSO : SkillSO
{
    public float kingSlimeAirTime = 4f;
    public float kingSlimeJumpHeight = 5f;

    private void OnEnable()
    {
        skill = new KingJumpAttackSkill();
    }
}
