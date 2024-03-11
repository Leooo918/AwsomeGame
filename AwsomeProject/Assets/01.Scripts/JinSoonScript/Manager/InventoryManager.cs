using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    private Inventory inventory;
    public Item curMovingItem { get; private set; }
    public InventorySlot curCheckingSlot { get; private set; }

    [SerializeField] private Transform itemParent;

    private void Awake()
    {
        if (Instance != null)
            Destroy(Instance);

        Instance = this;
    }

    public void MoveItem(Item item) => curMovingItem = item;
    public void CheckSlot(InventorySlot slot) => curCheckingSlot = slot;


}
