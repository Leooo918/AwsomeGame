using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Skill/KingSlime/KingDash")]
public class KingDashSkillSO : SkillSO
{
    public float dashPower = 15f;
    public float dashTime = 0.7f;
    public float dashReadyDelay = 3f;
    public float dashAfterDelay = 5f;

    private void OnEnable()
    {
        skill = new KingDashSkill();
    }
}
