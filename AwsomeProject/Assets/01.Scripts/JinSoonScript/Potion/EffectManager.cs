using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EffectTypeEnum
{
    Petrification,
}

public class EffectManager : Singleton<EffectManager>
{
    private Dictionary<EffectTypeEnum, Func<Effect>> _effectDictionary;

    private void Awake()
    {
        _effectDictionary = new Dictionary<EffectTypeEnum, Func<Effect>>();

        Add(EffectTypeEnum.Petrification, () => new PetrificationEffect());
    }

    private void Add(EffectTypeEnum effectEnum, Func<Effect> effect)
    {
        _effectDictionary.Add(effectEnum, effect);
    }

    public Effect GetEffect(EffectTypeEnum effectEnum)
    {
        if (_effectDictionary.TryGetValue(effectEnum, out Func<Effect> effect))
        {
            Effect e = effect.Invoke();
            return e;
        }
        return null;
    }
}
