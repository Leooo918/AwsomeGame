using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public Item[,] inventory = new Item[6, 5];

    /// <summary>
    /// �κ��丮�� �������� ���ų��� �� ����� �Լ��Ӥ���
    /// </summary>
    /// <param name="item">�κ��丮�� ���ų��� ������</param>
    /// <returns>�������� ���� �� return���� true�� �ʵ��� item �ν��Ͻ� �����ָ� �ǰ�
    /// return���� false�� �ʵ��� item �ν��Ͻ��� ������ ������ ��</returns>
    public bool TryInsertItem(Item item)
    {
        for(int i = 0; i < inventory.GetLength(0); i++)
        {
            for(int j = 0; j < inventory.GetLength(1); j++)
            {
                //���� �������� �ִ��� Ȯ��
                //���� �������� �ִٸ� �� �������� ��ĭ �ִ� �������� �������Ⱦ�������
                //�� ĭ�� �־��ֱ�
            }
        }

        for (int i = 0; i < inventory.GetLength(0); i++)
        {
            for (int j = 0; j < inventory.GetLength(1); j++)
            {
                //�� ������ ���� �ʰ� ���� �������� ���⼭ Null�� ĭ�� ã�� �� ��
            }
        }

        //���� �κ��丮�� �� ���� ���ٸ� return false
        return false;
    }

    //�� id���� ���� �̸����� �޵� ���������ؼ� inventory���� �������� return ����

    //public Item GetItem()
    //{

    //}
}
