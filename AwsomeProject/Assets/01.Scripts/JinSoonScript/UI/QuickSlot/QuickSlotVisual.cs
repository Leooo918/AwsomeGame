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
        //�̹� �������� �Ҵ��������� return
        if (AssignedPortion != null || item == null) return;

        AssignedPortion = item as PortionItemSO;

        itemObj = Instantiate(AssignedPortion.prefab, transform);
        itemObj.GetComponent<RectTransform>().anchoredPosition = offset;
        itemObj.transform.SetSiblingIndex(1);
        portion = itemObj.GetComponent<PortionItem>();

        //�������̶� �巡�׾ص���� �� �� �־ �������� ����
        itemObj.GetComponent<Image>().raycastTarget = false;
        portion.Init(1, null);
    }

    public void UseItem()
    {
        Debug.Log(portion.portionEffect);
        AssignedPortion = null;
        portion.RemoveItem(1);
        //���⼭ ���� ȿ���� �ߵ��ǵ��� �������
        //�Ʒ��ڵ�� �ϴ� �ӽ÷� �ڱ����� �ߵ������� ȿ���� �ϵ���
        if (portion.portionType == Portion.PortionForMyself)
        {
            PlayerManager.Instance.Player.healthCompo.GetEffort(portion.portionEffect);
            //���⼭ �߰��� ����Ʈ�� �׷��� ���� �� �ְ� �������
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
