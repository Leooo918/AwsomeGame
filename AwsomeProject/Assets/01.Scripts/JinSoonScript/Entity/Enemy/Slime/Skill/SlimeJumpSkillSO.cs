using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Skill/Slime/SlimeJump")]
public class SlimeJumpSkillSO : SkillSO
{
    [Header("SlimeJumpInfo")]
    public Stat jumpPower;
    public Stat damage;

    private void OnEnable()
    {
        skill = new SlimeJumpAttackSkill();
    }
}
