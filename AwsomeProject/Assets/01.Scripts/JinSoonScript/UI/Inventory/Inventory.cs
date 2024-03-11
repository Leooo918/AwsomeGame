using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public Item[,] inventory = new Item[6, 5];

    /// <summary>
    /// 인벤토리에 아이템을 쑤셔넣을 때 사용할 함수임ㅇㅇ
    /// </summary>
    /// <param name="item">인벤토리에 쑤셔넣을 아이템</param>
    /// <returns>아이템을 얻을 때 return값이 true면 필드의 item 인스턴스 지워주면 되고
    /// return값이 false면 필드의 item 인스턴스를 지우지 않으면 됨</returns>
    public bool TryInsertItem(Item item)
    {
        for(int i = 0; i < inventory.GetLength(0); i++)
        {
            for(int j = 0; j < inventory.GetLength(1); j++)
            {
                //같은 아이템이 있는지 확인
                //같은 아이템이 있다면 그 아이템의 한칸 최대 수량보다 적을동안아이템을
                //그 칸에 넣어주기
            }
        }

        for (int i = 0; i < inventory.GetLength(0); i++)
        {
            for (int j = 0; j < inventory.GetLength(1); j++)
            {
                //저 위에서 들어가지 않고 남은 아이템은 여기서 Null인 칸을 찾아 들어가 줌
            }
        }

        //만약 인벤토리에 빈 곳이 없다면 return false
        return false;
    }

    //뭐 id값을 받은 이름으로 받든 어찌저찌해서 inventory에서 아이템을 return 해줌

    //public Item GetItem()
    //{

    //}
}
