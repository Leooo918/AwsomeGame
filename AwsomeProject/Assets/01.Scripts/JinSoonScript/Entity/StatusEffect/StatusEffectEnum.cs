using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StatusEffectEnum
{
    PoorRecovery = 1,
    IncreaseKnockback = 2,
    Poison = 4,
    Petrification = 8,
    NatureSync = 16,
    Strength = 32
}

public enum StatusEffectType
{
    Buff,
    Debuff
}