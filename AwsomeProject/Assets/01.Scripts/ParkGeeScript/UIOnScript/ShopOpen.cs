using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopOpen : MonoBehaviour
{
    [SerializeField] private GameObject _interact;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private float detectionRadius = 5f;

    private bool isPlayerInRange = false;

    private void Update()
    {
        CheckPlayerDistance();
    }

    private void CheckPlayerDistance()
    {
        float distance = Vector2.Distance(transform.position, playerTransform.position);

        if (distance <= detectionRadius)
        {
            if (!isPlayerInRange)
            {
                isPlayerInRange = true;
                OnPlayerEnter();
            }

            if (Input.GetKeyDown(KeyCode.F))
            {
                UIManager.Instance.Open(UIType.Shop);
            }
        }
        else
        {
            if (isPlayerInRange)
            {
                isPlayerInRange = false;
                OnPlayerExit();
            }
        }
    }

    private void OnPlayerEnter()
    {
        _interact.SetActive(true);
        //UIManager.Instance.GuideOn();
    }

    private void OnPlayerExit()
    {
        _interact.SetActive(false);
        //UIManager.Instance.GuideOff();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1, 0, 0, 0.5f);
        Gizmos.DrawSphere(transform.position, detectionRadius);
    }
}
