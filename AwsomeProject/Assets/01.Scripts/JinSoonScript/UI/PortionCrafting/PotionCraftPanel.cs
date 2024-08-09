using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionCraftPanel : MonoBehaviour, IManageableUI
{
    public void Close()
    {
        gameObject.SetActive(false);
        PlayerManager.Instance.EnablePlayerMovementInput();
    }

    public void Open()
    {
        //���� dotween���� ���ִ� �� ����°Ŵ� ���߿�
        gameObject.SetActive(true);
        PlayerManager.Instance.DisablePlayerMovementInput();
    }

    public void Init()
    {
        gameObject.SetActive(false);
    }
}
