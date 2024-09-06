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

    protected override void Start()
    {
        base.Start();
        _colliders = new Collider2D[_maxDetactEntity];
    }

    public override void UsePotion()
    {
        int count = Physics2D.OverlapCircleNonAlloc(transform.position, range, _colliders, _whatIsEnemy);
        List<Entity> list = new List<Entity>();
        if(count > 0)
        {
            foreach(Collider2D coll in _colliders)
            {
                if(coll.TryGetComponent(out Entity entity))
                {
                    list.Add(entity);
                    
                }
            }
            foreach(Effect effect in effects)
            {
                effect.SetAffectedTargets(list);
                effect.ApplyEffect();
            }

        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Destroy(gameObject);
        UsePotion();
    }
}
