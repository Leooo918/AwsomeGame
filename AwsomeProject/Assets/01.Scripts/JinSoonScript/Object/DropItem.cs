using TMPro;
using UnityEngine;

public class DropItem : MonoBehaviour
{
    public ItemSO item;
    private Rigidbody2D rb;
    private TextMeshProUGUI txt;
    private bool interacting;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        txt = GameObject.Find("PressFAlram").GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        if (interacting && Input.GetKeyDown(KeyCode.F))
        {
            Item i = InventoryManager.Instance.MakeItemInstanceByItemSO(item);
            if (InventoryManager.Instance.PlayerInventory.TryInsertItem(i))
                Destroy(gameObject);
            else Destroy(i);
        }
    }

    public void SpawnItem(Vector2 dir)
    {
        rb.AddForce(dir, ForceMode2D.Impulse);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent<Player>(out Player player))
        {
            txt.enabled = true;
            interacting = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Player>(out Player player))
        {
            txt.enabled = false;
            interacting = false;
        }
    }
}
