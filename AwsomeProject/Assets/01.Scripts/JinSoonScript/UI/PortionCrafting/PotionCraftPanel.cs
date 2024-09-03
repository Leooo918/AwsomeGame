using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionCraftPanel : MonoBehaviour, IManageableUI
{
    private RectTransform _rectTrm;

    public void Close()
    {
        _rectTrm.anchoredPosition = Vector2.right * 3000;
        //gameObject.SetActive(false);
        PlayerManager.Instance.EnablePlayerMovementInput();
        PlayerManager.Instance.EnablePlayerInventoryInput();
    }

    public void Open()
    {
        //���� dotween���� ���ִ� �� ����°Ŵ� ���߿�
        _rectTrm.anchoredPosition = Vector2.zero;
        //gameObject.SetActive(true);
        PlayerManager.Instance.DisablePlayerMovementInput();
        PlayerManager.Instance.DisablePlayerInventoryInput();
    }

    public void Init()
    {
        _rectTrm = transform as RectTransform;
        _rectTrm.anchoredPosition = Vector2.right * 3000;
        //gameObject.SetActive(false);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Close();
        }
    }
}
