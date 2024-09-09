using Doryu;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionCraftPanel : MonoBehaviour, IManageableUI
{
    private RectTransform _rectTrm;
    private RectTransform _inventoryRectTrm;
    [SerializeField] private InventoryPanel _inventory;
    [SerializeField] private Pot _pot;

    public void Close() 
    {
        _rectTrm.anchoredPosition = Vector2.right * 3000;
        _inventoryRectTrm.anchoredPosition = Vector2.down * 1500;
        _inventory.OnPotUI(false);
        _pot.ReturnItem(); 
        //gameObject.SetActive(false);
        PlayerManager.Instance.EnablePlayerMovementInput();
        PlayerManager.Instance.EnablePlayerInventoryInput();
    }

    public void Open()
    {
        //���� dotween���� ���ִ� �� ����°Ŵ� ���߿�
        _rectTrm.anchoredPosition = Vector2.zero;
        _inventoryRectTrm.anchoredPosition = Vector2.right * 960;
        _inventory.OnPotUI(true);
        //gameObject.SetActive(true);
        PlayerManager.Instance.DisablePlayerMovementInput();
        PlayerManager.Instance.DisablePlayerInventoryInput();
    }

    public void Init()
    {
        _rectTrm = transform as RectTransform;
        _rectTrm.anchoredPosition = Vector2.right * 3000;
        _inventoryRectTrm = _inventory.transform as RectTransform;
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
