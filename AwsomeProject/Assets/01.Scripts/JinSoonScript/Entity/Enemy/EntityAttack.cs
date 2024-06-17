using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityAttack : MonoBehaviour
{
    [SerializeField] private Entity entity;

    [SerializeField] private int maxDetectingEnemyNum;
    [SerializeField] private float attackRange;
    [SerializeField] private Vector2 attackOffset;
    [SerializeField] private LayerMask whatIsEnemy;

    private Vector2 knockBackPower;
    private int damage = 1;
    private Collider2D[] colls;

    private void Awake()
    {
        colls = new Collider2D[maxDetectingEnemyNum];
        entity = GetComponent<Entity>();
    }

    /// <summary>
    /// Use With Function SetCurrentAttackInfo
    /// </summary>
    public void Attack()
    {
        int detected = Physics2D.OverlapCircleNonAlloc(transform.position + new Vector3(attackOffset.x * entity.FacingDir, attackOffset.y, 0), attackRange, colls, whatIsEnemy);
        bool isCameraShaked = false;

        for (int i = 0; i < detected; i++)
        {
            if (colls[i].TryGetComponent<Entity>(out Entity e))
            {
                knockBackPower.x *= Mathf.Sign(e.transform.position.x - transform.position.x);
                e.healthCompo.TakeDamage(damage, knockBackPower, entity);

                if(!isCameraShaked)
                {
                    CameraManager.Instance.ShakeCam(1f, 1f, 0.05f);
                    isCameraShaked = true;
                }
            }
        }
    }

    public void SetCurrentAttackInfo(AttackInfo attackInfo)
    {
        attackRange = attackInfo.radius;
        attackOffset = attackInfo.offset;
        knockBackPower = attackInfo.knockBackPower;
        damage = attackInfo.damage;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + new Vector3(attackOffset.x * entity.FacingDir, attackOffset.y, 0), attackRange);
    }
}
