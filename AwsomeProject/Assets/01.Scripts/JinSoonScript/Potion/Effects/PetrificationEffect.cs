using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetrificationEffect : Effect
{
    public override void ApplyEffect()
    {
        SpawnEffect(2);
        CameraManager.Instance.ShakeCam(5f, 9f, 0.05f);

        foreach (var target in _affectedTargets)
        {
            if (target is Entity entity)
            {
                entity.ApplyStatusEffect(StatusDebuffEffectEnum.Petrification, _level, 3f);
            }
        }
    }

    private void SpawnEffect(int count)
    {
        Vector3 scale = Vector3.one * (_potion.potionItemSO as ThrowPotionItemSO).GetPotionInfo(_level).range / 2;
        for (int i = 0; i < count; i++)
        {
            Transform effectTrm = GameObject.Instantiate(EffectInstantiateManager.Instance.stonHitEffect, _potion.transform.position, Quaternion.identity).transform;
            effectTrm.localScale = scale;
        }
    }
}
