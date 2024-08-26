using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapleTree : MonoBehaviour
{
    [SerializeField] private GameObject _interact;
    [SerializeField] private DropItem[] _dropItemPrefabs;
    [SerializeField] private float _detectionRadius = 5f;
    private Animator _animator;
    Collider2D _col;

    private Transform _playerTrm;
    private bool _isPlayerInRange = false;
    private bool _isAttack = false;

    private void Awake()
    {
        _col = GetComponent<Collider2D>();
        _animator = GetComponent<Animator>();
        _playerTrm = PlayerManager.Instance.PlayerTrm;
    }

    private void Update()
    {
        CheckInRange();
    }

    private void CheckInRange()
    {
        float distance = Vector2.Distance(transform.position, _playerTrm.position);

        if(distance <=_detectionRadius)
        {
            if(!_isPlayerInRange && !_isAttack)
            {
                _isPlayerInRange = true;
                OnPlayerEnter();
            }
            if(Input.GetMouseButtonDown(0) && !_isAttack)
            {
                _isAttack = true;
                StartCoroutine(PerformHit());
            }
        }
        else
        {
            if(_isPlayerInRange)
            {
                _isPlayerInRange = false;
                OnPlayerExit();
            }
        }
    }

    private void OnPlayerEnter()
    {
        _interact.SetActive(true);
    }

    private void OnPlayerExit()
    {
        _interact.SetActive(false);
    }

    //private void OnTriggerStay2D(Collider2D collision)
    //{
    //    if (collision.gameObject.CompareTag("Player"))
    //    {
    //        _interact.SetActive(true);
    //        if (Input.GetMouseButtonDown(0))
    //        {
    //            StartCoroutine(PerformHit());
    //        }
    //    }
    //}

    //private void OnTriggerExit2D(Collider2D collision)
    //{
    //    if (collision.gameObject.CompareTag("Player"))
    //    {
    //        _interact.SetActive(false);
    //    }
    //}

    private IEnumerator PerformHit()
    {
        _animator.SetBool("Hit", true);

        int randomIdx = Random.Range(0, _dropItemPrefabs.Length);
        DropItem randomDropItem = _dropItemPrefabs[randomIdx];
        Instantiate(randomDropItem, transform.position, Quaternion.identity);

        _col.enabled = false;

        yield return new WaitForSeconds(1f);
        _animator.SetBool("Hit", false);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1f, 0, 0, 0.5f);
        Gizmos.DrawSphere(transform.position, _detectionRadius);
    }
}
