using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffectManager
{
    private Entity _owner;

    private Dictionary<StatusEffectEnum, StatusEffect> _statusEffectDictionary;
    private List<StatusEffect> _enableEffects;

    public StatusEffectManager(Entity owner)
    {
        _owner = owner;
        _enableEffects = new List<StatusEffect>();
        _statusEffectDictionary = new Dictionary<StatusEffectEnum, StatusEffect>();
        foreach (StatusEffectEnum effectEnum in Enum.GetValues(typeof(StatusEffectEnum)))
        {
            string enumName = effectEnum.ToString();
            try
            {
                Type t = Type.GetType($"{enumName}StatusEffect");
                StatusEffect effect = Activator.CreateInstance(t) as StatusEffect;

                _statusEffectDictionary.Add(effectEnum, effect);
            }
            catch (Exception ex)
            {
                Debug.Log($"{enumName}");
            }
        }
    }

    public void UpdateStatusEffects()
    {
        for (int i = _enableEffects.Count - 1; i >= 0; i--)
        {
            var effect = _enableEffects[i];
            if (effect.IsCompleted())
            {
                effect.OnEnd();
                _enableEffects.RemoveAt(i);
            }
            else
            {
                effect.UpdateEffect();
            }
        }
    }

    public void AddStatusEffect(StatusEffectEnum statusEffect, int level, float cooltime)
    {
        StatusEffect effect = _statusEffectDictionary[statusEffect];
        effect.SetInfo(level);
        effect.ApplyEffect(_owner, cooltime);
        _enableEffects.Add(effect);
    }
}
