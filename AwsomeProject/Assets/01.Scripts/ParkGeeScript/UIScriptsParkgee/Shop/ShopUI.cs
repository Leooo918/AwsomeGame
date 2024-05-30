using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class ShopUI : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Image _selectedItemOn;
    [SerializeField] private TextMeshProUGUI _priceTxt;
    [SerializeField] private TextMeshProUGUI _descriptTxt;
    [SerializeField, TextArea] private string _menual;
    [SerializeField] private int _price;

    public void OnPointerClick(PointerEventData eventData)
    {
        // 클릭한 객체가 이미지인지 확인해주기
        if (eventData.pointerCurrentRaycast.gameObject.GetComponent<Image>() != null)
        {
            // 클릭한 이미지를 변수로저장
            Image clickedImage = eventData.pointerCurrentRaycast.gameObject.GetComponent<Image>();

            _selectedItemOn.sprite = clickedImage.sprite;
            _descriptTxt.text = _menual;

            _priceTxt.text = $"{_price}원";
        }
    }
}
