using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PutOpen : MonoBehaviour
{
    [SerializeField] private GameObject _f;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            //UIManager.Instance.GuideOn();
            _f.SetActive(true);
            if (Input.GetKeyDown(KeyCode.F))
            {
                UIManager.Instance.Open(Window.Shop);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            _f.SetActive(false);
            //UIManager.Instance.GuideOff();
        }
    }
}
