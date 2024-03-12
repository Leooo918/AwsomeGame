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
    /// 인벤토리에 아이템을 쑤셔넣을 때 사용할 함수임ㅇㅇ
    /// </summary>
    /// <param name="item">인벤토리에 쑤셔넣을 아이템</param>
    /// <returns>아이템을 얻을 때 return값이 true면 필드의 item 인스턴스 지워주면 되고
    /// return값이 false면 필드의 item 인스턴스를 지우지 않으면 됨</returns>
    public bool TryInsertItem(Item item, int amount)
    {
        int id = item.itemSO.id;
        int remainItem = amount;

        for (int i = 0; i < inventory.GetLength(0); i++)
        {
            for (int j = 0; j < inventory.GetLength(1); j++)
            {
                //같은 아이템이 있는지 확인
                //같은 아이템이 있다면 그 아이템의 한칸 최대 수량보다 적을동안아이템을
                //그 칸에 넣어주기
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
                //저 위에서 들어가지 않고 남은 아이템은 여기서 Null인 칸을 찾아 들어가 줌
                Item it = inventory[i, j].assignedItem;
                if (it == null)
                {
                    inventory[i, j].InsertItem(it);
                    return true;
                }
            }
        }

        //만약 인벤토리에 빈 곳이 없다면 return false
        return false;
    }

    //뭐 id값을 받은 이름으로 받든 어찌저찌해서 inventory에서 아이템을 return 해줌

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
