using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortionTable : MonoBehaviour
{
    private PortionCraftingIngredientSlot[] ingredientsSlot = new PortionCraftingIngredientSlot[5];
    private Portion portionType = Portion.PortionForThrow;
    [SerializeField] private RectTransform portionTrm;
    [SerializeField] private RectTransform portionTableTrm;
    [SerializeField] private GameObject portionResult;

    private void Awake()
    {
        for (int i = 0; i < ingredientsSlot.Length; i++)
        {
            ingredientsSlot[i] = transform.GetChild(i).GetComponent<PortionCraftingIngredientSlot>();
        }
    }

    public void MakePortion()
    {
        List<EffectInfo> effects = new List<EffectInfo>();

        for (int i = 0; i < 5; i++)
        {
            if (ingredientsSlot[i].assignedItem != null)
            {
                IngredientItemSO item = ingredientsSlot[i].assignedItem.itemSO as IngredientItemSO;
                EffectInfo effect = new EffectInfo();
                effect.effect = item.effectType;
                effect.requirePoint = item.effectPoint;

                effects.Add(effect);
            }
        }

        PortionManager.Instance.portionSet.FindMakeablePortion(effects.ToArray(),portionType, out PortionItemSO portion);
        
        if(portion == null)
        {
            Debug.Log("포션이 존재하지 않는www");
            return;
        }
        Item itemInstance = InventoryManager.Instance.MakeItemInstanceByItemSO(portion);

        if (InventoryManager.Instance.PlayerInventory.TryInsertItem(itemInstance) == false)
        {
            Debug.Log("인벤토리 자리없는데숭");
        }
        else
        {
            List<IngredientItemSO> ingredients = new List<IngredientItemSO>();
            for (int i = 0; i < 5; i++)
            {
                if (ingredientsSlot[i].assignedItem != null)
                {
                    ingredients.Add(ingredientsSlot[i].assignedItem.itemSO as IngredientItemSO);
                    Destroy(ingredientsSlot[i].assignedItem.gameObject);
                }
            }

            RecipeManager.Instance.AddRecipe(ingredients.ToArray(), portion);
        }

        Sequence seq = DOTween.Sequence();

        seq.Append(portionTableTrm.DOAnchorPosY(-1080, 0.2f))
            .Append(portionResult.transform.DOScale(Vector3.one, 0.4f).SetEase(Ease.OutBounce));
    }

    public void ChangePortionType(bool down)
    {
        if (down) portionType--;
        else portionType++;

        portionType = (Portion)Mathf.Clamp((int)portionType, 0, 3);

        portionTrm.DOAnchorPosX((int)portionType * 150f, 0.5f);
    }
}
