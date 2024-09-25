using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Encyclopedia : MonoBehaviour
{
    [SerializeField] private EncyPotionRecipe _encyPotionRecipe;
    [SerializeField] private Transform _potionSlotTrm;

    private EncySlot[] _potionSlots;

    private void Awake()
    {
        _encyPotionRecipe.Init();
    }
}
