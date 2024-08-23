using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class Herbs : MonoBehaviour
{
    [SerializeField] private GameObject interact;
    [SerializeField] private IngredientItemSO item;
    [SerializeField] private Sprite gatheredImage;

    public UnityEvent<float> OnGatheringHerb;

    private SpriteRenderer spriteRenderer;
    private Transform pivot;
    private Player player;

    private float gatherTime = 0;

    private float gatherTimeDown = 0;

    private bool gatherStart = false;
    private bool gatherEnd = false;
    private bool isTriggered = false;

    public bool herbGathered { get; private set; } = false;

    private void Awake()
    {
        spriteRenderer = transform.Find("Visual").GetComponent<SpriteRenderer>();
        pivot = interact.transform.Find("Pivot");

        if (item == null) return;
        gatherTime = item.gatheringTime;
    }


    private void Update()
    {
        if (gatherStart)
        {
            gatherTimeDown += Time.deltaTime;
            OnGatheringHerb?.Invoke(gatherTimeDown);
            float progress = gatherTimeDown / gatherTime;
            progress = Mathf.Clamp01(progress);
            pivot.localScale = new Vector3(1, progress, 1);

            if (gatherTime <= gatherTimeDown)
                GetHurb();
        }

        if (gatherEnd)
        {
            gatherTimeDown -= Time.deltaTime * 10;

            float progress = gatherTimeDown / gatherTime;
            progress = Mathf.Clamp01(progress);
            pivot.localScale = new Vector3(1, progress, 1);

            if (0 >= gatherTimeDown)
                CancelGathering();
        }
    }

    private void GetHurb()
    {
        if (herbGathered) return;

        GameManager.Instance.gatherCnt++;
        player.StateMachine.ChangeState(PlayerStateEnum.Idle);
        interact.SetActive(false);
        herbGathered = true;

        IngredientItem iItem = Instantiate(item.prefab, InventoryManager.Instance.itemParent).GetComponent<IngredientItem>();
        iItem.SetItemAmount(1);
        bool canAddItem = InventoryManager.Instance.PlayerInventory.TryInsertItem(iItem);
        
        if(canAddItem)
        {
            ItemGatherPanel gather = UIManager.Instance.GetUI(UIType.ItemGather) as ItemGatherPanel;
            gather.Init(iItem.itemSO);
            gather.Open();
        }
        else
        {
            StartCoroutine(DelayClosePopUp());
        }

        spriteRenderer.sprite = gatheredImage;
    }

    private IEnumerator DelayClosePopUp()
    {
        PopUpPanel popUp = UIManager.Instance.GetUI(UIType.PopUp) as PopUpPanel;
        popUp.SetText("�κ��丮�� ��á���ϴ�");
        popUp.Open();
        yield return new WaitForSeconds(2f);
        popUp.Close();

    }

    private void CancelGathering()
    {
        if (isTriggered == false)
            interact.SetActive(false);

        gatherTimeDown = 0;
        gatherStart = false;
        gatherEnd = false;
    }

    private void GatherHerb()
    {
        if (herbGathered) return;

        player.StateMachine.ChangeState(PlayerStateEnum.Gathering);
        gatherStart = true;
        gatherEnd = false;
    }

    private void CancleGathering()
    {
        player.StateMachine.ChangeState(PlayerStateEnum.Idle);
        gatherEnd = true;
        gatherStart = false;
    }

    public void ReduceTime(float value)
    {
        gatherTimeDown -= value;
        gatherTimeDown = Mathf.Clamp(gatherTimeDown, 0, gatherTime);

        if (gatherTime <= 0) CancelGathering();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Player>(out player) && herbGathered == false)
        {
            interact.SetActive(true);
            player.PlayerInput.InteractPress += GatherHerb;
            player.PlayerInput.InteractRelease += CancleGathering;


            isTriggered = true;
            gatherStart = false;
            gatherEnd = false;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Player>(out player) && herbGathered == false)
        {
            CancleGathering();
            player.PlayerInput.InteractPress -= GatherHerb;
            player.PlayerInput.InteractRelease -= CancleGathering;

            isTriggered = false;
        }
    }
}
