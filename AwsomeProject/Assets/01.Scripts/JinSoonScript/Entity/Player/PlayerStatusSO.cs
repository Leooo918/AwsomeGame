using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Status/PlayerStatus")]
public class PlayerStatusSO : StatusSO
{
    [Header("PlayerSkillSetting")]
    public Stat dashCoolTime;
}
