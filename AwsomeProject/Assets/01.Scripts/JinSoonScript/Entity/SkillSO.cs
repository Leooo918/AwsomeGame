using UnityEngine;


public abstract class SkillSO : ScriptableObject
{
    public int id;

    public Skill Skill;
    public Stat SkillCoolTime;
    public PlayerSkill skillType;
}
