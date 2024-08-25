using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapleTree : MonoBehaviour
{
    [SerializeField] private GameObject _interact;
    [SerializeField] private DropItem[] _dropItemPrefabs;
    private Animator _animator;
    Collider2D _col;

    private Transform _playerTrm;
    private bool _isPlayerInRange;

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

    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            _interact.SetActive(true);
            if (Input.GetKeyDown(KeyCode.F))
            {
                StartCoroutine(PerformHit());
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            _interact.SetActive(false);
        }
    }

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
}
