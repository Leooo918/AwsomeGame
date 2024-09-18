using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;



public class ThrowPotion : Potion
{
    private Collider2D[] _colliders;
    [SerializeField]
    private int _maxDetactEntity;
    [SerializeField]
    private LayerMask _whatIsEnemy;
    public int range;

    private SpriteRenderer _spriteRenderer;

    public override void Init(QuickSlot slot, Entity owner) 
    {
        ThrowPotionItemSO throwPotionItemSO = slot.assignedItem.itemSO as ThrowPotionItemSO;
        ThrowPotionInfos potionInfos = throwPotionItemSO.GetPotionInfo(level);
        _maxDetactEntity = potionInfos.maxDetactEntity;
        _whatIsEnemy = potionInfos.whatIsEnemy;
        range = potionInfos.range;
        base.Init(slot, owner);

        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.sprite = throwPotionItemSO.image;
        _colliders = new Collider2D[_maxDetactEntity];
    }

    public override void UsePotion()
    {
        int count = Physics2D.OverlapCircleNonAlloc(transform.position, range, _colliders, _whatIsEnemy);
        List<IAffectable> list = new List<IAffectable>();
        if(count > 0)
        {
            for (int i = 0; i < count; i++)
            {
                if (_colliders[i].TryGetComponent(out IAffectable entity))
                {
                    list.Add(entity);
                    
                }
            }
        }
        foreach (Effect effect in effects)
        {
            effect.SetAffectedTargets(list);
            effect.ApplyEffect();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Destroy(gameObject);
        UsePotion();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
