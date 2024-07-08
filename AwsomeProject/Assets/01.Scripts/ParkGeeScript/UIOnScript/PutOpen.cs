using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PutOpen : MonoBehaviour
{
    [SerializeField] private GameObject _interact;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            //UIManager.Instance.GuideOn();
            _interact.SetActive(true);
            if (Input.GetKeyDown(KeyCode.F))
            {
                UIManager.Instance.Open(UIType.PotionCraft);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            _interact.SetActive(false);
            //UIManager.Instance.GuideOff();
        }
    }
}
