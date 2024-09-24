using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HornSpikeEffect : Effect
{
    private int[] _damageWithLevel = { 3, 5, 10 };
    private float[] _sizeWithLevel = { 1f, 1.5f, 2f };

    public override void ApplyEffect()
    {
        HornSpike hornSpike = GameObject.Instantiate(PlayerManager.Instance.Player.hornSpike, _potion.transform.position, Quaternion.identity);
        ThrowPotion throwPotion = _potion as ThrowPotion;
        hornSpike.Init(_potion.owner, throwPotion.GetVelocity(), throwPotion.GetSize(), _damageWithLevel[_level], _sizeWithLevel[_level], 10f, _level >= 1);
    }
}
