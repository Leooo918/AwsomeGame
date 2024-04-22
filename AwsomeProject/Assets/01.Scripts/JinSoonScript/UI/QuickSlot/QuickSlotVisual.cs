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
        portion = itemObj.GetComponent<PortionItem>();

        //�������̶� �巡�׾ص���� �� �� �־ �������� ����
        itemObj.GetComponent<Image>().raycastTarget = false;
        portion.Init(1, null);
    }

    public void UseItem()
    {
        Debug.Log(portion.posionEffect);
        AssignedPortion = null;
        portion.RemoveItem(1);
        //���⼭ ���� ȿ���� �ߵ��ǵ��� �������
        //�Ʒ��ڵ�� �ϴ� �ӽ÷� �ڱ����� �ߵ������� ȿ���� �ϵ���
        //PlayerManager.Instance.Player.healthCompo.GetEffort(portion.posionEffect, portion.posionEffect.duration);
    }

    public void DeleteItem()
    {
        AssignedPortion = null;
        Destroy(itemObj);
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
