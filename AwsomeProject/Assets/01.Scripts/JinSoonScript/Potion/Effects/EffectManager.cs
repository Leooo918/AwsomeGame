using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EffectTypeEnum
{
    Petrification,
    Growing,
    Heal,
    PoorRecovery, //힐 감소
    Stun, //석화
    Slowdown, //이속 감소
    Fragile, //받는 데미지 증가
    DotDeal, //도트딜
    Brainwash, //아군 공격(세뇌)
    Floating, //부유
    Weak, //데미지 감소
    IncreaseKnockback, //넉백 증가
    NatureSync, //자연동화
    Strength, //힘
    Resistance, //받는 데미지 감소
}

public static class EffectManager
{
    private static readonly Dictionary<EffectTypeEnum, Func<Effect>> _effectDictionary;

    static EffectManager()
    {
        _effectDictionary = new Dictionary<EffectTypeEnum, Func<Effect>>
        {
            { EffectTypeEnum.Petrification, () => new PetrificationEffect() },
            { EffectTypeEnum.Growing, () => new GrowingEffect() },
            { EffectTypeEnum.Heal, () => new HealEffect() }
        };
    }

    public static Effect GetEffect(EffectTypeEnum effectEnum)
    {
        if (_effectDictionary.TryGetValue(effectEnum, out Func<Effect> effect))
        {
            return effect.Invoke();
        }
        return null;
    }
}