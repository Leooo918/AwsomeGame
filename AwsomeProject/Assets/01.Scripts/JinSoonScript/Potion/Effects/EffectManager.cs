using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EffectTypeEnum
{
    Petrification,
    Growing,
    Heal,
    PoorRecovery, //�� ����
    Stun, //��ȭ
    Slowdown, //�̼� ����
    Fragile, //�޴� ������ ����
    DotDeal, //��Ʈ��
    Brainwash, //�Ʊ� ����(����)
    Floating, //����
    Weak, //������ ����
    IncreaseKnockback, //�˹� ����
    NatureSync, //�ڿ���ȭ
    Strength, //��
    Resistance, //�޴� ������ ����
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