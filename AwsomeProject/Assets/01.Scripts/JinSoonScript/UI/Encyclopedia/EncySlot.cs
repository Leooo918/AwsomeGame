using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EncySlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public bool isLocked;

    private Potion _assignedPotion;
    private Image _potionSprite;

    public void init(Potion potion)
    {
        _assignedPotion = potion;
        _potionSprite = _assignedPotion.GetComponent<Image>();
        Lock();
    }

    public void Unlock()
    {
        isLocked = false;
        _potionSprite.color = Color.white;
    }
    public void Lock()
    {
        isLocked = true;
        _potionSprite.color = Color.black;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (isLocked) return;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (isLocked) return;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (isLocked) return;
    }
}
