using UnityEngine;
using UnityEngine.UI;

public class ShopUI : MonoBehaviour
{
    [SerializeField] private Image selectedItemOn;
    [SerializeField] private Image[] items;
    //[SerializeField] private GameObject[] itemsBtn;

    public void ItemSelect()
    {
        for (int i = 0; i < items.Length; i++)
        {
            selectedItemOn.sprite = items[i].sprite;
        }
    }
}
