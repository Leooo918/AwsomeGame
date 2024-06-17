using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public InventorySlot[,] inventory = new InventorySlot[5, 4];
    [SerializeField] private Vector2Int inventorySize = new Vector2Int(5, 4);
    private string path = "";
    public Action<ItemSO> OnSelectItem;

    [SerializeField] private Transform slotParent;
    [SerializeField] private Transform itemParent;
    [SerializeField] private InventorySlot slotPf;

    [SerializeField] private bool indicateIngredient = false;
    [SerializeField] private bool indicatePortion = false;

    private void Awake()
    {
        path = Path.Combine(Application.dataPath, "SaveDatas\\Inventory.json");
        inventory = new InventorySlot[inventorySize.x, inventorySize.y];

        for (int i = 0; i < inventory.GetLength(1); i++)
        {
            for (int j = 0; j < inventory.GetLength(0); j++)
            {
                inventory[j, i] = Instantiate(slotPf, slotParent);
                inventory[j, i].Init(this);
            }
        }
    }

    private void Start()
    {
        StartCoroutine(DelayLoad());
    }

    private void Update()
    {
        //����׿� �ڵ�� 
        if (Input.GetKeyDown(KeyCode.P))
        {
            Save();
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            Load();
        }
    }

    private void OnDisable()
    {
        for (int i = 0; i < inventory.GetLength(0); i++)
        {
            for (int j = 0; j < inventory.GetLength(1); j++)
            {
                if (inventory[i, j].assignedItem != null)
                {
                    inventory[i, j].assignedItem.gameObject.SetActive(false);
                }
            }
        }
    }

    private void OnEnable()
    {
        for (int i = 0; i < inventory.GetLength(0); i++)
        {
            for (int j = 0; j < inventory.GetLength(1); j++)
            {
                if (inventory[i, j].assignedItem != null)
                {
                    inventory[i, j].assignedItem.gameObject.SetActive(true);
                }

            }
        }
    }

    IEnumerator DelayLoad()
    {
        yield return null;
        Load();
    }

    /// <summary>
    /// �κ��丮�� �������� ���ų��� �� ����� �Լ��Ӥ���
    /// </summary>
    /// <param name="item">�κ��丮�� ���ų��� ������</param>
    /// <returns>�������� ���� �� return���� true�� �ʵ��� item �ν��Ͻ� �����ָ� �ǰ�
    /// return���� false�� �ʵ��� item �ν��Ͻ��� ������ ������ ��</returns>
    public bool TryInsertItem(Item item)
    {
        int id = item.itemSO.id;
        int remainItem = item.itemAmount;

        for (int i = 0; i < inventory.GetLength(0); i++)
        {
            for (int j = 0; j < inventory.GetLength(1); j++)
            {
                //���� �������� �ִ��� Ȯ��
                //���� �������� �ִٸ� �� �������� ��ĭ �ִ� �������� �������Ⱦ�������
                //�� ĭ�� �־��ֱ�
                Item it = inventory[i, j].assignedItem;
                if (it != null && it.itemSO.id == id)
                {
                    int remainSpace = it.itemSO.maxCarryAmountPerSlot - it.itemAmount;
                    if (remainSpace - remainItem < 0)
                    {
                        remainItem -= remainSpace;
                        it.AddItem(remainItem - remainSpace);
                    }
                    else
                    {
                        it.AddItem(remainItem);

                        Destroy(item.gameObject);
                        Save();
                        return true;
                    }

                }
            }
        }


        for (int i = 0; i < inventory.GetLength(1); i++)
        {
            for (int j = 0; j < inventory.GetLength(0); j++)
            {
                //�� ������ ���� �ʰ� ���� �������� ���⼭ Null�� ĭ�� ã�� �� ��
                Item it = inventory[j, i].assignedItem;
                if (it == null)
                {
                    inventory[j, i].InsertItem(item);
                    Save();
                    return true;
                }
            }
        }

        //���� �κ��丮�� �� ���� ���ٸ� return false
        Save();
        return false;
    }

    //�� id���� ���� �̸����� �޵� ���������ؼ� inventory���� �������� return ����

    public bool GetItem(int id, int amount, out Item item)
    {
        item = null;
        for (int i = 0; i < inventory.GetLength(0); i++)
        {
            for (int j = 0; j < inventory.GetLength(1); j++)
            {
                Item it = inventory[i, j].assignedItem;
                if (it != null && it.itemSO.id == id)
                {
                    item = it;
                    amount -= it.itemAmount;

                    if (amount <= 0) return true;
                }
            }
        }
        return amount <= 0;
    }

    public void UnSelectAllSlot()
    {
        for (int i = 0; i < inventory.GetLength(0); i++)
        {
            for (int j = 0; j < inventory.GetLength(1); j++)
            {
                inventory[i, j].UnSelect();
            }
        }
    }

    public void Save()
    {
        InventorySaveData saveData = new();

        for (int i = 0; i < inventory.GetLength(0); i++)
        {
            for (int j = 0; j < inventory.GetLength(1); j++)
            {
                if (inventory[i, j].assignedItem != null)
                {
                    ItemStruct itemS;
                    itemS.amount = inventory[i, j].assignedItem.itemAmount;
                    itemS.id = inventory[i, j].assignedItem.itemSO.id;

                    saveData.inventory.Add(itemS);
                }
            }
        }

        string json = JsonUtility.ToJson(saveData, true);
        File.WriteAllText(path, json);
    }

    public void Load()
    {
        InventorySaveData saveData;

        if (!File.Exists(path))
        {
            Save();
            return;
        }

        string json = File.ReadAllText(path);
        saveData = JsonUtility.FromJson<InventorySaveData>(json);

        //���� �ִ°� �� �����
        for (int i = 0; i < inventory.GetLength(1); i++)
        {
            for (int j = 0; j < inventory.GetLength(0); j++)
            {
                if (inventory[j, i].assignedItem != null)
                    Destroy(inventory[j, i].assignedItem.gameObject);
            }
        }

        for (int i = 0; i < saveData.inventory.Count; i++)
        {
            ItemStruct itemStruct = saveData.inventory[i];
            int id = itemStruct.id;

            ItemSetSO itemSet = InventoryManager.Instance.ItemSet;
            GameObject itemPf;
            if (id == -1) continue;

            for (int k = 0; k < itemSet.itemset.Count; k++)
            {
                if (itemSet.itemset[k].id != id) continue;
                if (itemSet.itemset[k].itemType == ItemType.Portion && indicatePortion == false) continue;
                if (itemSet.itemset[k].itemType == ItemType.Ingredient && indicateIngredient == false) continue;

                bool b = false;
                for (int l = 0; l < inventory.GetLength(0); l++)
                {
                    for (int j = 0; j < inventory.GetLength(1); j++)
                    {
                        if (inventory[l, j].assignedItem == null)
                        {
                            itemPf = itemSet.itemset[k].prefab;
                            Item it = Instantiate(itemPf, itemParent).GetComponent<Item>();

                            it.Init(itemStruct.amount, inventory[l, j]);
                            inventory[l, j].InsertItem(it);

                            b = true;
                            break;
                        }
                    }
                    if (b) break;
                }
            }
        }
    }

    public class InventorySaveData
    {
        public List<ItemStruct> quickSlot = new();
        public List<ItemStruct> inventory = new();
    }

    [System.Serializable]
    public struct ItemStruct
    {
        public int id;
        public int amount;
    }
}
