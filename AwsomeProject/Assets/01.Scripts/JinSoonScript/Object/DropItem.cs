using TMPro;
using UnityEngine;

public class DropItem : MonoBehaviour
{
    public ItemSO item;
    private Rigidbody2D rb;
    private PopUpPanel _popUp;
    private bool interacting;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        _popUp = UIManager.Instance.panelDictionary[UIType.PopUp] as PopUpPanel;
    }

    private void Update()
    {
        if (interacting && Input.GetKeyDown(KeyCode.F))
        {
            Item i = InventoryManager.Instance.MakeItemInstanceByItemSO(item);
            if (InventoryManager.Instance.PlayerInventory.TryInsertItem(i))
            {
                DropItemManager.Instance.IndicateItemPanel(i.itemSO);
                Destroy(gameObject);
            }
            else Destroy(i);
        }
    }

    public void SpawnItem(Vector2 dir)
    {
        rb.AddForce(dir, ForceMode2D.Impulse);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Player player))
        {
            _popUp.SetText("아이템 줍기 [F]");
            UIManager.Instance.Open(UIType.PopUp);
            interacting = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Player>(out Player player))
        {
            UIManager.Instance.Close(UIType.PopUp);
            interacting = false;
        }
    }
}
