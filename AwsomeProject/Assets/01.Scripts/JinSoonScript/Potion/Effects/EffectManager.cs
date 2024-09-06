using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EffectTypeEnum
{
    Petrification,
    Growing,
}

public static class EffectManager
{
    private static readonly Dictionary<EffectTypeEnum, Func<Effect>> _effectDictionary;

    static EffectManager()
    {
        _effectDictionary = new Dictionary<EffectTypeEnum, Func<Effect>>
        {
            { EffectTypeEnum.Petrification, () => new PetrificationEffect() },
            { EffectTypeEnum.Growing, () => new GrowingEffect() }
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