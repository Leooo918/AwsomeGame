using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapleTree : MonoBehaviour
{
    [SerializeField] private GameObject _interact;
    [SerializeField] private DropItem[] _dropItemPrefabs;
    Collider2D col;

    private void Awake()
    {
        col = GetComponent<Collider2D>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            //UIManager.Instance.GuideOn();
            _interact.SetActive(true);
            if (Input.GetKeyDown(KeyCode.F))
            {
                int randomIdx = Random.Range(0, _dropItemPrefabs.Length);
                DropItem randomDropItem = _dropItemPrefabs[randomIdx];

                Instantiate(randomDropItem, transform.position, Quaternion.identity);
                col.enabled = false;
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
}
