using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityAttack : MonoBehaviour
{
    [SerializeField] private Entity _entity;

    [SerializeField] private int _maxDetectingEnemyNum;
    [SerializeField] private float _attackRange;
    [SerializeField] private Vector2 _attackOffset;
    [SerializeField] private LayerMask _whatIsEnemy;

    private Vector2 _knockBackPower;
    private Vector2 _moveDesire;
    private int _damage = 1;
    private Collider2D[] _colls;

    private void Awake()
    {
        _colls = new Collider2D[_maxDetectingEnemyNum];
        _entity = GetComponent<Entity>();
    }

    /// <summary>
    /// Use With Function SetCurrentAttackInfo
    /// </summary>
    public void Attack()
    {
        int detected = Physics2D.OverlapCircleNonAlloc(transform.position + new Vector3(_attackOffset.x * _entity.FacingDir, _attackOffset.y, 0), _attackRange, _colls, _whatIsEnemy);
        bool isCameraShaked = false;

        Vector2 currentMoveDesire = _moveDesire;
        currentMoveDesire.x *= _entity.FacingDir;

        _entity.rigidbodyCompo.AddForce(currentMoveDesire, ForceMode2D.Impulse);
        for (int i = 0; i < detected; i++)
        {
            if (_colls[i].TryGetComponent<Entity>(out Entity e))
            {
                _knockBackPower.x *= Mathf.Sign(e.transform.position.x - transform.position.x);
                e.healthCompo.TakeDamage(_damage, _knockBackPower, _entity);

                if (!isCameraShaked)
                {
                    CameraManager.Instance.ShakeCam(1f, 1f, 0.05f);
                    isCameraShaked = true;
                }
            }
        }
    }

    public void SetCurrentAttackInfo(AttackInfo attackInfo)
    {
        _attackRange = attackInfo.radius;
        _attackOffset = attackInfo.offset;
        _knockBackPower = attackInfo.knockBackPower;
        _moveDesire = attackInfo.moveDesire;
        _damage = attackInfo.damage;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        //Gizmos.DrawWireSphere(transform.position + new Vector3(_attackOffset.x * _entity.FacingDir, _attackOffset.y, 0), _attackRange);
    }
}
