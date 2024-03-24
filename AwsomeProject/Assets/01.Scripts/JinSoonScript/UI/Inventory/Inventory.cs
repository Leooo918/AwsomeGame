using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using static UnityEditor.Progress;

public class Inventory : MonoBehaviour
{
    public InventorySlot[,] inventory = new InventorySlot[5, 4];
    public InventorySlot[] quickSlot = new InventorySlot[5];
    private string path = "";
    [SerializeField] private Transform slotParent;
    [SerializeField] private Transform quickSlotParent;


    [SerializeField] private ItemSO testItem;

    private void Awake()
    {
        path = Path.Combine(Application.dataPath, "SaveDatas\\Inventory.json");

        for (int i = 0; i < inventory.GetLength(1); i++)
        {
            for (int j = 0; j < inventory.GetLength(0); j++)
            {
                inventory[j, i] = slotParent.GetChild(i * 5 + j).GetComponent<InventorySlot>();
                inventory[j, i].Init(this);
            }
        }

        for (int i = 0; i < 5; i++)
        {
            quickSlot[i] = quickSlotParent.GetChild(i).GetComponent<InventorySlot>();
            quickSlot[i].Init(this);
        }
    }

    private void Start()
    {
        StartCoroutine(DelayLoad());
    }

    private void Update()
    {
        //디버그용 코드들 
        if (Input.GetKeyDown(KeyCode.P))
        {
            Save();
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            Load();
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            ititi = Instantiate(testItem.prefab, GameObject.Find("Items").transform).GetComponent<Item>();
            ititi.Init(1, null);
            StartCoroutine("DelayInsert");
        }
    }
    Item ititi;
    IEnumerator DelayInsert()
    {
        yield return null;
        TryInsertItem(ititi);
    }

    IEnumerator DelayLoad()
    {
        yield return null;
        Load();
    }

    /// <summary>
    /// 인벤토리에 아이템을 쑤셔넣을 때 사용할 함수임ㅇㅇ
    /// </summary>
    /// <param name="item">인벤토리에 쑤셔넣을 아이템</param>
    /// <returns>아이템을 얻을 때 return값이 true면 필드의 item 인스턴스 지워주면 되고
    /// return값이 false면 필드의 item 인스턴스를 지우지 않으면 됨</returns>
    public bool TryInsertItem(Item item)
    {
        int id = item.itemSO.id;
        int remainItem = item.itemAmount;

        for (int i = 0; i < inventory.GetLength(0); i++)
        {
            for (int j = 0; j < inventory.GetLength(1); j++)
            {
                //같은 아이템이 있는지 확인
                //같은 아이템이 있다면 그 아이템의 한칸 최대 수량보다 적을동안아이템을
                //그 칸에 넣어주기
                Item it = inventory[i, j].assignedItem;
                if (it != null && it.itemSO.id == id)
                {
                    int remainSpace = it.itemSO.maxCarryAmountPerSlot - it.itemAmount;

                    Debug.Log(remainSpace + " " + remainItem);

                    if (remainSpace - remainItem < 0)
                    {
                        remainItem = remainItem - remainSpace;
                        it.AddItem(remainItem - remainSpace);
                    }
                    else
                    {
                        it.AddItem(remainItem);

                        Destroy(item.gameObject);
                        return true;
                    }

                }
            }
        }

        Debug.Log("밍");

        for (int i = 0; i < inventory.GetLength(1); i++)
        {
            for (int j = 0; j < inventory.GetLength(0); j++)
            {
                //저 위에서 들어가지 않고 남은 아이템은 여기서 Null인 칸을 찾아 들어가 줌
                Item it = inventory[j, i].assignedItem;
                if (it == null)
                {
                    inventory[j, i].InsertItem(item);
                    return true;
                }
            }
        }

        //만약 인벤토리에 빈 곳이 없다면 return false
        return false;
    }

    //뭐 id값을 받은 이름으로 받든 어찌저찌해서 inventory에서 아이템을 return 해줌

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

                    if(amount <= 0) return true;
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

        for (int i = 0; i < quickSlot.Length; i++)
        {
            quickSlot[i].UnSelect();
        }
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
                    ItemStruct itemS = new ItemStruct();
                    itemS.amount = inventory[i, j].assignedItem.itemAmount;
                    itemS.id = inventory[i, j].assignedItem.itemSO.id;
                    itemS.posX = i;
                    itemS.posY = j;

                    saveData.inventory.Add(itemS);
                }
            }
        }

        for (int i = 0; i < quickSlot.Length; i++)
        {
            if (quickSlot[i].assignedItem != null)
            {
                ItemStruct itemS = new ItemStruct();
                itemS.amount = quickSlot[i].assignedItem.itemAmount;
                itemS.id = quickSlot[i].assignedItem.itemSO.id;
                itemS.posX = i;

                saveData.quickSlot.Add(itemS);
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

        for (int i = 0; i < saveData.inventory.Count; i++)
        {
            ItemStruct itemStruct = saveData.inventory[i];
            int id = itemStruct.id;

            ItemSetSO itemSet = InventoryManager.Instance.ItemSet;
            GameObject itemPf;
            if (id == -1) continue;

            for (int k = 0; k < itemSet.itemset.Count; k++)
            {
                if (itemSet.itemset[k].id == id)
                {
                    itemPf = itemSet.itemset[k].prefab;
                    Item it = Instantiate(itemPf, InventoryManager.Instance.itemParent).GetComponent<Item>();

                    Debug.Log(itemStruct);
                    it.Init(itemStruct.amount, inventory[itemStruct.posX, itemStruct.posY]);

                    inventory[itemStruct.posX, itemStruct.posY].InsertItem(it);
                }
            }
        }

        for (int i = 0; i < saveData.quickSlot.Count; i++)
        {
            ItemStruct itemStruct = saveData.quickSlot[i];
            int id = itemStruct.id;

            ItemSetSO itemSet = InventoryManager.Instance.ItemSet;
            GameObject itemPf;
            if (id == -1) continue;

            for (int k = 0; k < itemSet.itemset.Count; k++)
            {
                if (itemSet.itemset[k].id == id)
                {
                    itemPf = itemSet.itemset[k].prefab;
                    Item it = Instantiate(itemPf, InventoryManager.Instance.itemParent).GetComponent<Item>();

                    Debug.Log(itemStruct);
                    it.Init(itemStruct.amount, quickSlot[itemStruct.posX]);

                    quickSlot[itemStruct.posX].InsertItem(it);
                }
            }
        }
    }



    public class InventorySaveData
    {
        public List<ItemStruct> quickSlot = new List<ItemStruct>();
        public List<ItemStruct> inventory = new List<ItemStruct>();
    }

    [System.Serializable]
    public struct ItemStruct
    {
        public int id;
        public int amount;
        public int posX;
        public int posY;
    }
}
