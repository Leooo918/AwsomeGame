using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EncyPotionRecipe : MonoBehaviour
{
    [SerializeField] private Image _potionIcon;
    [SerializeField] private Image _potionType;
    [SerializeField] private Image[] _potionRecipes;
    [SerializeField] private TextMeshProUGUI _description;
    [SerializeField] private Sprite[] _potionTypeSprite;
    [SerializeField] private PotionRecipeListSO _potionRecipeListSO;
    [SerializeField] private ItemListSO _itemListSO;

    public void Init(Potion potionItem = null)
    {
        if (potionItem == null)
        {
            _potionIcon.color = new Color(1, 1, 1, 0);
            _potionType.color = new Color(1, 1, 1, 0);
            for (int i = 0; i < 3; i++)
            {
                _potionRecipes[i].color = new Color(1, 1, 1, 0);
            }
            _description.SetText("");
        }
        else
        {
            _potionIcon.color = Color.white;
            _potionType.color = Color.white;

            _potionIcon.sprite = potionItem.potionItemSO.image;
            if (potionItem.potionItemSO is ThrowPotionItemSO)
                _potionType.sprite = _potionTypeSprite[0];
            else
                _potionType.sprite = _potionTypeSprite[1];
            IngredientItemType[] ingredientItemType = _potionRecipeListSO.GetPotionRecipe(potionItem.potionItemSO);
            for (int i = 0; i < potionItem.level; i++)
            {
                _potionRecipes[i].color = Color.white;
                _potionRecipes[i].sprite = _itemListSO.GetIngredientItemSO(ingredientItemType[i]).image;
            }
            _description.SetText(potionItem.potionItemSO.GetItemDescription(potionItem.level));
        }
    }
}
