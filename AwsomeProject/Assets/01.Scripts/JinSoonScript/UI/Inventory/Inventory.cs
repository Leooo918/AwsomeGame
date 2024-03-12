using System.IO;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public InventorySlot[,] inventory = new InventorySlot[6, 5];
    private string path = "";
    [SerializeField] private Transform slotParent;

    private void Awake()
    {
        path = Path.Combine(Application.dataPath, "SaveDatas\\Inventory.json");
    }

    private void Start()
    {
        for (int i = 0; i < inventory.GetLength(1); i++)
        {
            for (int j = 0; j < inventory.GetLength(0); j++)
            {
                inventory[j, i] = slotParent.GetChild(i * 6 + j).GetComponent<InventorySlot>();
            }
        }

        Load();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            Save();
        }
    }

    /// <summary>
    /// �κ��丮�� �������� ���ų��� �� ����� �Լ��Ӥ���
    /// </summary>
    /// <param name="item">�κ��丮�� ���ų��� ������</param>
    /// <returns>�������� ���� �� return���� true�� �ʵ��� item �ν��Ͻ� �����ָ� �ǰ�
    /// return���� false�� �ʵ��� item �ν��Ͻ��� ������ ������ ��</returns>
    public bool TryInsertItem(Item item, int amount)
    {
        int id = item.itemSO.id;
        int remainItem = amount;

        for (int i = 0; i < inventory.GetLength(0); i++)
        {
            for (int j = 0; j < inventory.GetLength(1); j++)
            {
                //���� �������� �ִ��� Ȯ��
                //���� �������� �ִٸ� �� �������� ��ĭ �ִ� �������� �������Ⱦ�������
                //�� ĭ�� �־��ֱ�
                Item it = inventory[i, j].assignedItem;
                if (it != null && it.itemSO.id == id && it.itemAmount > amount)
                {
                    amount -= (it.itemSO.maxCarryAmountPerSlot - it.itemAmount);
                    it.AddItem(amount);
                    if (amount <= 0)
                    {
                        Destroy(item);
                        return true;
                    }
                }
            }
        }



        for (int i = 0; i < inventory.GetLength(0); i++)
        {
            for (int j = 0; j < inventory.GetLength(1); j++)
            {
                //�� ������ ���� �ʰ� ���� �������� ���⼭ Null�� ĭ�� ã�� �� ��
                Item it = inventory[i, j].assignedItem;
                if (it == null)
                {
                    inventory[i, j].InsertItem(it);
                    return true;
                }
            }
        }

        //���� �κ��丮�� �� ���� ���ٸ� return false
        return false;
    }

    //�� id���� ���� �̸����� �޵� ���������ؼ� inventory���� �������� return ����

    public Item GetItem(int id)
    {
        for (int i = 0; i < inventory.GetLength(0); i++)
        {
            for (int j = 0; j < inventory.GetLength(1); j++)
            {
                Item it = inventory[i, j].assignedItem;
                if (it != null && it.itemSO.id == id)
                {
                    return it;
                }
            }
        }

        return null;
    }

    public void Save()
    {
        InventorySaveData saveData = new InventorySaveData();

        for (int i = 0; i < inventory.GetLength(0); i++)
        {
            for (int j = 0; j < inventory.GetLength(1); j++)
            {
                if (inventory[i, j].assignedItem != null)
                {
                    saveData.inventory[i, j].amount =
                        inventory[i, j].assignedItem.itemAmount;

                    saveData.inventory[i, j].id =
                        inventory[i, j].assignedItem.itemSO.id;
                }
                else
                {
                    saveData.inventory[i, j].amount = -1;
                    saveData.inventory[i, j].id = -1;
                }
            }
        }

        string json = JsonUtility.ToJson(saveData, true);

        File.WriteAllText(path, json);
    }

    public void Load()
    {
        InventorySaveData saveData = new InventorySaveData();

        if (!File.Exists(path))
        {
            Save();
            return;
        }

        string json = File.ReadAllText(path);
        saveData = JsonUtility.FromJson<InventorySaveData>(json);

        for (int i = 0; i < saveData.inventory.GetLength(0); i++)
        {
            for (int j = 0; j < saveData.inventory.GetLength(1); j++)
            {
                int id = saveData.inventory[i, j].id;
                ItemSetSO itemSet = InventoryManager.Instance.ItemSet;
                GameObject itemPf;
                if (id == -1) continue;

                for (int k = 0; k < itemSet.itemset.Count; k++)
                {
                    if (itemSet.itemset[k].id == id)
                    {
                        itemPf = itemSet.itemset[k].prefab;
                        Item it = Instantiate(itemPf, InventoryManager.Instance.itemParent).GetComponent<Item>();

                        inventory[i, j].InsertItem(it);
                        Debug.Log($"{j} {i} : {it}");
                    }
                }

            }
        }
    }
}

public class InventorySaveData
{
    public ItemStruct[,] inventory = new ItemStruct[6, 5];
}

public struct ItemStruct
{
    public int id;
    public int amount;
}
