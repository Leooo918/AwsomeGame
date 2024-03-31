using UnityEngine;


public abstract class SkillSO : ScriptableObject
{
    public int id;

    [HideInInspector]
    public Skill skill;
    public Stat skillCoolTime;
    public Stat attackDistance;
    public string skillName;
}
