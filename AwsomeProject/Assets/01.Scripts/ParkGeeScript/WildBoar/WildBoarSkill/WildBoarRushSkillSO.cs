using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Skill/WildBoar/WildBoarRush")]
public class WildBoarRushSkillSO : SkillSO
{
    [Header("WildBoarInfo")]
    public Stat rushSpeed;
    public Stat damage;

    private void OnEnable()
    {
        skill = new WildBoarRushSkill();
    }
}
