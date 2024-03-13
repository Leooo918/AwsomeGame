using DG.Tweening;
using UnityEngine;

public class SettingPanel : MonoBehaviour
{
    //지금 너무 코드 별론데 최대한 머리짜내봄 미안해
    [SerializeField] private GameObject[] uiPrefab;
    [SerializeField] private GameObject canvas;

    private GameObject setPanelInstance;
    private GameObject inventoryInstance;

    private RectTransform rectTransform;

    private Sequence seq;

    private void Awake()
    {
        seq = DOTween.Sequence();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (setPanelInstance == null)
            {
                Time.timeScale = 0;
                setPanelInstance = Instantiate(uiPrefab[0]) as GameObject;
                setPanelInstance.transform.SetParent(canvas.transform, false);
                rectTransform = setPanelInstance.GetComponent<RectTransform>();
                rectTransform.localPosition = new Vector3(1000, -12, 0);
                //rectTransform.sizeDelta = new Vector2(200, 200);
                Debug.Log(setPanelInstance);
            }
            else
            {
                Time.timeScale = 1;
                Destroy(setPanelInstance);
            }
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (inventoryInstance == null)
            {
                Time.timeScale = 0;
                inventoryInstance = Instantiate(uiPrefab[1]) as GameObject;
                inventoryInstance.transform.SetParent(canvas.transform, false);
                rectTransform = inventoryInstance.GetComponent<RectTransform>();
                rectTransform.sizeDelta = new Vector2(1600,800);
                Debug.Log(inventoryInstance);
            }
            else
            {
                Time.timeScale = 1;
                Destroy(inventoryInstance);
            }
        }
    }

    /*private void UIOn(GameObject ui, GameObject[] prefab, int array)
    {


    }

    private void UIOff(GameObject ui)
    {

    }
    일단 살려놓음*/
}
