using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EffectTypeEnum
{
    Damage,
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
            { EffectTypeEnum.Damage, () => new DamageEffect() },
            { EffectTypeEnum.Petrification, () => new PetrificationEffect() },
            { EffectTypeEnum.Growing, () => new GrowingEffect() },
            { EffectTypeEnum.Heal, () => new HealEffect() },
            { EffectTypeEnum.Floating, () => new FloatingEffect() },
            { EffectTypeEnum.Weak, () => new WeakEffect() },
            { EffectTypeEnum.Slowdown, () => new SlowdownEffect() },
            { EffectTypeEnum.PoorRecovery, () => new PoorRecoveryEffect() },
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