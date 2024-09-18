using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NatureSyncStatusEffect : StatusEffect
{
    private Player _player;
    public override void ApplyEffect(Entity target, float cooltime)
    {
        base.ApplyEffect(target, cooltime);

        _player = target as Player;

        _player.isNatureSync = true;
        if (level >= 1)
            _player.canAttackWithNatureSync = true;
        if (level >= 2)
            _player.OnKilled += HandleKilledEvent;
    }

    private void HandleKilledEvent(Entity entity)
    {
        _cooltime += 0.2f;
    }

    public override void UpdateEffect()
    {
        base.UpdateEffect();

        if (_player.isNatureSync == false)
        {
            _cooltime = 0;
        }
    }

    public override void OnEnd()
    {
        base.OnEnd();

        if (level >= 2)
            _player.OnKilled -= HandleKilledEvent;

        _player.isNatureSync = false;
        _player.canAttackWithNatureSync = false;
    }
}
