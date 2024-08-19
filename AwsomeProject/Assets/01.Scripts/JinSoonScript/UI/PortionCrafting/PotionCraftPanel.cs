using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionCraftPanel : MonoBehaviour, IManageableUI
{
    public void Close()
    {
        gameObject.SetActive(false);
        PlayerManager.Instance.EnablePlayerMovementInput();
        PlayerManager.Instance.EnablePlayerInventoryInput();
    }

    public void Open()
    {
        //뭔가 dotween으로 맛있는 거 만드는거는 나중에
        gameObject.SetActive(true);
        PlayerManager.Instance.DisablePlayerMovementInput();
        PlayerManager.Instance.DisablePlayerInventoryInput();
    }

    public void Init()
    {
        gameObject.SetActive(false);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Close();
        }
    }
}
