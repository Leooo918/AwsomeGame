using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class QuickSlotVisual : MonoBehaviour
{
    public PortionItemSO AssignedPortion { get; private set; }
    private PortionItem portion;
    private GameObject itemObj;

    private RectTransform rect;
    private Tween tween;
    private Vector3 offset = new Vector3(0f, 7f, 0f);

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
    }

    public void SetItem(ItemSO item)
    {
        //이미 아이템이 할당해있으면 return
        if (AssignedPortion != null || item == null) return;

        AssignedPortion = item as PortionItemSO;

        itemObj = Instantiate(AssignedPortion.prefab, transform);
        itemObj.GetComponent<RectTransform>().anchoredPosition = offset;
        itemObj.transform.SetSiblingIndex(1);
        portion = itemObj.GetComponent<PortionItem>();

        //아이템이라서 드래그앤드롭이 될 수 있어서 안잡히게 해줘
        itemObj.GetComponent<Image>().raycastTarget = false;
        portion.Init(1, null);
    }

    public void UseItem()
    {
        Debug.Log(portion.portionEffect);
        AssignedPortion = null;
        portion.RemoveItem(1);
        //여기서 포션 효과가 발동되도록 해줘야함
        //아래코드는 일단 임시로 자기한테 발동가능한 효과만 하도록
        if (portion.portionType == Portion.PortionForMyself)
        {
            PlayerManager.Instance.Player.healthCompo.GetEffort(portion.portionEffect);
            //여기서 추가로 이펙트나 그런거 나올 수 있게 해줘야함
        }
        else if (portion.portionType == Portion.PortionForThrow)
        {
            PlayerManager.Instance.Player.ThrowPortion(portion);
        }
        else
            PlayerManager.Instance.Player.WeaponEnchant(portion);
    }

    public void DeleteItem()
    {
        AssignedPortion = null;
        if (itemObj != null) Destroy(itemObj);
    }

    public void EnableSlot()
    {
        if (tween != null && tween.active)
            tween.Kill();

        tween = rect.DOAnchorPosY(-30f, 0.5f);
    }

    public void DisableSlot()
    {
        if (tween != null && tween.active)
            tween.Kill();

        tween = rect.DOAnchorPosY(-90f, 0.5f);
    }
}
