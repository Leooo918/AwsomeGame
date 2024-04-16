using System;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "SO/EntitySkillSO")]
public class EntitySkillSO : ScriptableObject
{
    public List<SkillSO> skills;
}
