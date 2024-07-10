using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class MysteryPortionIndicator : MonoBehaviour
{
    [SerializeField] private MysteryPortionInventory _inventory;
    [SerializeField] private Transform _portionParent;

    private RectTransform _rect;
    private Player _player;

    private PortionItem _portion;
    private PortionItemSO _mysteryPortion;

    private bool _isSelectedMysteryPortion = false;
    private Tween _slotTween;

    private void Awake()
    {
        _player = PlayerManager.Instance.Player;
        _rect = GetComponent<RectTransform>();
    }

    private void OnEnable()
    {
        _player.PlayerInput.SelectMysteryPortion += SelectMysteriyPortion;
        _player.PlayerInput.SelectQuickSlot += UnSelectMysteryPortion;
        _player.PlayerInput.OnUseQuickSlot += UseMysteryPortion;
    }

    private void OnDisable()
    {
        _player.PlayerInput.SelectMysteryPortion -= SelectMysteriyPortion;
        _player.PlayerInput.SelectQuickSlot -= UnSelectMysteryPortion;
        _player.PlayerInput.OnUseQuickSlot -= UseMysteryPortion;
    }

    public void ChangePortionImage(ItemSO itemSO)
    {
        if (_portion != null)
            Destroy(_portion.gameObject);

        _portion = InventoryManager.Instance.MakeItemInstanceByItemSO(itemSO) as PortionItem;

        if (_portion != null)
        {
            _mysteryPortion = itemSO as PortionItemSO;
            _portion.transform.SetParent(_portionParent);
        }
    }

    private void UseMysteryPortion()
    {
        if (_isSelectedMysteryPortion == false) return;

        switch (_portion.portionType)
        {
            case Portion.PortionForMyself:
                PlayerManager.Instance.Player.healthCompo.GetEffort(_portion.portionEffect);
                break;
            case Portion.PortionForThrow:
                PlayerManager.Instance.Player.ThrowPortion(_portion);
                break;
            case Portion.Flask:
                PlayerManager.Instance.Player.WeaponEnchant(_portion);
                break;
        }
    }

    private void SelectMysteriyPortion()
    {
        if(_isSelectedMysteryPortion == true)
        {
            UnSelectMysteryPortion();
            return;
        }

        _isSelectedMysteryPortion = true;

        if (_slotTween != null && _slotTween.active)
            _slotTween.Kill();

        _slotTween = _rect.DOAnchorPosY(-445, 0.5f);
    }

    private void UnSelectMysteryPortion(int num = 69)
    {
        _isSelectedMysteryPortion = false;

        if (_slotTween != null && _slotTween.active)
            _slotTween.Kill();

        _slotTween = _rect.DOAnchorPosY(-500, 0.5f);
    }
}
