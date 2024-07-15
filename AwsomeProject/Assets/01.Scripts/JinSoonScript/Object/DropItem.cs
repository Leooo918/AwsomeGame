using TMPro;
using UnityEngine;

public class DropItem : MonoBehaviour
{
    public ItemSO item;
    private Rigidbody2D rb;
    private GameObject txt;
    private bool interacting;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        txt = UIManager.Instance.PressFMessageObj;
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
        if (collision.TryGetComponent<Player>(out Player player))
        {
            txt.SetActive(true);
            interacting = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Player>(out Player player))
        {
            txt.SetActive(false);
            interacting = false;
        }
    }
}
